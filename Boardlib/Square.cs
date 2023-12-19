namespace Boardlib
{
    //The possible piece types
    public enum PieceType
    {
        BLACK,
        WHITE,
        EMPTY,
    }

    public enum SquareContent
    {
        EMPTY,
        PAWN,
        BISHOP,
        KNIGHT,
        ROCK,
        KING,
        QUEEN,
    }
    public class Square
    {
        public SquareContent Type { get; private set; }
        public PieceType PieceType { get; private set; }

        public int Id { get; private set; }

        private static int GeneratedId = 0;

        public Square(SquareContent type, PieceType pieceType)
        {
            ChangeType(type);
            ChangePieceType(pieceType);
            Id = GeneratedId++;
        }

        public void EmptySquare() => ChangeType(SquareContent.EMPTY);

        public void ChangeType(SquareContent type) => this.Type = type;
        public void ChangePieceType(PieceType type) => this.PieceType = type;

    }
}