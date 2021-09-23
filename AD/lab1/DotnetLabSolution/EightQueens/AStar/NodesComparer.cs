using System.Collections.Generic;

namespace EightQueens.AStar
{
    internal class NodesComparer : IComparer<int>
    {
        public int Compare(int x, int y) => y - x;
    }
}