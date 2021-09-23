using System;

namespace EightQueens.Common
{
    public readonly struct Position
    {
        public int Row { get; }
        public int Column { get; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override bool Equals(object obj)
        {
            return obj is Position other &&
                   Row == other.Row &&
                   Column == other.Column;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }

        public override string ToString()
        {
            return "[" + Row + "; " + Column + "]";
        }

        public static bool operator ==(Position p1, Position p2) => p1.Row == p2.Row && p1.Column == p2.Column;

        public static bool operator !=(Position p1, Position p2) => !(p1 == p2);
    }
}