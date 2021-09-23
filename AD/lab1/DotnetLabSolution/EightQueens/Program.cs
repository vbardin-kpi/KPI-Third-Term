using System;
using System.Linq;
using System.Text;

using EightQueens.AStar;
using EightQueens.Common;
using EightQueens.Common.Utils;
using EightQueens.LDFS;

namespace EightQueens
{
    internal static class Program
    {
        private const int BoardSize = 8;

        public static void Main()
        {
            try
            {
                var boards = Enumerable.Range(0, 20)
                    .Select(_ => BoardGenerator.GenerateBoard(BoardSize)).ToList();

                var boardNumber = 1;
                foreach (var board in boards)
                {
                    SolveBoardAndPrintInfo(boardNumber, board);
                    boardNumber++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void SolveBoardAndPrintInfo(int currentBoardNumber, int[,] chessBoard)
        {
            var (aStarSolution, ldfsSolution) = SolveBoard(chessBoard);
            PrintSolutions(currentBoardNumber, chessBoard, aStarSolution, ldfsSolution);
        }

        private static void PrintSolutions(
            int currentBoardNumber,
            int[,] chessBoard,
            AStarSolutionInfo aStarSolution,
            LdfsSolutionInfo ldfsSolution)
        {
            var sb = new StringBuilder();

            Printer.AddInputBoardInfoToStringBuilder(currentBoardNumber, BoardSize, chessBoard, sb);
            try
            {
                // AStar
                Printer.AddAStarResultToStringBuilder(sb, aStarSolution);

                // LDFS
                Printer.AddLdfsResultToStringBuilder(sb, ldfsSolution);
                sb.AppendLine();
            }
            catch (Exception e)
            {
                sb.Append(e.Message).AppendLine();
            }

            Console.WriteLine(sb);
        }


        private static (AStarSolutionInfo AStarSolution, LdfsSolutionInfo LdfsSolution) SolveBoard(
            int[,] chessBoard)
        {
            var aStarSolution = new AStarSolver().Solve(new Board(chessBoard, BoardSize));
            LdfsSolutionInfo ldfsSolution;
            try
            {
                ldfsSolution = new LdfsSolver().Solve(new Board(chessBoard, 8));
            }
            catch (DepthLimitReached e)
            {
                ldfsSolution = e.SolutionInfo;
            }

            return new ValueTuple<AStarSolutionInfo, LdfsSolutionInfo>(aStarSolution, ldfsSolution);
        }
    }
}