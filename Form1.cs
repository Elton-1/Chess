using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using Boardlib;

namespace Chess
{
    public partial class Form1 : Form
    {
        private bool first = true;
        private bool KingWasPreviouslyClicked = false;
        private const int BoardSize = 8;
        private Button[,] chessButtons;
        private Board Board = null;
        private Tuple<int, int> MoveFrom = null;

        public Color WhiteSquares = Color.FromArgb(235, 236, 208);
        public Color DarkSquares = Color.FromArgb(109, 139, 70);

        private PieceType? PieceType = null;
        private Label label1 = null;
        private Label label2 = null;
        private TextBox panelPositionInfo = null;
        private Panel panel = null;
        private TrackBar difficultyBar = null;
        private Label difficultyLabel = null;
        private Panel borderLeft = null;
        private Panel ThemeBtn = null;

        private bool engineThinking = false;

        int buttonSize = 80;

        public Form1()
        {
            InitializeComponent();

            // Set the initial state to Maximized (full screen)
            this.WindowState = FormWindowState.Maximized;

            InitializeChessboard();
            this.BackColor = WhiteSquares;
            this.Resize += Form1_Resize;
            _ = Play();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            CenterChessboard();
            AddComponents(false);
        }

        private async Task Play()
        {
            Random rn = new Random();

            Board board = new Board(Boardlib.PieceType.WHITE);
            if (rn.Next(2) == 0)
                board = new Board(Boardlib.PieceType.BLACK);

            PieceType = board.PlayerPieceType;
            AddComponents();
            Print(board);
            Board = board;
            UpdateLabelPoints();

            if (PieceType == Boardlib.PieceType.BLACK)
            {
                await PlayEngine();
            }
            Print(Board);
        }

        private void InitializeChessboard()
        {
            chessButtons = new Button[BoardSize, BoardSize];

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    Button button = new Button
                    {
                        Width = buttonSize,
                        Height = buttonSize,
                        Location = new Point(col * buttonSize, row * buttonSize),
                        Tag = new Tuple<int, int>(row, col), // Store row and col in Tag for later use
                        BackColor = (row + col) % 2 == 0 ? WhiteSquares : DarkSquares,
                        ForeColor = (row + col) % 2 == 0 ? WhiteSquares : DarkSquares,
                        FlatStyle = FlatStyle.Flat
                    };

                    button.Click += ChessButton_ClickAsync;

                    Controls.Add(button);
                    chessButtons[row, col] = button;
                }
            }

            // Calculate the total size of the chessboard and center it in the form
            int boardTotalSize = buttonSize * BoardSize;
            
