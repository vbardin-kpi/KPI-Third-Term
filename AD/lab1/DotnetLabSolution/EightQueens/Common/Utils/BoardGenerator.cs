using System;
using System.Collections.Generic;

namespace EightQueens.Common.Utils
{
    public static class BoardGenerator
    {
        private static readonly Random Randomizer = new();

        public static int[,] GenerateBoard(int boardSize)
        {
            var queensPlaced = 0;
            var chessBoard = new int[boardSize,boardSize];

            while (queensPlaced < boardSize)
            {
                var freeCells = FindFreeCells(boardSize, chessBoard);

                var cell = freeCells[Randomizer.Next(0, freeCells.Count)];
                freeCells.Remove(cell);

                chessBoard[cell.Row, cell.Col] = 1;
                queensPlaced++;
            }

            return chessBoard;
        }

        private static List<(int Row, int Col)> FindFreeCells(int boardSize, int[,] board)
        {
            var list = new List<ValueTuple<int, int>>();

            for (var row = 0; row < boardSize; row++)
            {
                for (var col = 0; col < boardSize; col++)
                {
                    if (board[row, col] is 0)
                    {
                        list.Add(new ValueTuple<int, int>(row, col));
                    }
                }
            }

            return list;
        }
    }
}