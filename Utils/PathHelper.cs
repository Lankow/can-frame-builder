using System.IO;

namespace CanFrameBuilder.Utils
{
    public class PathHelper
    {
        public static bool IsSubPath(string filePath, string baseDir)
        {
            try
            {
                var fullFilePath = Path.GetFullPath(filePath).TrimEnd(Path.DirectorySeparatorChar);
                var fullBaseDir = Path.GetFullPath(baseDir).TrimEnd(Path.DirectorySeparatorChar);

                return fullFilePath.StartsWith(fullBaseDir + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
                       || string.Equals(fullFilePath, fullBaseDir, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}
