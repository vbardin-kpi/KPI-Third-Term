using EightQueens.Common;

namespace EightQueens.AStar
{
    internal record AStarNode
    {
        public int Weight { get; set; }
        public Board Board { get; init; }
        public QueenPositionUpdate PositionUpdate { get; init; }
    }
}