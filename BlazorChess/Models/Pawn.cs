namespace ChessGame.Models
{
    public class Pawn : ChessPiece
    {
        public Pawn()
        {
            Type = PieceType.Pawn;
            Value = 100;
        }
    }
}