using System.Collections.Generic;
using System.Linq;

namespace EightQueens.Common
{
    public class BoardLite
    {
        public int BoardSize { get; }
        public List<Position> Positions { get; }
        public List<Position> InvalidPositions { get; }

        protected BoardLite(int boardSize, List<Position> positions)
        {
            BoardSize = boardSize;
            Positions = new List<Position>(positions);
            InvalidPositions = new List<Position>();
        }

        protected BoardLite(int boardSize, int[,] chessBoard)
        {
            BoardSize = boardSize;
            InvalidPositions = new List<Position>();

            BoardSize = boardSize;
            Positions = new List<Position>(boardSize);
            for (var row = 0; row < BoardSize; row++)
            {
                for (var column = 0; column < BoardSize; column++)
                {
                    if (chessBoard.GetValue(row, column) is 1)
                    {
                        Positions.Add(new Position(row, column));
                    }
                }
            }
        }

        public bool ValidateLite()
        {
            return Positions.Count == BoardSize && Positions.All(IsSafe);
        }

        public bool Validate()
        {
            if (Positions.Count != BoardSize)
                return false;

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var position in Positions)
            {
                if (!IsSafe(position) && !InvalidPositions.Contains(position))
                {
                    InvalidPositions.Add(position);
                }
            }

            return InvalidPositions.Count is 0;
        }

        protected bool IsRowSafe(Position pos)
        {
            for (var col = 0; col < BoardSize; col++)
            {
                if (col == pos.Column) continue;
                if (IsFilled(pos.Row, col)) return false;
            }

            return true;
        }

        protected bool IsColSafe(Position pos)
        {
            for (var row = 0; row < BoardSize; row++)
            {
                if (row == pos.Row) continue;
                if (IsFilled(row, pos.Column)) return false;
            }

            return true;
        }

        protected bool IsFilled(int row, int column)
        {
            return Positions.Any(pos => pos.Row == row && pos.Column == column);
        }

        private bool IsSafe(Position pos)
        {
            return IsRowSafe(pos) && IsColSafe(pos) && IsDiagonalsSafe(pos);
        }

        private bool IsDiagonalsSafe(Position pos)
        {
            // Left-diagonal
            // Upper-diagonal
            for (int row = pos.Row - 1, col = pos.Column - 1;
                row < BoardSize && row >= 0 && col < BoardSize && col >= 0;
                row--, col--)
            {
                if (IsFilled(row, col)) return false;
            }

            // Bottom-diagonal
            for (int row = pos.Row + 1, col = pos.Column + 1;
                row < BoardSize && row >= 0 && col < BoardSize && col >= 0;
                row++, col++)
            {
                if (IsFilled(row, col)) return false;
            }

            // Right-diagonal
            // Upper-diagonal
            for (int row = pos.Row - 1, col = pos.Column + 1;
                row < BoardSize && row >= 0 && col < BoardSize && col >= 0;
                row--, col++)
            {
                if (IsFilled(row, col)) return false;
            }

            // Bottom-diagonal
            for (int row = pos.Row + 1, col = pos.Column - 1;
                row < BoardSize && row >= 0 && col < BoardSize && col >= 0;
                row++, col--)
            {
                if (IsFilled(row, col)) return false;
            }

            return true;
        }
    }
}