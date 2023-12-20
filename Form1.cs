using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        private Color DarkSquareViewed = Color.FromArgb(205, 206, 178);
        private Color WhiteSquareViewed = Color.FromArgb(89, 119, 56);
        private PieceType? PieceType = null;

        public Form1()
        {
            Random rn = new Random();

            Board board = new Board(Boardlib.PieceType.WHITE);
            if (rn.Next(2) == 0)
                board = new Board(Boardlib.PieceType.BLACK);

            PieceType = board.PlayerPieceType;

            InitializeComponent();
            InitializeChessboard();
            Print(board);
            Board = board;

            if (PieceType == Boardlib.PieceType.BLACK)
            {
                PlayEngine();
            }
        }

        private void InitializeChessboard()
        {
            chessButtons = new Button[BoardSize, BoardSize];
            int buttonSize = 50; // Adjust button size as needed

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
                        BackColor = (row + col) % 2 == 0 ? Color.FromArgb(235, 236, 208) : Color.FromArgb(119, 149, 86),
                        ForeColor = (row + col) % 2 == 0 ? Color.FromArgb(235, 236, 208) : Color.FromArgb(119, 149, 86),
                        FlatStyle = FlatStyle.Flat
                    };

                    button.Click += ChessButton_Click;

                    Controls.Add(button);
                    chessButtons[row, col] = button;
                }
            }

            // Calculate the total size of the chessboard and center it in the form
            int boardTotalSize = buttonSize * BoardSize;
            int formWidth = boardTotalSize + 20; // Add some padding
            int formHeight = boardTotalSize + 40; // Add some padding

            ClientSize = new Size(formWidth, formHeight);
            CenterToScreen();
        }

        private void ChessButton_Click(object sender, EventArgs e)
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
                    if (!moved) first = true;

                    else
                    {
                        PlayEngine();
                    }
                }

            }

            first = !first;
            Print(Board);
        }

        private void Print(Board board)
        {
            foreach (Control control in Controls)
            {
                if (control is Button button)
                {
                    button.BackgroundImage = null;
                    Tuple<int, int> position = (Tuple<int, int>)button.Tag;
                    Square piece = board.getSquares()[position.Item1, position.Item2];

                    InitalizeSquare(piece, button);
                }
            }
        }

        private void PlayEngine()
        {
            ChessEngine engine = new ChessEngine("Stockfish/Stockfish.exe");

            string bestMove = engine.GetBestMove(Board.GetFen(), 100);
            char[] letters = bestMove.ToCharArray();
            if (Board.PlayerPieceType == Boardlib.PieceType.WHITE)
                Board.MoveOpponent(Board.ConvertRowAndCol(letters[1], letters[0]).X, Board.ConvertRowAndCol(letters[1], letters[0]).Y, Board.ConvertRowAndCol(letters[3], letters[2]).X, Board.ConvertRowAndCol(letters[3], letters[2]).Y);
            else
                Board.MoveOpponent(Board.ConvertRowAndColBlack(letters[1], letters[0]).X, Board.ConvertRowAndColBlack(letters[1], letters[0]).Y, Board.ConvertRowAndColBlack(letters[3], letters[2]).X, Board.ConvertRowAndColBlack(letters[3], letters[2]).Y);
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

                            if (color != DarkSquareViewed && color != WhiteSquareViewed)
                            {
                                button.BackColor = Color.FromArgb(color.R - 30, color.G - 30, color.B - 30);
                            }
                            else
                            {
                                //Reset
                                button.BackColor = Color.FromArgb(color.R + 30, color.G + 30, color.B + 30);

                            }
                        }
                    }
                }
            }
        }

        private void ResetBtnStyle()
        {
            foreach (Control control in Controls)
            {
                if (control is Button button)
                {
                    Tuple<int, int> position = (Tuple<int, int>)button.Tag;
                    button.BackColor = (position.Item1 + position.Item2) % 2 == 0 ? Color.FromArgb(235, 236, 208) : Color.FromArgb(119, 149, 86);
                    button.ForeColor = (position.Item1 + position.Item2) % 2 == 0 ? Color.FromArgb(235, 236, 208) : Color.FromArgb(119, 149, 86);
                }
            }
        }
    }
}