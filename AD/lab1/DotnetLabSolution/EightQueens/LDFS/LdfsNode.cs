using System.Collections.Generic;

using EightQueens.Common;

namespace EightQueens.LDFS
{
    internal record LdfsNode
    {
        public int Depth { get; set; }
        public Board Board { get; init; }
        public QueenPositionUpdate PositionUpdate { get; init; }
        public LdfsNode ParentLdfsNode { get; init; }
        public List<LdfsNode> Nodes { get; set; }
    }
}