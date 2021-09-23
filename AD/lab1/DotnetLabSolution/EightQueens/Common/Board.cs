using System;
using System.Collections.Generic;
using System.Linq;

namespace EightQueens.Common
{
    public class Board : BoardLite
    {
        public Board(int[,] chessBoard, int boardSize) : base(boardSize, chessBoard)
        {
        }

        public Board(BoardLite chessBoard)
            : base(chessBoard.BoardSize, chessBoard.Positions)
        {
        }

        public void Move(QueenPositionUpdate positionUpdate)
        {
            if (positionUpdate is null || Positions.Contains(positionUpdate.NewPosition)) return;

            Positions.Remove(positionUpdate.CurrentPosition);
            Positions.Add(positionUpdate.NewPosition);
        }

        public void MoveBack(QueenPositionUpdate positionUpdate)
        {
            if (positionUpdate is null) return;

            Positions.Remove(positionUpdate.NewPosition);
            Positions.Add(positionUpdate.CurrentPosition);
        }

        public List<(Position Current, Position New)> GetSafeCellsExt()
        {
            var safeCells = new List<(Position Current, Position New)>();

            InvalidPositions.Clear();
            Validate();

            foreach (var invPos in InvalidPositions)
            {
                // Horizontal movements
                for (var col = 0; col < BoardSize; ++col)
                {
                    if (IsFilled(invPos.Row, col)) continue;
                    var cell = new Position(invPos.Row, col);
                    if (IsColSafe(cell))
                    {
                        safeCells.Add(new ValueTuple<Position, Position>(invPos, cell));
                    }
                }

                // Vertical movements
                for (var row = 0; row < BoardSize; ++row)
                {
                    if (IsFilled(row, invPos.Column)) continue;
                    var cell = new Position(row, invPos.Column);
                    if (IsRowSafe(cell))
                    {
                        safeCells.Add(new ValueTuple<Position, Position>(invPos, cell));
                    }
                }
                // TODO: Add diagonal moves
            }

            return safeCells;
        }

        public int GetQueensAmountOnRow(int rowNumber)
        {
            return Positions.Count(x => x.Row == rowNumber);
        }

        public int GetQueensAmountOnColumn(int colNumber)
        {
            return Positions.Count(x => x.Column == colNumber);
        }

        public int GetQueensAmountOnDiagonals(int row, int col)
        {
            // abs(chessboard[i][0] - chessboard[j][0]) == abs(chessboard[i][1] - chessboard[j][1]
            return Positions.Count(x => Math.Abs(x.Row - row) == Math.Abs(x.Column - col));
        }
    }
}