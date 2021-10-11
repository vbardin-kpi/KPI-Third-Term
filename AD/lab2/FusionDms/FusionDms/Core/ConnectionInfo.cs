using System;

namespace FusionDms.Core
{
    internal readonly struct ConnectionInfo
    {
        public string FilePath { get; }
        public FusionAccessMode AccessMode { get; }

        public ConnectionInfo(string filePath, string accessMode)
        {
            FilePath = filePath;
            AccessMode = Enum.Parse<FusionAccessMode>(accessMode);
        }
    }
}