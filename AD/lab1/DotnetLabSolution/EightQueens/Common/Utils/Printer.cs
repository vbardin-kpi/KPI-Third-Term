using System;
using System.Linq;
using System.Text;

using EightQueens.AStar;
using EightQueens.LDFS;

namespace EightQueens.Common.Utils
{
    internal static class Printer
    {
        public static void AddInputBoardInfoToStringBuilder(
            int boardNumber,
            int boardSize,
            int[,] chessBoard,
            StringBuilder sb)
        {
            sb.Append("Board Number: " + boardNumber).AppendLine();
            sb.Append("------------------------").AppendLine();
            PrintBoard(boardSize, chessBoard, sb);
            sb.Append("------------------------").AppendLine();
        }

        public static void AddAStarResultToStringBuilder(StringBuilder sb, AStarSolutionInfo aStarSolution)
        {
            sb.Append("### AStar ###").AppendLine();
            sb.Append("Start date & time: " + DateTime.Now.ToString("G")).AppendLine();
            sb.Append("------------------------").AppendLine();
            if (aStarSolution.SolvedBoard != null)
            {
                PrintBoard(aStarSolution.SolvedBoard, sb);
            }
            else
            {
                sb.Append("Time for solving the given board expired!").AppendLine();
            }
            sb.Append("------------------------").AppendLine();
            sb.Append("Complete date & time: " + DateTime.Now.ToString("G")).AppendLine();
            sb.Append("Open nodes: " + aStarSolution.OpenStates).AppendLine();
            sb.Append("Checked nodes: " + aStarSolution.ClosedStates).AppendLine();
            sb.Append("------------------------").AppendLine();
        }

        public static void AddLdfsResultToStringBuilder(StringBuilder sb, LdfsSolutionInfo ldfsSolution)
        {
            sb.Append("### LDFS ###").AppendLine();
            sb.Append("Start date & time: " + DateTime.Now.ToString("G")).AppendLine();
            sb.Append("------------------------").AppendLine();
            if (ldfsSolution.SolvedBoard != null)
            {
                PrintBoard(ldfsSolution.SolvedBoard, sb);
            }
            else
            {
                sb.Append("Time for solving the given board expired!").AppendLine();
            }
            sb.Append("------------------------").AppendLine();
            sb.Append("Complete date & time: " + DateTime.Now.ToString("G")).AppendLine();
            sb.Append("Depth: " + ldfsSolution.Depth).AppendLine();
            sb.Append("Backtracks: " + ldfsSolution.BackTracks).AppendLine();
            sb.Append("Total steps: " + ldfsSolution.TotalSteps).AppendLine();
            sb.Append("------------------------").AppendLine();
        }

        private static void PrintBoard(Board board, StringBuilder sb)
        {
            for (var row = 0; row < board.BoardSize; row++)
            {
                for (var column = 0; column < board.BoardSize; column++)
                {
                    sb.Append(board.Positions.Any(x => x.Row == row && x.Column == column) ? " x " : " . ");
                }

                sb.AppendLine();
            }
        }

        private static void PrintBoard(int boardSize, int[,] board, StringBuilder stringBuilder)
        {
            for (var row = 0; row < boardSize; row++)
            {
                for (var column = 0; column < boardSize; column++)
                {
                    if (board[row, column] is 0)
                        stringBuilder.Append(" . ");
                    else if (board[row, column] is 1)
                        stringBuilder.Append(" X ");
                }

                stringBuilder.AppendLine();
            }
        }
    }
}