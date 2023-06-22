using System.Collections.Generic;

namespace ChessGame.Models
{
    public abstract class ChessPiece
    {
        public PlayerColor Color { init; get; }
        public PieceType Type { protected set; get; }
        public int Value { init; get; }
        public bool HasMoved { get; set; }
        public bool Captured { get; set; }
        private List<ChessSquare> PossibleMoves { get; set; } = new();
        public List<ChessSquare> GetPossibleMoves() => PossibleMoves;
        public void SetPossibleMoves(List<ChessSquare> moves) => PossibleMoves = moves;

        public void Promote(PieceType type) => Type = type;

        public override string ToString()
        {
            if (Color == PlayerColor.White)
            {
                return Type switch
                {
                    PieceType.King => "♔",
                    PieceType.Queen => "♕",
                    PieceType.Rook => "♖",
                    PieceType.Bishop => "♗",
                    PieceType.Knight => "♘",
                    PieceType.Pawn => "♙",
                    _ => " "
                };
            }
            else
            {
                return Type switch
                {
                    PieceType.King => "♚",
                    PieceType.Queen => "♛",
                    PieceType.Rook => "♜",
                    PieceType.Bishop => "♝",
                    PieceType.Knight => "♞",
                    PieceType.Pawn => "♟",
                    _ => " "
                };
            }
        }
    }

    public enum PieceType
    {
        King,
        Queen,
        Rook,
        Bishop,
        Knight,
        Pawn
    }
}