            this.MinimumSize = new Size(BoardSize * buttonSize + SystemInformation.FrameBorderSize.Width * 2,
                                BoardSize * buttonSize + SystemInformation.FrameBorderSize.Height * 2 + 40);
            CenterToScreen();
            CenterChessboard();

        }

        private void CenterChessboard()
        {
            int totalWidth = BoardSize * buttonSize;
            int totalHeight = BoardSize * buttonSize;

            int xOffset = (ClientSize.Width - totalWidth) / 2;
            int yOffset = (ClientSize.Height - totalHeight) / 2;

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    chessButtons[row, col].Location = new Point(col * buttonSize + xOffset, row * buttonSize + yOffset);
                }
            }
        }


        private async void ChessButton_ClickAsync(object sender, EventArgs e)
        {
            if (PieceType == Boardlib.PieceType.WHITE && Board.WhitesTurn || PieceType == Boardlib.PieceType.BLACK && !Board.WhitesTurn)
            {
                //Reset the btn styles
                ResetBtnStyle();

                // Handle button click if needed
                Button clickedButton = (Button)sender;
                Tuple<int, int> position = (Tuple<int, int>)clickedButton.Tag;
                int row = position.Item1;
                int col = position.Item2;

                if (!first && Board.getSquares()[row, col].PieceType == this.PieceType && Board.getSquares()[row, col].Type != SquareContent.ROCK
                    || Board.getSquares()[row, col].Type == SquareContent.ROCK && Board.getSquares()[row, col].PieceType == this.PieceType && (!KingWasPreviouslyClicked || Board.KingMoved))
                    first = true;

                if (Board.getSquares()[row, col].Type == SquareContent.KING && Board.getSquares()[row, col].PieceType == PieceType)
                    KingWasPreviouslyClicked = true;
                else
                    KingWasPreviouslyClicked = false;

                if (first)
                {
                    MoveFrom = new Tuple<int, int>(row, col);

                    //Highlight the validMoves
                    Square piece = Board.getSquares()[position.Item1, position.Item2];
                    HighlightValidMoves(Board, piece);
                }
                else
                {
                    if (MoveFrom != null)
                    {
                        bool moved = false;
                        Board.MovePiece(MoveFrom.Item1, MoveFrom.Item2, row, col, ref moved);
                        if(Board != null) UpdateLabelPoints();
                        Print(Board);

                        if (!moved) first = true;
                        else
                        {
                            AddMoveToInfo(MoveFrom.Item1, MoveFrom.Item2, row, col);
                            if (Board.Won())
                            {
                                Print(Board);
                                MessageBox.Show("You Won!");
                                DialogResult result = MessageBox.Show("Do you want to save the game position have played?", "Save Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (result == DialogResult.Yes) SaveGame();
                                await Play();
                                return;
                            }
                            else if (Board.Draw())
                            {
                                Print(Board);
                                MessageBox.Show("Draw!");
                                DialogResult result = MessageBox.Show("Do you want to save the game position have played?", "Save Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (result == DialogResult.Yes) SaveGame();
                                await Play();
                                return;
                            }

                            await PlayEngine();

                            if (Board != null) UpdateLabelPoints();
                        }
                        if (Board.Draw())
                        {
                            Print(Board);
                            MessageBox.Show("Draw!");
                            DialogResult result = MessageBox.Show("Do you want to save the game position have played?", "Save Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes) SaveGame();
                            await Play();
                            return;
                        }
                        else if (Board.Lost())
                        {
                            Print(Board);
                            MessageBox.Show("You Lost!");
                            DialogResult result = MessageBox.Show("Do you want to save the game position have played?", "Save Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes) SaveGame();
                            await Play();
                            return;
                        }
                    }

                }

                first = !first;
                Print(Board);
            }
        }

        private void Print(Board board)
        {
            foreach (Control control in Controls)
            {
                if (control is Button button && button.Text == String.Empty)
                {
                    button.BackgroundImage = null;
                    Tuple<int, int> position = (Tuple<int, int>)button.Tag;
                    Square piece = board.getSquares()[position.Item1, position.Item2];

                    InitalizeSquare(piece, button);
                }
            }
        }

        private static int count = 1;
        private void SaveGame()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Title = "Select a Text File";
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            saveFileDialog.FileName = $"ChessGameResults{count}.txt";

            if (saveFileDialog.ShowDialog() != DialogResult.OK) MessageBox.Show("Operation Terminated");
            else
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                {
                    sw.Write(panelPositionInfo.Text);
                }

                count++;
            }
        }

        private async Task PlayEngine()
        {
            ChessEngine engine = new ChessEngine("Stockfish/Stockfish.exe");

            engineThinking = true;
            String bestMove = await engine.GetBestMove(Board.GetFen(), difficultyBar.Value * 10);
            char[] letters = bestMove.ToCharArray();
            if (Board.PlayerPieceType == Boardlib.PieceType.WHITE)
            {
                Board.MoveOpponent(Board.ConvertRowAndCol(letters[1], letters[0]).X, Board.ConvertRowAndCol(letters[1], letters[0]).Y, Board.ConvertRowAndCol(letters[3], letters[2]).X, Board.ConvertRowAndCol(letters[3], letters[2]).Y);
                AddMoveToInfo(Board.ConvertRowAndCol(letters[1], letters[0]).X, Board.ConvertRowAndCol(letters[1], letters[0]).Y, Board.ConvertRowAndCol(letters[3], letters[2]).X, Board.ConvertRowAndCol(letters[3], letters[2]).Y);
            }
            else
            {
                Board.MoveOpponent(Board.ConvertRowAndColBlack(letters[1], letters[0]).X, Board.ConvertRowAndColBlack(letters[1], letters[0]).Y, Board.ConvertRowAndColBlack(letters[3], letters[2]).X, Board.ConvertRowAndColBlack(letters[3], letters[2]).Y);
                AddMoveToInfo(Board.ConvertRowAndColBlack(letters[1], letters[0]).X, Board.ConvertRowAndColBlack(letters[1], letters[0]).Y, Board.ConvertRowAndColBlack(letters[3], letters[2]).X, Board.ConvertRowAndColBlack(letters[3], letters[2]).Y);
            }
            engineThinking = false;
        }

        private void InitalizeSquare(Square square, Button btn)
        {
            switch (square.Type)
            {
                case SquareContent.EMPTY: break;
                case SquareContent.PAWN when square.PieceType == Boardlib.PieceType.BLACK: btn.BackgroundImage = Image.FromFile("Images/BlackPawn.png"); break;
                case SquareContent.BISHOP when square.PieceType == Boardlib.PieceType.BLACK: btn.BackgroundImage = Image.FromFile("Images/BlackBishop.png"); break;
                case SquareContent.KNIGHT when square.PieceType == Boardlib.PieceType.BLACK: btn.BackgroundImage = Image.FromFile("Images/BlackKnight.png"); break;
                case SquareContent.QUEEN when square.PieceType == Boardlib.PieceType.BLACK: btn.BackgroundImage = Image.FromFile("Images/BlackQueen.png"); break;
                case SquareContent.KING when square.PieceType == Boardlib.PieceType.BLACK: btn.BackgroundImage = Image.FromFile("Images/BlackKing.png"); break;
                case SquareContent.ROCK when square.PieceType == Boardlib.PieceType.BLACK: btn.BackgroundImage = Image.FromFile("Images/BlackRock.png"); break;

                case SquareContent.PAWN when square.PieceType == Boardlib.PieceType.WHITE: btn.BackgroundImage = Image.FromFile("Images/WhitePawn.png"); break;
                case SquareContent.BISHOP when square.PieceType == Boardlib.PieceType.WHITE: btn.BackgroundImage = Image.FromFile("Images/WhiteBishop.png"); break;
                case SquareContent.KNIGHT when square.PieceType == Boardlib.PieceType.WHITE: btn.BackgroundImage = Image.FromFile("Images/WhiteKnight.png"); break;
                case SquareContent.QUEEN when square.PieceType == Boardlib.PieceType.WHITE: btn.BackgroundImage = Image.FromFile("Images/WhiteQueen.png"); break;
                case SquareContent.KING when square.PieceType == Boardlib.PieceType.WHITE: btn.BackgroundImage = Image.FromFile("Images/WhiteKing.png"); break;
                case SquareContent.ROCK when square.PieceType == Boardlib.PieceType.WHITE: btn.BackgroundImage = Image.FromFile("Images/WhiteRock.png"); break;
            }

            btn.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void HighlightValidMoves(Board board, Square square)
        {
            foreach (var valid in board.BoardValidMoves.validMoves)
            {
                if (valid.Item1 == square)
                {
                    String tag = "(" + valid.Item2.ToString() + ", " + valid.Item3.ToString() + ")";
                    foreach (Control control in this.Controls)
                    {
                        if (control is Button button && button.Tag != null && button.Tag.ToString() == tag)
                        {
                            var color = button.BackColor;

                            if (color != GetViewedColor(DarkSquares, 30) && color != GetViewedColor(DarkSquares, 30))
                            {
                                button.BackColor = GetViewedColor(color, 30);
                            }
                            else
                            {
                                //Reset
                                button.BackColor = GetPrevColor(color, 30);
                            }
                        }
                    }
                }
            }
        }

        private Color GetPrevColor(Color clr, int tone)
        {
            Color depest = clr;
            for(int i = 0; i <= tone; i++)
            {
                if (clr.R + i > 255 || clr.G + i > 255 || clr.B + i > 255) break;

                depest = Color.FromArgb(clr.R + i, clr.G + i, clr.B + i);
            }

            return depest;
        }

        private void ResetBtnStyle()
        {
            foreach (Control control in Controls)
            {
                if (control is Button button && button.Text == String.Empty)
                {
                    Tuple<int, int> position = (Tuple<int, int>)button.Tag;
                    button.BackColor = (position.Item1 + position.Item2) % 2 == 0 ? WhiteSquares : DarkSquares;
                    button.ForeColor = (position.Item1 + position.Item2) % 2 == 0 ? WhiteSquares : DarkSquares;
                }
            }
        }

        private int Evaluation()
        {
            int eval = 0;
            for(int i = 0; i < Board.COL; i++)
            {
                for(int j = 0; j < Board.ROW; j++)
                {
                    if (Board.getSquares()[i, j].PieceType == Board.getOpponentPieceType()) eval -= GetPointForPiece(Board.getSquares()[i, j]);
                    else eval += GetPointForPiece(Board.getSquares()[i, j]);
                }
            }

            return eval;
        }

        private int GetPointForPiece(Square piece)
        {
            switch(piece.Type)
            {
                case SquareContent.PAWN: return 1;
                case SquareContent.BISHOP: return 3;
                case SquareContent.KNIGHT: return 3;
                case SquareContent.ROCK: return 4;
                case SquareContent.QUEEN: return 5;
            }

            return 0;
        }

        private void AddComponents(bool reset = true)
        {
            Controls.Remove(label1);
            label1 = new Label();
            label1.ForeColor = Color.White;
            label1.BackColor = Color.FromArgb(109, 139, 70);
            label1.Width = 120;
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Location = new Point(chessButtons[0, 0].Location.X, chessButtons[0, 0].Location.Y - buttonSize);
            if (Board != null) UpdateLabelPoints();

            Controls.Add(label1);

            Controls.Remove(label2);
            label2 = new Label();
            label2.BackColor = Color.FromArgb(109, 139, 70);
            label2.ForeColor = Color.White;
            label2.Width = 120;
            label2.TextAlign = ContentAlignment.MiddleCenter;
            label2.Location = new Point(chessButtons[BoardSize - 1, 0].Location.X, chessButtons[BoardSize - 1, 0].Location.Y + (buttonSize + 50));
            if (Board != null) UpdateLabelPoints();

            Controls.Add(label2);

            Controls.Remove(difficultyBar);
            difficultyBar = new TrackBar();
            difficultyBar.Location = new Point(chessButtons[0, 0].Location.X - 550, chessButtons[0, 0].Location.Y);
            difficultyBar.Width = 400;
            difficultyBar.Minimum = 1;
            difficultyBar.Maximum = 100;
            difficultyBar.Value = 50;
            difficultyBar.BackColor = Color.FromArgb(70, 69, 62);
            difficultyBar.ValueChanged += DifficultyBarValueChanged;
            Controls.Add(difficultyBar);

            Controls.Remove(difficultyLabel);
            difficultyLabel = new Label();
            difficultyLabel.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            difficultyLabel.Width = difficultyBar.Width;
            difficultyLabel.Height = 40;
            difficultyLabel.Padding = new Padding(0, 10, 0, 0);
            difficultyLabel.BackColor = Color.FromArgb(70, 69, 62);
            difficultyLabel.ForeColor = Color.White;
            difficultyLabel.TextAlign = ContentAlignment.MiddleCenter;
            difficultyLabel.Location = new Point(difficultyBar.Location.X, difficultyBar.Location.Y - 40);
            difficultyLabel.Text = "Difficulty: " + (difficultyBar.Value * 10);

            Controls.Add(difficultyLabel);

            //Adding left border
            if (reset)
            {
                borderLeft = new Panel();
                borderLeft.Location = new Point(difficultyLabel.Location.X - 7, difficultyLabel.Location.Y);
                borderLeft.Height = difficultyLabel.Height + difficultyBar.Height;
                borderLeft.Width = 7;
                borderLeft.BackColor = Color.FromArgb(109, 139, 70);
                Controls.Add(borderLeft);
            }

            String temp = null;
            if(panelPositionInfo != null && !reset) temp = panelPositionInfo.Text;
           
            Controls.Remove(panel);

            panel = new Panel();
            panel.BackColor = Color.FromArgb(40, 155, 152, 137);
            panel.Width = 400;
            panel.Height = buttonSize * BoardSize;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Padding = new Padding(20, 20, 20, 20);
            panel.Location = new Point(chessButtons[0, 7].Location.X + 250, chessButtons[0, 7].Location.Y);

            Label panelPosition = new Label();
            panelPosition.Text = "Position";
            panelPosition.Width = panel.Width;
            panelPosition.Height = 50;
            panelPosition.TextAlign = ContentAlignment.MiddleCenter;
            panelPosition.BackColor = Color.FromArgb(70, 69, 62);
            panelPosition.ForeColor = Color.White;

            panel.Controls.Add(panelPosition);

            panelPositionInfo = new TextBox();
            panelPositionInfo.Location = new Point(panelPosition.Location.X, panelPosition.Location.Y + panelPosition.Height + 20); //20 for padding
            panelPositionInfo.Width = panel.Width;
            panelPositionInfo.Padding = new Padding(10, 10, 10, 10);
            panelPositionInfo.Font = new Font("Segoe UI", 8f, FontStyle.Bold);
            panelPositionInfo.Height = panel.Height / 2 + panel.Height / 5;
            panelPositionInfo.ReadOnly = true;
            panelPositionInfo.BackColor = Color.FromArgb(216, 212, 188);

            if(temp != null) panelPositionInfo.Text = temp;

            panelPositionInfo.BorderStyle = BorderStyle.None;
            panelPositionInfo.Multiline = true;
            panelPositionInfo.ScrollBars = ScrollBars.Vertical;

            panel.Controls.Add(panelPositionInfo);

            Panel gameControls = new Panel();
            gameControls.Location = new Point(panelPositionInfo.Location.X, panelPositionInfo.Location.Y + panelPositionInfo.Height + 20);
            gameControls.Width = panel.Width;
            gameControls.Height = 100;
            gameControls.BackColor = Color.FromArgb(216, 212, 188);

            Button newGameBtn = new Button();
            newGameBtn.Location = new Point(gameControls.Width / 4, gameControls.Height / 4);
            newGameBtn.Text = "New Game";
            newGameBtn.BackColor = Color.FromArgb(109, 139, 70);
            newGameBtn.ForeColor = Color.White;
            newGameBtn.Width = gameControls.Width / 2;
            newGameBtn.Height = gameControls.Height / 2;
            newGameBtn.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            newGameBtn.Padding = new Padding(10, 10, 10, 10);
            newGameBtn.FlatStyle = FlatStyle.Flat;
            newGameBtn.FlatAppearance.BorderSize = 0;
            newGameBtn.Cursor = Cursors.Hand;
            newGameBtn.Click += NewGameBtn_Click;

            gameControls.Controls.Add(newGameBtn);

            panel.Controls.Add(gameControls);

            if (reset)
            {
                ThemeBtn = new Panel();
                ThemeBtn.Location = new Point(difficultyBar.Location.X - 70, difficultyBar.Location.Y + 750);
                ThemeBtn.Width = 50;
                ThemeBtn.Height = 50;
                Image originalImage = Image.FromFile("Images/Theme.png");
                Bitmap resizedImage = new Bitmap(originalImage, 50, 50);
                ThemeBtn.BackgroundImage = resizedImage;
                ThemeBtn.Cursor = Cursors.Hand;
                ThemeBtn.Click += ThemeBtn_Click;
                ThemeBtn.Tag = null;

                Controls.Add(ThemeBtn);
            }

            Controls.Add(panel);

            //Fixing the resolution problem
            if (this.Size.Width < 1800)
            {
                difficultyBar.Visible = false;
                difficultyLabel.Visible = false;
                borderLeft.Visible = false;
                panel.Visible = false;
            }

        }

        private void ThemeBtn_Click(object sender, EventArgs e)
        {
            LoadThemePicker();
            ChangeBoardTheme();
        }

        private void ChangeBoardTheme()
        {
            ResetBtnStyle();
            AddComponents(false);
            Print(Board);
        }

        private void LoadThemePicker()
        {
            using (ThemePicker themePicker = new ThemePicker(DarkSquares, WhiteSquares))
            {
                // Show the form as a modal dialog
                DialogResult result = themePicker.ShowDialog();

                // Check if the user clicked OK or if the form was closed
                if (result == DialogResult.OK || result == DialogResult.Cancel)
                {
                    DarkSquares = themePicker.DarkSquares;
                    WhiteSquares = themePicker.WhiteSquares;
                }
            }
        }

        private Color GetViewedColor(Color clr, int darkTone)
        {
            Color depestColor = clr;
            for (int i = 0; i <= darkTone; i++)
            {
                if (clr.R - i < 0 || clr.G - i < 0 || clr.B - i < 0) break;

                depestColor = Color.FromArgb(clr.R - i, clr.G - i, clr.B - i);
            }

            return depestColor;
        }

        private void DifficultyBarValueChanged(object sender, EventArgs e)
        {
            difficultyLabel.Text = $"Difficulty: {difficultyBar.Value * 10}";
        }

        private async void NewGameBtn_Click(object sender, EventArgs e)
        {
            if (!engineThinking)
            {
                ResetBtnStyle();
                await Play();
                UpdateLabelPoints();
            }
        }

        private void UpdateLabelPoints()
        {
            int BotPoints = 0;
            int PlayerPoints = 0;

            if (Evaluation() < 0) BotPoints = Math.Abs(Evaluation());
            else PlayerPoints = Evaluation();

            if(label1 != null) label1.Text = "Bot points: " + BotPoints;
            if(label2 != null) label2.Text = "Player points: " + PlayerPoints;
        }

        private void AddMoveToInfo(int row1, int col1, int row2, int col2)
        {
            if (PieceType == Boardlib.PieceType.BLACK)
                panelPositionInfo.Text += Board.ConvertIntRowColToStrBlack(row1, col1) + " " +  Board.ConvertIntRowColToStrBlack(row2, col2) + ", ";
            else
                panelPositionInfo.Text += Board.convertIntRowColToStrWhite(row1, col1) + " " + Board.convertIntRowColToStrWhite(row2, col2) + ", ";
        }
    }
}