using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Boardlib
{
    public class Board
    {

        public const int ROW = 8;
        public const int COL = 8;

        private Square[,] squares = new Square[ROW, COL];
        public bool KingMoved { get; private set; } = false;
        public PieceType PlayerPieceType { get; private set; }
        public ValidMoves BoardValidMoves { get; private set; }

        public bool WhitesTurn { get; set; } = true;

        public Board(PieceType player, Square[,] squares = null)
        {
            PlayerPieceType = player;
            if (squares is null) InitalizeSquares(player);
            else this.squares = squares;
            BoardValidMoves = new ValidMoves(this);
        }

        public bool Won()
        {
            Board opponent = getOpponentBoard();
            opponent.BoardValidMoves = new ValidMoves(opponent);

            if (opponent.CheckMate()) return true;

            return false;
        }

        public bool Lost() => CheckMate();

        //Checks for insuffient material based on the chess rules and checks for stalemate
        public bool Draw()
        {
            bool whiteInsuffient = true;
            bool blackInsuffient = true;

            int whiteKnights = 0;
            int blackKnights = 0;
            int whiteBishop = 0;
            int blackBishop = 0;

            List<Square> WhitePiece = new List<Square>();
            List<Square> BlackPiece = new List<Square>();

            for (int i = 0; i < Board.ROW; i++)
            {
                for (int j = 0; j < Board.COL; j++)
                {
                    if (squares[i, j].PieceType == PieceType.WHITE) WhitePiece.Add(squares[i, j]);
                    else if (squares[i, j].PieceType == PieceType.BLACK) BlackPiece.Add(squares[i, j]);
                }
            }

            foreach (var peices in WhitePiece)
            {
                if (peices.Type == SquareContent.BISHOP) whiteBishop++;
                else if (peices.Type == SquareContent.KNIGHT) whiteKnights++;
                else if (peices.Type != SquareContent.KING) whiteInsuffient = false;
            }

            foreach (var peices in BlackPiece)
            {
                if (peices.Type == SquareContent.BISHOP) blackBishop++;
                else if (peices.Type == SquareContent.KNIGHT) blackKnights++;
                else if (peices.Type != SquareContent.KING) blackInsuffient = false;
            }

            if (blackBishop >= 2 || blackKnights >= 2) blackInsuffient = false;
            if (whiteBishop >= 2 || whiteKnights >= 2) whiteInsuffient = false;

            if (blackInsuffient && whiteInsuffient) return true;

            Board opponent = getOpponentBoard();
            if (opponent.StaleMate() && this.StaleMate()) return true;

            return false;
        }

        public void MovePiece(int row1, int col1, int row2, int col2, ref bool moved)
        {
            if (row1 < 0 || row1 >= Board.ROW || col1 < 0 || col1 >= Board.COL || row2 < 0 || row2 >= Board.COL || col2 < 0 || col2 >= Board.COL)
                throw new IndexOutOfRangeException("row1 or col1 or row2 or col2 were out of range.");

            int initalKingRow = 7;
            int initalKingCol = 4;
            if (PlayerPieceType == PieceType.BLACK) initalKingCol = 3;

            //If we are trying to castle in the left
            if (!KingIsInCheck()
            && row1 == initalKingRow && col1 == initalKingCol
            && row2 == initalKingRow && col2 == 0
            && BoardValidMoves.CanCastleLeft(KingMoved))
            {
                if (PlayerPieceType == PieceType.BLACK)
                {
                    SwapSqueres(row1, col1, row1, 1);
                    SwapSqueres(row2, col2, row2, 2);
                    moved = true;
                }
                else
                {
                    SwapSqueres(row1, col1, row1, 2);
                    SwapSqueres(row2, col2, row2, 3);
                    moved = true;
                }
            }
            else if (!KingIsInCheck()
            && row1 == initalKingRow && col1 == initalKingCol
            && row2 == initalKingRow && col2 == Board.COL - 1
            && BoardValidMoves.CanCastleRight(KingMoved))
            {
                if (PlayerPieceType == PieceType.WHITE)
                {
                    SwapSqueres(row1, col1, row1, 6);
                    SwapSqueres(row2, col2, row2, 5);
                    moved = true;
                }
                else
                {
                    SwapSqueres(row1, col1, row1, 5);
                    SwapSqueres(row2, col2, row2, 4);
                    moved = true;
                }
            }
            else
            {
                bool exists = false;
                foreach (var valid in BoardValidMoves.validMoves)
                {
                    if (squares[row1, col1] == valid.Item1 && valid.Item2 == row2 && valid.Item3 == col2)
                    {
                        exists = true;
                        break;
                    }
                }

                if (exists)
                {
                    if (getKingPosition().X == row1 && getKingPosition().Y == col1) KingMoved = true;
                    SwapSqueres(row1, col1, row2, col2);
                    moved = true;
                }

                PromotePawns();
                BoardValidMoves = new ValidMoves(this);
            }

            if (moved)
                WhitesTurn = !WhitesTurn;
        }

        public bool canMoveTo(int row1, int col1, int row2, int col2)
        {
            if (row1 < 0 || row1 >= Board.ROW || col1 < 0 || col1 >= Board.COL || row2 < 0 || row2 >= Board.COL || col2 < 0 || col2 >= Board.COL)
                throw new IndexOutOfRangeException("row1 or col1 or row2 or col2 were out of range.");

            int initalKingRow = 7;
            int initalKingCol = 4;
            if (PlayerPieceType == PieceType.BLACK) initalKingCol = 3;

            //trying to castle to the left
            if (!KingIsInCheck()
            && row1 == initalKingRow && col1 == initalKingCol
            && row2 == initalKingRow && col2 == 0
            && BoardValidMoves.CanCastleLeft(KingMoved)) return true;

            //trying to castle to the right
            else if (!KingIsInCheck()
            && row1 == initalKingRow && col1 == initalKingCol
            && row2 == initalKingRow && col2 == Board.COL - 1
            && BoardValidMoves.CanCastleRight(KingMoved)) return true;

            foreach (var valid in BoardValidMoves.validMoves)
                if (squares[row1, col1] == valid.Item1 && valid.Item2 == row2 && valid.Item3 == col2) return true;

            return false;
        }

        internal void MoveOpponent(int row1, int col1, int row2, int col2)
        {
            WhitesTurn = !WhitesTurn;

            if (row1 == row2 && col1 == col2) return;

            int initalKingCol = 4;
            int movingRightRock = 5;
            int movingLeftRock = 3;

            if (PlayerPieceType == PieceType.BLACK)
            {
                initalKingCol = 3;
                movingRightRock = 4;
                movingLeftRock = 2;
            }

            if (col1 == initalKingCol && squares[row1, col1].Type == SquareContent.KING && (row1 == Board.ROW - 1 || row1 == 0) && col2 > initalKingCol + 1)
            {
                var copy = squares[row1, 7];
                squares[row1, 7] = new Square(SquareContent.EMPTY, PieceType.EMPTY);
                squares[row1, movingRightRock] = copy;

                SwapSqueres(row1, initalKingCol, row1, initalKingCol + 2);
            }

            else if (col1 == initalKingCol && squares[row1, col1].Type == SquareContent.KING && (row1 == Board.ROW - 1 || row1 == 0) && col2 < initalKingCol - 1)
            {
                var copy = squares[row1, 0];
                squares[row1, 0] = new Square(SquareContent.EMPTY, PieceType.EMPTY);
                squares[row1, movingLeftRock] = copy;

                SwapSqueres(row1, initalKingCol, row1, initalKingCol - 2);
            }
            else
            {
                SwapSqueres(row1, col1, row2, col2);
            }


            BoardValidMoves = new ValidMoves(this);
            PromotePawns();
        }

        internal void SwapSqueres(int row1, int col1, int row2, int col2, bool checkEmptySquare = true)
        {
            Square copy = squares[row1, col1];
            squares[row1, col1] = squares[row2, col2];
            squares[row2, col2] = copy;

            if (checkEmptySquare && squares[row1, col1].Type != SquareContent.EMPTY)
            {
                squares[row1, col1].ChangeType(SquareContent.EMPTY);
                squares[row1, col1].ChangePieceType(PieceType.EMPTY);
            }
        }

        private void InitalizePieces(int x, int y, PieceType player)
        {
            PieceType opponentPieces = getOpponentPieceType();

            Square square = null;

            if (PlayerPieceType == PieceType.WHITE)
            {
                switch (x)
                {
                    //Opponent
                    case 0 when y == 0: square = new Square(SquareContent.ROCK, opponentPieces); break;
                    case 0 when y == 1: square = new Square(SquareContent.KNIGHT, opponentPieces); break;
                    case 0 when y == 2: square = new Square(SquareContent.BISHOP, opponentPieces); break;
                    case 0 when y == 4: square = new Square(SquareContent.KING, opponentPieces); break;
                    case 0 when y == 3: square = new Square(SquareContent.QUEEN, opponentPieces); break;
                    case 0 when y == 5: square = new Square(SquareContent.BISHOP, opponentPieces); break;
                    case 0 when y == 6: square = new Square(SquareContent.KNIGHT, opponentPieces); break;
                    case 0 when y == 7: square = new Square(SquareContent.ROCK, opponentPieces); break;
                    case 1: square = new Square(SquareContent.PAWN, opponentPieces); break;

                    //Player
                    case 6: square = new Square(SquareContent.PAWN, player); break;
                    case 7 when y == 0: square = new Square(SquareContent.ROCK, player); break;
                    case 7 when y == 1: square = new Square(SquareContent.KNIGHT, player); break;
                    case 7 when y == 2: square = new Square(SquareContent.BISHOP, player); break;
                    case 7 when y == 4: square = new Square(SquareContent.KING, player); break;
                    case 7 when y == 3: square = new Square(SquareContent.QUEEN, player); break;
                    case 7 when y == 5: square = new Square(SquareContent.BISHOP, player); break;
                    case 7 when y == 6: square = new Square(SquareContent.KNIGHT, player); break;
                    case 7 when y == 7: square = new Square(SquareContent.ROCK, player); break;
                }
            }
            else
            {
                switch (x)
                {
                    //Opponent
                    case 0 when y == 0: square = new Square(SquareContent.ROCK, opponentPieces); break;
                    case 0 when y == 1: square = new Square(SquareContent.KNIGHT, opponentPieces); break;
                    case 0 when y == 2: square = new Square(SquareContent.BISHOP, opponentPieces); break;
                    case 0 when y == 3: square = new Square(SquareContent.KING, opponentPieces); break;
                    case 0 when y == 4: square = new Square(SquareContent.QUEEN, opponentPieces); break;
                    case 0 when y == 5: square = new Square(SquareContent.BISHOP, opponentPieces); break;
                    case 0 when y == 6: square = new Square(SquareContent.KNIGHT, opponentPieces); break;
                    case 0 when y == 7: square = new Square(SquareContent.ROCK, opponentPieces); break;
                    case 1: square = new Square(SquareContent.PAWN, opponentPieces); break;

                    //Player
                    case 6: square = new Square(SquareContent.PAWN, player); break;
                    case 7 when y == 0: square = new Square(SquareContent.ROCK, player); break;
                    case 7 when y == 1: square = new Square(SquareContent.KNIGHT, player); break;
                    case 7 when y == 2: square = new Square(SquareContent.BISHOP, player); break;
                    case 7 when y == 3: square = new Square(SquareContent.KING, player); break;
                    case 7 when y == 4: square = new Square(SquareContent.QUEEN, player); break;
                    case 7 when y == 5: square = new Square(SquareContent.BISHOP, player); break;
                    case 7 when y == 6: square = new Square(SquareContent.KNIGHT, player); break;
                    case 7 when y == 7: square = new Square(SquareContent.ROCK, player); break;
                }
            }

            if (square != null) squares[x, y] = square;
            else squares[x, y] = new Square(SquareContent.EMPTY, PieceType.EMPTY);
        }

        private void InitalizeSquares(PieceType player)
        {

            if (player is PieceType.EMPTY) throw new InvalidOperationException("Cannot have an empty player!");

            PlayerPieceType = player;

            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    InitalizePieces(i, j, player);
                }
            }

        }

        public Square[,] getSquares() => squares;

        public PieceType getOpponentPieceType()
        {
            if (PlayerPieceType == PieceType.WHITE) return PieceType.BLACK;
            else return PieceType.WHITE;
        }

        public bool KingIsInCheck()
        {
            var opponentValidMoves = this.getOpponentValidMoves();

            foreach (var moves in opponentValidMoves)
            {
                if (moves.Item2 == getKingPosition().X && moves.Item3 == getKingPosition().Y) return true;
            }

            return false;
        }

        public Point getKingPosition()
        {
            Point pos = new Point();
            for (int i = 0; i < Board.ROW; i++)
            {
                for (int j = 0; j < Board.COL; j++)
                {
                    if (squares[i, j].Type == SquareContent.KING && squares[i, j].PieceType == PlayerPieceType)
                    {
                        pos.X = i;
                        pos.Y = j;
                    }
                }
            }

            return pos;
        }

        private bool CheckMate()
        {
            bool checkMate = true;
            if (KingIsInCheck())
            {
                foreach (var moves in BoardValidMoves.validMoves)
                {
                    int currentRow = findSquare(moves.Item1).x;
                    int currentCol = findSquare(moves.Item1).y;

                    SquareContent prevType = squares[moves.Item2, moves.Item3].Type;
                    PieceType prevPieceType = squares[moves.Item2, moves.Item3].PieceType;
                    //Make that move in the valid moves
                    SwapSqueres(currentRow, currentCol, moves.Item2, moves.Item3);
                    //Is it not in check?
                    if (!KingIsInCheck()) checkMate = false;
                    //Move the piece back again
                    SwapSqueres(moves.Item2, moves.Item3, currentRow, currentCol);

                    squares[moves.Item2, moves.Item3].ChangeType(prevType);
                    squares[moves.Item2, moves.Item3].ChangePieceType(prevPieceType);
                }
            }
            else return false;

            return checkMate;
        }

        private bool StaleMate()
        {
            Board opponent = getOpponentBoard();
            if (KingIsInCheck() || opponent.KingIsInCheck()) return false;

            bool stale = true;
            List<(Square, int, int)> kingValidMoves = new List<(Square, int, int)>();
            ValidMoves validMoves = new ValidMoves(this);

            foreach (var moves in validMoves.validMoves)
                if (moves.Item1.Type == SquareContent.KING && moves.Item1.PieceType == opponent.PlayerPieceType) kingValidMoves.Add(moves);

            //And if the king has not valid moves
            if (kingValidMoves.Count == 0)
            {
                foreach (var moves in BoardValidMoves.validMoves)
                {
                    int currentRow = findSquare(moves.Item1).x;
                    int currentCol = findSquare(moves.Item1).y;

                    SquareContent prevType = squares[moves.Item2, moves.Item3].Type;
                    PieceType prevPieceType = squares[moves.Item2, moves.Item3].PieceType;
                    //Make that move in the valid moves
                    SwapSqueres(currentRow, currentCol, moves.Item2, moves.Item3);

                    //Update
                    ValidMoves.KingValidMoves(getKingPosition().X, getKingPosition().Y, this, kingValidMoves, getOpponentPieceType());

                    //Is it not in check?
                    if (kingValidMoves.Count != 0) stale = false;
                    //Move the piece back again
                    SwapSqueres(moves.Item2, moves.Item3, currentRow, currentCol);

                    squares[moves.Item2, moves.Item3].ChangeType(prevType);
                    squares[moves.Item2, moves.Item3].ChangePieceType(prevPieceType);

                    //Reset back
                    ValidMoves.KingValidMoves(getKingPosition().X, getKingPosition().Y, this, kingValidMoves, getOpponentPieceType());
                }
            }
            else return false;


            return stale;
        }

        public Board getOpponentBoard()
        {
            return new Board(this.getOpponentPieceType(), this.squares);
        }

        internal (Square, int x, int y) findSquare(Square square)
        {
            for (int i = 0; i < Board.ROW; i++)
            {
                for (int j = 0; j < Board.COL; j++)
                {
                    if (square.Id == squares[i, j].Id) return (square, i, j);
                }
            }

            throw new IndexOutOfRangeException("Cannot find the square");
        }

        public List<(Square, int, int)> getOpponentValidMoves()
        {
            List<(Square, int, int)> validMoves = new List<(Square, int, int)>();
            for (int i = 0; i < Board.ROW; i++)
            {
                for (int j = 0; j < Board.COL; j++)
                {
                    if (squares[i, j].PieceType == this.getOpponentPieceType())
                    {
                        switch (squares[i, j].Type)
                        {
                            case SquareContent.PAWN when PlayerPieceType == PieceType.WHITE: ValidMoves.PawnValidMoves(i, j, this, validMoves, PlayerPieceType); break;
                            case SquareContent.PAWN when PlayerPieceType == PieceType.BLACK: ValidMoves.PawnValidMoves(i, j, this, validMoves, PlayerPieceType, GettingOpponentBlackValid: true); break;
                            case SquareContent.KNIGHT: ValidMoves.KnightValidMoves(i, j, this, validMoves, PlayerPieceType); break;
                            case SquareContent.BISHOP: ValidMoves.BishopValidMoves(i, j, this, validMoves, PlayerPieceType); break;
                            case SquareContent.ROCK: ValidMoves.RockValidMoves(i, j, this, validMoves, PlayerPieceType); break;
                            case SquareContent.KING: ValidMoves.KingValidMoves(i, j, this, validMoves, PlayerPieceType); break;
                            case SquareContent.QUEEN: ValidMoves.QueenValidMoves(i, j, this, validMoves, PlayerPieceType); break;
                        }
                    }
                }
            }

            return validMoves;
        }

        private void PromotePawns()
        {
            //go through each pawn
            for (int i = 0; i < Board.ROW; i++)
            {
                for (int j = 0; j < Board.COL; j++)
                {
                    if (squares[i, j].Type == SquareContent.PAWN && squares[i, j].PieceType == PlayerPieceType && i == 0) squares[i, j] = new Square(SquareContent.QUEEN, PlayerPieceType);
                    else if (squares[i, j].Type == SquareContent.PAWN && squares[i, j].PieceType == getOpponentPieceType() && i == (Board.ROW - 1)) squares[i, j] = new Square(SquareContent.QUEEN, getOpponentPieceType());
                }
            }
        }

        //Does not implement halfmove, fullmove, and enpasant check 
        public String GetFen()
        {
            String items = String.Empty;
            int counter = 0;
            if (PlayerPieceType == PieceType.WHITE)
            {
                for (int i = 0; i < Board.COL; i++)
                {
                    for (int j = 0; j < Board.ROW; j++)
                    {
                        char? pieceFen = GetPieceFen(squares[i, j]);
                        if (pieceFen is null) counter++;
                        else
                        {
                            if (counter > 0) items += counter.ToString();
                            counter = 0;
                            items += pieceFen.ToString();
                        }
                    }

                    if (counter > 0) items += counter.ToString();

                    if (i < Board.ROW - 1) items += "/";
                    counter = 0;
                }
            }
            else
            {
                for (int i = Board.COL - 1; i >= 0; i--)
                {
                    for (int j = Board.ROW - 1; j >= 0; j--)
                    {
                        char? pieceFen = GetPieceFen(squares[i, j]);
                        if (pieceFen is null) counter++;
                        else
                        {
                            if (counter > 0) items += counter.ToString();
                            counter = 0;
                            items += pieceFen.ToString();
                        }
                    }

                    if (counter > 0) items += counter.ToString();

                    items += "/";
                    counter = 0;
                }
            }

            if (WhitesTurn) items += " w ";
            else items += " b ";

            if (PlayerPieceType == PieceType.BLACK)
            {
                if (!getOpponentBoard().KingMoved && squares[0, Board.COL - 1].Type == SquareContent.ROCK && squares[0, Board.COL - 1].PieceType == PieceType.WHITE) items += "K";
                if (!getOpponentBoard().KingMoved && squares[0, 0].Type == SquareContent.ROCK && squares[0, 0].PieceType == PieceType.WHITE) items += "Q";


                if (!KingMoved && squares[Board.ROW - 1, Board.COL - 1].Type == SquareContent.ROCK && squares[Board.ROW - 1, Board.COL - 1].PieceType == PieceType.BLACK) items += "k";
                if (!KingMoved && squares[Board.ROW - 1, 0].Type == SquareContent.ROCK && squares[Board.ROW - 1, 0].PieceType == PieceType.BLACK) items += "q";
            }
            else
            {

                if (!KingMoved && squares[Board.ROW - 1, Board.COL - 1].Type == SquareContent.ROCK && squares[Board.ROW - 1, Board.COL - 1].PieceType == PieceType.WHITE) items += "K";
                if (!KingMoved && squares[Board.ROW - 1, 0].Type == SquareContent.ROCK && squares[Board.ROW - 1, 0].PieceType == PieceType.WHITE) items += "Q";


                if (!getOpponentBoard().KingMoved && squares[0, Board.COL - 1].Type == SquareContent.ROCK && squares[0, Board.COL - 1].PieceType == PieceType.BLACK) items += "k";
                if (!getOpponentBoard().KingMoved && squares[0, 0].Type == SquareContent.ROCK && squares[0, 0].PieceType == PieceType.BLACK) items += "q";
            }

            if (items[items.Length - 1] == ' ') items += "-";

            items += " - 0 1";

            return items;
        }

        public char? GetPieceFen(Square square)
        {
            switch (square.Type)
            {
                case SquareContent.EMPTY: return null;
                case SquareContent.KING when (square.PieceType == PieceType.WHITE): return 'K';
                case SquareContent.PAWN when (square.PieceType == PieceType.WHITE): return 'P';
                case SquareContent.BISHOP when (square.PieceType == PieceType.WHITE): return 'B';
                case SquareContent.KNIGHT when (square.PieceType == PieceType.WHITE): return 'N';
                case SquareContent.ROCK when (square.PieceType == PieceType.WHITE): return 'R';
                case SquareContent.QUEEN when (square.PieceType == PieceType.WHITE): return 'Q';
                case SquareContent.KING when (square.PieceType == PieceType.BLACK): return 'k';
                case SquareContent.PAWN when (square.PieceType == PieceType.BLACK): return 'p';
                case SquareContent.BISHOP when (square.PieceType == PieceType.BLACK): return 'b';
                case SquareContent.KNIGHT when (square.PieceType == PieceType.BLACK): return 'n';
                case SquareContent.ROCK when (square.PieceType == PieceType.BLACK): return 'r';
                case SquareContent.QUEEN when (square.PieceType == PieceType.BLACK): return 'q';
            };

            throw new InvalidDataException();
        }

        public static String convertIntRowColToStrWhite(int row, int col)
        {
            String rowAndCol = String.Empty;

            switch (col)
            {
                case 0: rowAndCol += "a"; break;
                case 1: rowAndCol += "b"; break;
                case 2: rowAndCol += "c"; break;
                case 3: rowAndCol += "d"; break;
                case 4: rowAndCol += "e"; break;
                case 5: rowAndCol += "f"; break;
                case 6: rowAndCol += "g"; break;
                case 7: rowAndCol += "h"; break;
                default:
                    throw new InvalidDataException();
            }

            switch (row)
            {
                case 0: rowAndCol += 8; break;
                case 1: rowAndCol += 7; break;
                case 2: rowAndCol += 6; break;
                case 3: rowAndCol += 5; break;
                case 4: rowAndCol += 4; break;
                case 5: rowAndCol += 3; break;
                case 6: rowAndCol += 2; break;
                case 7: rowAndCol += 1; break;
                default:
                    throw new InvalidDataException();
            }

            return rowAndCol;
        }

        public static String ConvertIntRowColToStrBlack(int row, int col)
        {
            String rowAndCol = String.Empty;

            switch (col)
            {
                case 0: rowAndCol += "h"; break;
                case 1: rowAndCol += "g"; break;
                case 2: rowAndCol += "f"; break;
                case 3: rowAndCol += "e"; break;
                case 4: rowAndCol += "d"; break;
                case 5: rowAndCol += "c"; break;
                case 6: rowAndCol += "b"; break;
                case 7: rowAndCol += "a"; break;
                default:
                    throw new InvalidDataException();
            }

            switch (row)
            {
                case 0: rowAndCol += 1; break;
                case 1: rowAndCol += 2; break;
                case 2: rowAndCol += 3; break;
                case 3: rowAndCol += 4; break;
                case 4: rowAndCol += 5; break;
                case 5: rowAndCol += 6; break;
                case 6: rowAndCol += 7; break;
                case 7: rowAndCol += 8; break;
                default:
                    throw new InvalidDataException();
            }

            return rowAndCol;
        }

        public static Point ConvertRowAndCol(char row, char col)
        {
            int newRow;
            int newCol;

            switch (col)
            {
                case 'a': newCol = 0; break;
                case 'b': newCol = 1; break;
                case 'c': newCol = 2; break;
                case 'd': newCol = 3; break;
                case 'e': newCol = 4; break;
                case 'f': newCol = 5; break;
                case 'g': newCol = 6; break;
                case 'h': newCol = 7; break;
                default:
                    throw new InvalidDataException();
            }

            switch (row)
            {
                case '8': newRow = 0; break;
                case '7': newRow = 1; break;
                case '6': newRow = 2; break;
                case '5': newRow = 3; break;
                case '4': newRow = 4; break;
                case '3': newRow = 5; break;
                case '2': newRow = 6; break;
                case '1': newRow = 7; break;
                default:
                    throw new InvalidDataException();
            }

            return new Point(newRow, newCol);
        }

        public static Point ConvertRowAndColBlack(char row, char col)
        {
            int newRow;
            int newCol;

            switch (col)
            {
                case 'h': newCol = 0; break;
                case 'g': newCol = 1; break;
                case 'f': newCol = 2; break;
                case 'e': newCol = 3; break;
                case 'd': newCol = 4; break;
                case 'c': newCol = 5; break;
                case 'b': newCol = 6; break;
                case 'a': newCol = 7; break;
                default:
                    throw new InvalidDataException();
            }

            switch (row)
            {
                case '8': newRow = 7; break;
                case '7': newRow = 6; break;
                case '6': newRow = 5; break;
                case '5': newRow = 4; break;
                case '4': newRow = 3; break;
                case '3': newRow = 2; break;
                case '2': newRow = 1; break;
                case '1': newRow = 0; break;
                default:
                    throw new InvalidDataException();
            }

            return new Point(newRow, newCol);
        }
    }
}