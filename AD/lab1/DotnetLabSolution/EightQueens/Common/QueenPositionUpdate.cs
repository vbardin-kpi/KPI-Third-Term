namespace EightQueens.Common
{
    public record QueenPositionUpdate
    {
        public Position CurrentPosition { get; init; }
        public Position NewPosition { get; init; }
    }
}