using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Boardlib
{
    public class ValidMoves
    {
        public Board Board { get; private set; }

        //(Initial, row, col)
        public List<(Square, int, int)> validMoves { get; private set; } = new List<(Square, int, int)>();

        public ValidMoves(Board board)
        {
            this.Board = board;
            getValidMoves(this.Board.PlayerPieceType);
        }

        public bool CanCastleRight(bool kingMoved)
        {
            if (kingMoved) return false;

            bool castle = true;
            int initalKingRow = 7;
            int initalKingCol = 4;
            bool passSquare = true;
            if (Board.PlayerPieceType == PieceType.BLACK)
            {
                initalKingCol = 3;
                passSquare = false;
            }
            int initalRockRow = 7;
            int initalRockCol = 7;

            if (!Board.KingIsInCheck() && Board.getKingPosition().X == initalKingRow
            && Board.getKingPosition().Y == initalKingCol
            && Board.getSquares()[initalRockRow, initalRockCol].Type == SquareContent.ROCK
            && Board.getSquares()[initalRockRow, initalRockCol].PieceType == Board.PlayerPieceType
            && Board.getSquares()[initalRockRow, initalRockCol - 1].PieceType == PieceType.EMPTY
            && Board.getSquares()[initalRockCol, initalRockCol - 2].PieceType == PieceType.EMPTY
            && (Board.getSquares()[initalRockCol, initalRockCol - 3].PieceType == PieceType.EMPTY || passSquare))
            {
                //get the opponent valid moves
                Board opponent = Board.getOpponentBoard();
                ValidMoves opponentValidMoves = new ValidMoves(opponent);
                //Loop through those two squares

                for (int i = initalKingCol; i <= initalRockCol; i++)
                {
                    foreach (var moves in opponentValidMoves.validMoves)
                    {
                        //if that squares is ocyopyed
                        if (moves.Item2 == initalKingRow && moves.Item3 == i)
                        {
                            return false;
                        }
                    }
                }

            }
            else return false;

            return castle;
        }

        public bool CanCastleLeft(bool kingMoved)
        {
            if (kingMoved) return false;

            bool castle = true;
            int initalKingRow = 7;
            int initalKingCol = 4;
            bool passSquare = false;
            if (Board.PlayerPieceType == PieceType.BLACK)
            {
                initalKingCol = 3;
                passSquare = true;
            }
            int initalRockRow = 7;
            int initalRockCol = 0;

            if (!Board.KingIsInCheck() && Board.getKingPosition().X == initalKingRow
            && Board.getKingPosition().Y == initalKingCol
            && Board.getSquares()[initalRockRow, initalRockCol].Type == SquareContent.ROCK
            && Board.getSquares()[initalRockRow, initalRockCol].PieceType == Board.PlayerPieceType
            && Board.getSquares()[initalRockRow, initalRockCol + 1].PieceType == PieceType.EMPTY
            && Board.getSquares()[initalRockRow, initalRockCol + 2].PieceType == PieceType.EMPTY
            && (Board.getSquares()[initalRockRow, initalRockCol + 3].PieceType == PieceType.EMPTY || passSquare))
            {
                //get the opponent valid moves
                Board opponent = Board.getOpponentBoard();
                ValidMoves opponentValidMoves = new ValidMoves(opponent);
                //Loop through those two squares
                for (int i = 0; i < initalKingCol; i++)
                {
                    foreach (var moves in opponentValidMoves.validMoves)
                    {
                        //if that squares is ocyopyed
                        if (moves.Item2 == initalKingRow && moves.Item3 == i) return false;
                    }
                }
            }
            else return false;

            return castle;
        }

        private void getValidMoves(PieceType pieces)
        {
            if (pieces is PieceType.EMPTY) throw new InvalidDataException("Cannot check the valid moves of an empty piece");

            setValidMoves(pieces);
            ValidateValidMoves();
        }

        private void setValidMoves(PieceType type)
        {
            for (int i = 0; i < Board.ROW; i++)
            {
                for (int j = 0; j < Board.COL; j++)
                {
                    if (this.Board.getSquares()[i, j].PieceType == type && this.Board.getSquares()[i, j].PieceType != PieceType.EMPTY) setValidMovesPiece(i, j);
                }
            }
        }

        private void ValidateValidMoves()
        {
            List<(Square, int, int)> newValidMoves = new List<(Square, int, int)>();

            foreach (var moves in validMoves)
            {
                int currentRow = Board.findSquare(moves.Item1).x;
                int currentCol = Board.findSquare(moves.Item1).y;

                SquareContent prevType = Board.getSquares()[moves.Item2, moves.Item3].Type;
                PieceType prevPieceType = Board.getSquares()[moves.Item2, moves.Item3].PieceType;
                //Make that move in the valid moves
                Board.SwapSqueres(currentRow, currentCol, moves.Item2, moves.Item3);
                //Is it not in check?
                if (!Board.KingIsInCheck()) newValidMoves.Add(moves);
                //Move the piece back again
                Board.SwapSqueres(moves.Item2, moves.Item3, currentRow, currentCol);

                Board.getSquares()[moves.Item2, moves.Item3].ChangeType(prevType);
                Board.getSquares()[moves.Item2, moves.Item3].ChangePieceType(prevPieceType);
            }

            validMoves = newValidMoves;
        }

        private void setValidMovesPiece(int row, int col)
        {
            switch (this.Board.getSquares()[row, col].Type)
            {
                case SquareContent.PAWN: ValidMoves.PawnValidMoves(row, col, Board, validMoves, Board.getOpponentPieceType(), true); break;
                case SquareContent.KNIGHT: ValidMoves.KnightValidMoves(row, col, Board, validMoves, Board.getOpponentPieceType()); break;
                case SquareContent.BISHOP: ValidMoves.BishopValidMoves(row, col, Board, validMoves, Board.getOpponentPieceType()); break;
                case SquareContent.ROCK: ValidMoves.RockValidMoves(row, col, Board, validMoves, Board.getOpponentPieceType()); break;
                case SquareContent.KING: ValidMoves.KingValidMoves(row, col, Board, validMoves, Board.getOpponentPieceType()); break;
                case SquareContent.QUEEN: ValidMoves.QueenValidMoves(row, col, Board, validMoves, Board.getOpponentPieceType()); break;
            }
        }

        static internal void PawnValidMoves(int row, int col, Board Board, List<(Square, int, int)> validMoves, PieceType opponentPieceType, bool swap = false)
        {
            //This checks for white
            if (opponentPieceType == PieceType.BLACK || swap)
            {
                //if theres an row up the pawn and that square is empty then add that square
                if (row - 1 >= 0)
                {
                    if (Board.getSquares()[row - 1, col].Type == SquareContent.EMPTY)
                        validMoves.Add((Board.getSquares()[row, col], row - 1, col));
                }

                //if theres two rows up the pawn and that square is empty (and the square before it was empty) 
                //then add the square only if the row is the first in the row of pawns
                if (row == 6)
                {
                    if (Board.getSquares()[row - 2, col].Type == SquareContent.EMPTY && Board.getSquares()[row - 1, col].Type == SquareContent.EMPTY)
                        validMoves.Add((Board.getSquares()[row, col], row - 2, col));
                }

                //if the left diagonal has got an opponent piece then add that square as an valid move
                if (col - 1 < Board.COL && col - 1 >= 0 && row - 1 >= 0)
                {
                    if (Board.getSquares()[row - 1, col - 1].PieceType == opponentPieceType)
                        validMoves.Add((Board.getSquares()[row, col], row - 1, col - 1));
                }

                //if the right diagonal has got an opponent piece then add that square as an valid move
                if (col + 1 < Board.COL && row - 1 >= 0)
                {
                    if (Board.getSquares()[row - 1, col + 1].PieceType == opponentPieceType)
                        validMoves.Add((Board.getSquares()[row, col], row - 1, col + 1));
                }
            }
            else
            {
                //if theres an row down the pawn and that square is empty then add that square
                if (row + 1 < Board.ROW)
                {
                    if (Board.getSquares()[row + 1, col].Type == SquareContent.EMPTY)
                        validMoves.Add((Board.getSquares()[row, col], row + 1, col));
                }

                //if theres two rows down the pawn and that square is empty (and the square before it was empty) 
                //then add the square only if the row is the first in the row of pawns
                if (row == 1 && row + 2 < Board.ROW && row + 1 < Board.ROW)
                {
                    if (Board.getSquares()[row + 2, col].Type == SquareContent.EMPTY && Board.getSquares()[row + 1, col].Type == SquareContent.EMPTY)
                        validMoves.Add((Board.getSquares()[row, col], row + 2, col));
                }

                //if the left diagonal has got an opponent piece then add that square as an valid move
                if (row + 1 < Board.ROW && col - 1 < Board.COL && col - 1 >= 0)
                {
                    if (Board.getSquares()[row + 1, col - 1].PieceType == opponentPieceType)
                        validMoves.Add((Board.getSquares()[row, col], row + 1, col - 1));
                }

                //if the right diagonal has got an opponent piece then add that square as an valid move
                if (row + 1 < Board.ROW && col + 1 < Board.COL)
                {
                    if (Board.getSquares()[row + 1, col + 1].PieceType == opponentPieceType)
                        validMoves.Add((Board.getSquares()[row, col], row + 1, col + 1));
                }
            }
        }

        static internal void KnightValidMoves(int row, int col, Board Board, List<(Square, int, int)> validMoves, PieceType opponentPieceType)
        {


            int nextRow = 1;
            //Loop two times for right, left, top, bottom
            for (int nextCol = 2; nextCol >= 1; nextCol--, nextRow++)
            {
                //bottom right
                if (col + nextCol < Board.COL && row + nextRow < Board.ROW)
                {
                    if (Board.getSquares()[row + nextRow, col + nextCol].Type == SquareContent.EMPTY
                       || Board.getSquares()[row + nextRow, col + nextCol].PieceType == opponentPieceType)
                        validMoves.Add((Board.getSquares()[row, col], row + nextRow, col + nextCol));
                }

                //bottom left
                if (col - nextCol < Board.COL && row + nextRow < Board.ROW && col - nextCol >= 0)
                {
                    if (Board.getSquares()[row + nextRow, col - nextCol].Type == SquareContent.EMPTY
                       || Board.getSquares()[row + nextRow, col - nextCol].PieceType == opponentPieceType)
                        validMoves.Add((Board.getSquares()[row, col], row + nextRow, col - nextCol));
                }

                //top right
                if (col + nextCol < Board.COL && row - nextRow < Board.ROW && row - nextRow >= 0)
                {
                    if (Board.getSquares()[row - nextRow, col + nextCol].Type == SquareContent.EMPTY
                       || Board.getSquares()[row - nextRow, col + nextCol].PieceType == opponentPieceType)
                        validMoves.Add((Board.getSquares()[row, col], row - nextRow, col + nextCol));
                }

                //top left
                if (col - nextCol < Board.COL && row - nextRow < Board.ROW && row - nextRow >= 0 && col - nextCol >= 0)
                {
                    if (Board.getSquares()[row - nextRow, col - nextCol].Type == SquareContent.EMPTY
                       || Board.getSquares()[row - nextRow, col - nextCol].PieceType == opponentPieceType)
                        validMoves.Add((Board.getSquares()[row, col], row - nextRow, col - nextCol));
                }

            }
        }

        static internal void BishopValidMoves(int row, int col, Board Board, List<(Square, int, int)> validMoves, PieceType opponentPieceType)
        {

            bool rightSquaresFree = true;
            bool leftSquaresFree = true;

            //Loop through all the rows that we can go up (we caluclate the max by subtracting the maximum Rows with the current Row)
            for (int i = 1; i < (Board.ROW - row); i++)
            {

                //Right squares
                if (row + i < Board.ROW && col + i < Board.COL && rightSquaresFree)
                {
                    if (Board.getSquares()[row + i, col + i].Type == SquareContent.EMPTY || Board.getSquares()[row + i, col + i].PieceType == opponentPieceType)
                        validMoves.Add((Board.getSquares()[row, col], row + i, col + i));
                    if (Board.getSquares()[row + i, col + i].Type != SquareContent.EMPTY)
                        rightSquaresFree = false;
                }

                //Left squares
                if (row + i < Board.ROW && col - i >= 0 && leftSquaresFree)
                {
                    if (Board.getSquares()[row + i, col - i].Type == SquareContent.EMPTY || Board.getSquares()[row + i, col - i].PieceType == opponentPieceType)
                        validMoves.Add((Board.getSquares()[row, col], row + i, col - i));
                    if (Board.getSquares()[row + i, col - i].Type != SquareContent.EMPTY)
                        leftSquaresFree = false;
                }

            }

            rightSquaresFree = true;
            leftSquaresFree = true;

            //Loop Up
            for (int i = 1; i <= row; i++)
            {
                //Right squares
                if (row - i >= 0 && row - i < Board.ROW && col + i < Board.COL && rightSquaresFree)
                {
                    if (Board.getSquares()[row - i, col + i].Type == SquareContent.EMPTY || Board.getSquares()[row - i, col + i].PieceType == opponentPieceType)
                        validMoves.Add((Board.getSquares()[row, col], row - i, col + i));
                    if (Board.getSquares()[row - i, col + i].Type != SquareContent.EMPTY)
                        rightSquaresFree = false;
                }

                //Left squares
                if (row - i >= 0 && row - i < Board.ROW && col - i >= 0 && leftSquaresFree)
                {
                    if (Board.getSquares()[row - i, col - i].Type == SquareContent.EMPTY || Board.getSquares()[row - i, col - i].PieceType == opponentPieceType)
                        validMoves.Add((Board.getSquares()[row, col], row - i, col - i));
                    if (Board.getSquares()[row - i, col - i].Type != SquareContent.EMPTY)
                        leftSquaresFree = false;
                }
            }
        }

        static internal void RockValidMoves(int row, int col, Board Board, List<(Square, int, int)> validMoves, PieceType opponentPieceType)
        {
            //Going through each row up
            for (int i = 1; i <= row; i++)
            {
                if (row - i < Board.ROW)
                {
                    if (Board.getSquares()[row - i, col].PieceType == opponentPieceType || Board.getSquares()[row - i, col].Type == SquareContent.EMPTY) validMoves.Add((Board.getSquares()[row, col], row - i, col));
                    if (Board.getSquares()[row - i, col].Type != SquareContent.EMPTY) break;
                }
            }

            //Going through each row down
            for (int i = 1; i < (Board.ROW - row); i++)
            {
                if (row + i < Board.ROW)
                {
                    if (Board.getSquares()[row + i, col].PieceType == opponentPieceType || Board.getSquares()[row + i, col].Type == SquareContent.EMPTY) validMoves.Add((Board.getSquares()[row, col], row + i, col));
                    if (Board.getSquares()[row + i, col].Type != SquareContent.EMPTY) break;
                }
            }

            //Going through each coloumn in the left
            for (int i = 1; col - i >= 0; i++)
            {
                if (Board.getSquares()[row, col - i].PieceType == opponentPieceType || Board.getSquares()[row, col - i].Type == SquareContent.EMPTY) validMoves.Add((Board.getSquares()[row, col], row, col - i));
                if (Board.getSquares()[row, col - i].Type != SquareContent.EMPTY) break;
            }

            //Going through each coloumn in the right
            for (int i = col + 1; i < Board.COL; i++)
            {
                if (Board.getSquares()[row, i].PieceType == opponentPieceType || Board.getSquares()[row, i].Type == SquareContent.EMPTY) validMoves.Add((Board.getSquares()[row, col], row, i));
                if (Board.getSquares()[row, i].Type != SquareContent.EMPTY) break;
            }
        }

        static internal void QueenValidMoves(int row, int col, Board board, List<(Square, int, int)> validMoves, PieceType opponentPieceType)
        {
            ValidMoves.BishopValidMoves(row, col, board, validMoves, opponentPieceType);
            ValidMoves.RockValidMoves(row, col, board, validMoves, opponentPieceType);
        }

        static internal void KingValidMoves(int row, int col, Board Board, List<(Square, int, int)> validMoves, PieceType opponentPieceType)
        {

            //Up
            for (int i = -1; i <= 1; i++)
            {
                if (col + i < Board.COL && col + i >= 0 && row - 1 >= 0)
                {
                    if (Board.getSquares()[row - 1, col + i].Type == SquareContent.EMPTY || Board.getSquares()[row - 1, col + i].PieceType == opponentPieceType)
                        validMoves.Add((Board.getSquares()[row, col], row - 1, col + i));
                }
            }

            //Down
            for (int i = -1; i <= 1; i++)
            {
                if (col - i < Board.COL && col + i >= 0 && col + i < Board.COL && row + 1 < Board.COL)
                {
                    if (Board.getSquares()[row + 1, col + i].Type == SquareContent.EMPTY || Board.getSquares()[row + 1, col + i].PieceType == opponentPieceType)
                        validMoves.Add((Board.getSquares()[row, col], row + 1, col + i));
                }
            }

            //Right
            if (col + 1 < Board.COL)
            {
                if (Board.getSquares()[row, col + 1].Type == SquareContent.EMPTY || Board.getSquares()[row, col + 1].PieceType == opponentPieceType)
                    validMoves.Add((Board.getSquares()[row, col], row, col + 1));
            }

            //Left
            if (col - 1 >= 0)
            {
                if (Board.getSquares()[row, col - 1].Type == SquareContent.EMPTY || Board.getSquares()[row, col - 1].PieceType == opponentPieceType)
                    validMoves.Add((Board.getSquares()[row, col], row, col - 1));
            }
        }

    }
}
