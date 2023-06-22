using System.Collections.Generic;

namespace ChessGame.Models
{
    public class ChessPlayer
    {
        public PlayerColor Color { get; set; }

        public List<ChessPiece> CapturedPieces { init; get; } = new();

        public int Wins { get; set; }
        public int Losses { get; set; }

        public int TotalMaterial()
        {
            int material = 0;
            foreach (var piece in CapturedPieces)
                material += piece.Value;

            return material;
        }
    }

    public enum PlayerColor
    {
        White,
        Black,
        None
    }
}