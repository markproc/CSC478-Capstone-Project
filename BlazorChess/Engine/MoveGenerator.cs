using ChessGame.Models;

namespace ChessGame.Engine
{
    public static class MoveGenerator
    {
        public static List<ChessSquare> GetPossibleMoves(ChessBoard board, ChessSquare square)
        {
            if (square.Piece is null)
                return new();

            return square.Piece.Type switch
            {
                PieceType.King => GetKingMoves(board, square),
                PieceType.Queen => GetQueenMoves(board, square),
                PieceType.Rook => GetRookMoves(board, square),
                PieceType.Bishop => GetBishopMoves(board, square),
                PieceType.Knight => GetKnightMoves(board, square),
                PieceType.Pawn => GetPawnMoves(board, square),
                _ => new()
            };
        }

        private static IEnumerable<ChessSquare> GetLineOfSight(ChessBoard board, ChessSquare square,
            int x, int y, int range = 8)
        {
            // Start at 1 so we don't include the square the piece is currently on
            for (int i = 1; i <= range; i++)
            {
                var disRank = square.Rank + (i * y);
                var disFile = square.File + (i * x);

                // Out of bounds
                if (disRank is < 0 or >= 8 ||
                    disFile is < 0 or >= 8)
                    yield break;

                var destSquare = board.Squares[disRank, disFile];
                if (destSquare.Piece is not null)
                {
                    // This is one of your pieces
                    if (destSquare.Piece.Color == square.Piece!.Color)
                        yield break;

                    var s = new ChessSquare(destSquare);
                    // This is an enemy piece
                    if (destSquare.Piece.Type == PieceType.King)
                        s.Move = MoveType.Check;
                    else
                        s.Move = MoveType.Capture;

                    yield return s;
                    break;
                }

                yield return new(destSquare);
            }
        }

        private static List<ChessSquare> GetKingMoves(ChessBoard board, ChessSquare square)
        {
            var possibleMoves = new List<ChessSquare>();

            // Castling: King must not have moved and cannot currently be in check
            if (square.Piece is King king && !king.HasMoved && !king.InCheck)
            {
                var corner = board.Squares[square.Rank, 7].Piece;
                var castle = board.Squares[square.Rank, 6];
                if (corner != null
                    && corner.Type == PieceType.Rook
                    && !corner.HasMoved
                    && board.Squares[square.Rank, 5].Piece == null
                    && castle.Piece == null)
                {
                    castle.Move = MoveType.Castle;
                    possibleMoves.Add(new(castle));
                }

                corner = board.Squares[square.Rank, 0].Piece;
                castle = board.Squares[square.Rank, 2];
                if (corner != null
                    && corner.Type == PieceType.Rook
                    && !corner.HasMoved
                    && board.Squares[square.Rank, 1].Piece == null
                    && castle.Piece == null
                    && board.Squares[square.Rank, 3].Piece == null)
                {
                    castle.Move = MoveType.Castle;
                    possibleMoves.Add(new(castle));
                }
            }

            possibleMoves.AddRange(GetLineOfSight(board, square, -1, 0, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, -1, 1, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, 0, 1, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, 1, 1, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, 1, 0, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, 1, -1, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, 0, -1, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, -1, -1, 1));

            return possibleMoves;
        }

        private static List<ChessSquare> GetQueenMoves(ChessBoard board, ChessSquare square)
        {
            var possibleMoves = new List<ChessSquare>();

            possibleMoves.AddRange(GetLineOfSight(board, square, -1, 0));
            possibleMoves.AddRange(GetLineOfSight(board, square, -1, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, 0, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, 1, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, 1, 0));
            possibleMoves.AddRange(GetLineOfSight(board, square, 1, -1));
            possibleMoves.AddRange(GetLineOfSight(board, square, 0, -1));
            possibleMoves.AddRange(GetLineOfSight(board, square, -1, -1));

            return possibleMoves;
        }

        private static List<ChessSquare> GetRookMoves(ChessBoard board, ChessSquare square)
        {
            var possibleMoves = new List<ChessSquare>();

            possibleMoves.AddRange(GetLineOfSight(board, square, 1, 0));
            possibleMoves.AddRange(GetLineOfSight(board, square, -1, 0));
            possibleMoves.AddRange(GetLineOfSight(board, square, 0, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, 0, -1));

            return possibleMoves;
        }

        private static List<ChessSquare> GetBishopMoves(ChessBoard board, ChessSquare square)
        {
            var possibleMoves = new List<ChessSquare>();

            possibleMoves.AddRange(GetLineOfSight(board, square, 1, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, 1, -1));
            possibleMoves.AddRange(GetLineOfSight(board, square, -1, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, -1, -1));

            return possibleMoves;
        }

        private static List<ChessSquare> GetKnightMoves(ChessBoard board, ChessSquare square)
        {
            var possibleMoves = new List<ChessSquare>();

            possibleMoves.AddRange(GetLineOfSight(board, square, 2, 1, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, 2, -1, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, -2, 1, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, -2, -1, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, 1, 2, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, 1, -2, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, -1, 2, 1));
            possibleMoves.AddRange(GetLineOfSight(board, square, -1, -2, 1));

            return possibleMoves;
        }

        private static List<ChessSquare> GetPawnMoves(ChessBoard board, ChessSquare square)
        {
            var possibleMoves = new List<ChessSquare>();

            var rankDir = square.Piece!.Color == PlayerColor.White ? -1 : 1;

            // Can move forward two spaces if first move
            if (!square.Piece.HasMoved)
                possibleMoves.AddRange(GetLineOfSight(board, square, 0, rankDir, 2));
            else
                possibleMoves.AddRange(GetLineOfSight(board, square, 0, rankDir, 1));

            // Can't capture by moving forward
            var destPiece = possibleMoves.LastOrDefault()?.Piece;
            if (destPiece is not null && destPiece.Color != square.Piece.Color)
                possibleMoves.RemoveAt(possibleMoves.Count - 1);

            // Can only move diagonally to capture
            var leftCapture = GetLineOfSight(board, square, -1, rankDir, 1);
            destPiece = leftCapture.LastOrDefault()?.Piece;
            if (destPiece is not null && destPiece.Color != square.Piece.Color)
                possibleMoves.AddRange(leftCapture);

            var rightCapture = GetLineOfSight(board, square, 1, rankDir, 1);
            destPiece = rightCapture.LastOrDefault()?.Piece;
            if (destPiece is not null && destPiece.Color != square.Piece.Color)
                possibleMoves.AddRange(rightCapture);

            // TODO: En passant

            // Mark any promotion moves
            var promotionRank = square.Piece.Color == PlayerColor.White ? 0 : 7;
            foreach (var move in possibleMoves)
                if (move.Rank == promotionRank && move.Move != MoveType.Check)
                    move.Move = MoveType.Promotion;

            return possibleMoves;
        }
    }
}
