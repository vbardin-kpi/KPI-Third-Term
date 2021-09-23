using System;

namespace EightQueens.LDFS
{
    internal class DepthLimitReached : Exception
    {
        public LdfsSolutionInfo SolutionInfo { get; }

        public DepthLimitReached(LdfsSolutionInfo solutionInfo)
        {
            SolutionInfo = solutionInfo;
        }
    }
}