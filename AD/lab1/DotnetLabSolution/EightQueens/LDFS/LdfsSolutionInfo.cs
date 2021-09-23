using EightQueens.Common;

namespace EightQueens.LDFS
{
    public record LdfsSolutionInfo
    {
        public int Depth { get; init; }
        public int BackTracks { get; init; }
        public int TotalSteps { get; init; }
        public Board SolvedBoard { get; init; }
    }
}