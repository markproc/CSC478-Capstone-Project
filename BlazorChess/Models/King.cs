namespace ChessGame.Models
{
    public class King : ChessPiece
    {
        public bool InCheck { get; set; }

        public King()
        {
            Type = PieceType.King;
        }
    }
}