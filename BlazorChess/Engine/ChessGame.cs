using ChessGame.Models;

namespace ChessGame.Engine
{
    public enum GameStatus
    {
        InProgress,
        Checkmate,
        Resign,
        Timeout,
        Stalemate,
    }

    public enum GameMode
    {
        Standard,
        Blitz
    }

    public class ChessGame
    {
        public ChessBoard Board { get; set; } = new ChessBoard();

        public ChessPlayer PlayerWhite { get; set; } = new ChessPlayer() { Color = PlayerColor.White };
        public ChessPlayer PlayerBlack { get; set; } = new ChessPlayer() { Color = PlayerColor.Black };

        public GameStatus Status { get; set; } = GameStatus.InProgress;

        public PlayerColor Turn { get; set; } = PlayerColor.White;

        public PlayerColor Subject { get; set; } = PlayerColor.None;

        public ChessSquare? SelectedSquare { get; set; }

        public ChessGame()
        {
            GenerateLegalMoves();
        }

        public void Reset()
        {
            PlayerWhite = new ChessPlayer() { Color = PlayerColor.White };
            PlayerBlack = new ChessPlayer() { Color = PlayerColor.Black };
            Status = GameStatus.InProgress;
            Turn = PlayerColor.White;
            Subject = PlayerColor.None;
            SelectedSquare = null;

            Board.ResetBoard();
            GenerateLegalMoves();
        }

        public void Resign()
        {
            Status = GameStatus.Resign;
            Subject = Turn;
        }

        public ChessPlayer GetCurrentPlayer() => Turn switch
        {
            PlayerColor.White => PlayerWhite,
            PlayerColor.Black => PlayerBlack,
            _ => PlayerWhite
        };

        private void GenerateLegalMoves()
        {
            // Clear squares
            foreach (var s in Board.Squares)
            {
                s.IsLegal = false;
                s.Move = MoveType.Normal;
            }

            foreach (var s in Board.Squares)
            {
                if (s.Piece == null) continue;

                var possibleMoves = MoveGenerator.GetPossibleMoves(Board, s);

                s.Piece.SetPossibleMoves(possibleMoves);
            }
        }

        public ChessPiece? SelectPiece(ChessSquare square)
        {
            foreach (var s in Board.Squares)
            {
                s.IsLegal = false;
                s.Move = MoveType.Normal;
            }

            if (square.Piece != null)
            {
                // Update board
                foreach (var move in square.Piece.GetPossibleMoves())
                {
                    Board.Squares[move.Rank, move.File].IsLegal = true;
                    Board.Squares[move.Rank, move.File].Move = move.Move;
                }
            }

            if (SelectedSquare != null)
                SelectedSquare.IsSelected = false;

            square.IsSelected = true;
            SelectedSquare = square;

            return square.Piece;
        }

        /// <summary>
        /// Moves a piece and ends the turn
        /// </summary>
        public void Move(ChessSquare destinationSquare)
        {
            if (SelectedSquare is null ||
                SelectedSquare.Piece is null ||
                SelectedSquare.Piece.Color != Turn)
                return;

            if (SelectedSquare.Piece.GetPossibleMoves().Any(s => s.Equals(destinationSquare)))
            {
                SelectedSquare.Piece.HasMoved = true;

                // Did we capture a piece?
                if (destinationSquare.Piece is not null)
                {
                    destinationSquare.Piece.Captured = true;
                    GetCurrentPlayer().CapturedPieces.Add(destinationSquare.Piece);
                }

                // Special case: Castling
                if (destinationSquare.Move == MoveType.Castle)
                    CastleMoveRook(destinationSquare);

                SelectedSquare.MovePiece(destinationSquare);
                SelectedSquare.IsSelected = false;
                SelectedSquare = null;

                // Special case: Pawn Promotion
                //if (destinationSquare.Move == MoveType.Promotion)
                // TODO: Set a specific status or callback?

                // Recalc all legal moves for all pieces
                GenerateLegalMoves();

                // Update turn
                Turn = Turn == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;

                // Checkmate
                var king = StageForCheckmate();
                if (king != null)
                {
                    Status = king.InCheck == true ? GameStatus.Checkmate : GameStatus.Stalemate;
                    Subject = Turn == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
                }
            }
        }

        private King? StageForCheckmate()
        {
            King? king = null;
            foreach (var s in Board.Squares)
                if (s.Piece is King k && k.Color == Turn)
                    king = k;

            king!.InCheck = false;

            var gameover = true;

            foreach (var s in Board.Squares)
            {
                if (s.Piece == null)
                    continue;

                if (s.Piece.Color != Turn)
                {
                    // See if king is already in check first
                    if (s.Piece.GetPossibleMoves().Any(m => m.Move == MoveType.Check))
                        king!.InCheck = true;
                    continue;
                }

                var moves = new List<ChessSquare>();

                foreach (var move in s.Piece.GetPossibleMoves())
                {
                    // Temporarily move piece
                    var retainPiece = move.Piece;
                    var boardMove = Board.Squares[move.Rank, move.File];
                    s.MovePiece(boardMove);

                    // Simulate moves for all player's pieces on board
                    var check = false;
                    foreach (var ps in Board.Squares)
                    {
                        if (ps.Piece == null || ps.Piece.Color == Turn) continue;

                        var possibleMoves = MoveGenerator.GetPossibleMoves(Board, ps);

                        if (possibleMoves.Any(m => m.Move == MoveType.Check))
                            check = true;
                    }

                    if (!check)
                        moves.Add(move);

                    // Put the piece back
                    s.Piece = boardMove.Piece;
                    boardMove.Piece = retainPiece;
                }

                // Update with legal moves
                s.Piece!.SetPossibleMoves(moves);

                if (moves.Any())
                    gameover = false;
            }

            return gameover ? king : null;
        }

        private void CastleMoveRook(ChessSquare destSquare)
        {
            if (destSquare.File == 6)
            {
                Board.Squares[SelectedSquare!.Rank, 7]
                    .MovePiece(Board.Squares[SelectedSquare.Rank, 5]);
            }
            else if (destSquare.File == 2)
            {
                Board.Squares[SelectedSquare!.Rank, 0]
                    .MovePiece(Board.Squares[SelectedSquare.Rank, 3]);
            }
        }
    }
}