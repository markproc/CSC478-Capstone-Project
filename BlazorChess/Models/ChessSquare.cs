namespace ChessGame.Models
{
    public class ChessSquare
    {
        public int Rank { get; set; }
        public int File { get; set; }

        public SquareColor Color { get; set; }

        public ChessPiece? Piece { get; set; }

        public bool IsSelected { get; set; }

        public bool IsLegal { get; set; }

        public MoveType Move { get; set; }

        public ChessSquare()
        {

        }

        public ChessSquare(ChessSquare s)
        {
            Rank = s.Rank;
            File = s.File;
            Color = s.Color;
            Piece = s.Piece;
            IsLegal = s.IsLegal;
            Move = s.Move;
        }

        public void MovePiece(ChessSquare square)
        {
            square.Piece = Piece;
            Piece = null;
        }

        public bool Equals(ChessSquare other)
        {
            return Rank == other.Rank && File == other.File;
        }

        public string ToRankAndFile()
        {
            return $"{ToAlpha(File)}{ToNumeric(Rank)}";
        }

        public override string ToString()
        {
            return Piece != null ? Piece.ToString() : "\u3000";
        }

        private static string ToAlpha(int rank) => rank switch
        {
            0 => "A",
            1 => "B",
            2 => "C",
            3 => "D",
            4 => "E",
            5 => "F",
            6 => "G",
            7 => "H",
            _ => "Z"
        };

        private static string ToNumeric(int file) => file switch
        {
            0 => "8",
            1 => "7",
            2 => "6",
            3 => "5",
            4 => "4",
            5 => "3",
            6 => "2",
            7 => "1",
            _ => "0"
        };
    }

    public enum SquareColor
    {
        White,
        Black
    }

    public enum MoveType
    {
        Normal,
        Capture,
        Check,
        Castle,
        Promotion,
        EnPassant
    }
}