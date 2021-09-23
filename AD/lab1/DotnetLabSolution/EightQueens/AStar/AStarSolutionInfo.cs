using EightQueens.Common;

namespace EightQueens.AStar
{
    public record AStarSolutionInfo
    {
        public int ClosedStates { get; init; }
        public int OpenStates { get; init; }
        public Board SolvedBoard { get; init; }
    }
}