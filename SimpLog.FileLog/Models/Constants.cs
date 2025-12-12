using System.IO;

namespace SimpLog.FileLog.Models
{
    public class Constants
    {
        public const string DateFormat = "yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'";

        public const string FileFormat = ".txt";

        public const string Separator = " | ";

        public static readonly char PathSeparator = Path.DirectorySeparatorChar;
    }
}
