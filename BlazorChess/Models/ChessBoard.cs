using System.Diagnostics;

namespace ChessGame.Models
{
    public class ChessBoard
    {
        public ChessSquare[,] Squares = new ChessSquare[8, 8];

        public ChessBoard()
        {
            ResetBoard();
        }

        public void ResetBoard()
        {
            var squareColor = SquareColor.White;

            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    Squares[rank, file] = new ChessSquare()
                    {
                        Rank = rank,
                        File = file,
                        Color = squareColor
                    };

                    if (rank == 0)
                    {
                        if (file == 0 || file == 7)
                            Squares[rank, file].Piece = new Rook() { Color = PlayerColor.Black };
                        else if (file == 1 || file == 6)
                            Squares[rank, file].Piece = new Knight() { Color = PlayerColor.Black };
                        else if (file == 2 || file == 5)
                            Squares[rank, file].Piece = new Bishop() { Color = PlayerColor.Black };
                        else if (file == 3)
                            Squares[rank, file].Piece = new Queen() { Color = PlayerColor.Black };
                        else if (file == 4)
                            Squares[rank, file].Piece = new King() { Color = PlayerColor.Black };
                    }
                    else if (rank == 1)
                        Squares[rank, file].Piece = new Pawn() { Color = PlayerColor.Black };

                    else if (rank == 6)
                        Squares[rank, file].Piece = new Pawn();
                    else if (rank == 7)
                    {
                        if (file == 0 || file == 7)
                            Squares[rank, file].Piece = new Rook();
                        else if (file == 1 || file == 6)
                            Squares[rank, file].Piece = new Knight();
                        else if (file == 2 || file == 5)
                            Squares[rank, file].Piece = new Bishop();
                        else if (file == 3)
                            Squares[rank, file].Piece = new Queen();
                        else if (file == 4)
                            Squares[rank, file].Piece = new King();
                    }

                    squareColor = squareColor == SquareColor.White ? SquareColor.Black : SquareColor.White;
                }

                squareColor = squareColor == SquareColor.White ? SquareColor.Black : SquareColor.White;
            }
        }

        public string ToRankAndFileString()
        {
            var boardStr = string.Empty;

            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    boardStr += Squares[rank, file].ToRankAndFile();
                }

                boardStr += "\n";
            }

            return boardStr;
        }
        public string ToLegalMovesString()
        {
            var boardStr = string.Empty;

            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    boardStr += $"[{(Squares[rank, file].IsLegal ? "\u2612" : Squares[rank, file])}]";
                }

                boardStr += "\n";
            }

            return boardStr;
        }


        public override string ToString()
        {
            var boardStr = string.Empty;

            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    boardStr += $"[{Squares[rank, file]}]";
                }

                boardStr += "\n";
            }

            return boardStr;
        }

        public void PrintRankAndFile()
        {
            Debug.WriteLine(ToRankAndFileString());
        }

        public void PrintBoard()
        {
            Debug.WriteLine(ToString());
        }

        public void PrintLegalMoves()
        {
            Debug.WriteLine(ToLegalMovesString());
        }
    }
}