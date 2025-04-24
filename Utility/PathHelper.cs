using System.IO;

namespace CanFrameBuilder.Utility;

public class PathHelper
{
    public static bool IsSubPath(string pathA, string pathB)
    {
        try
        {
            static string NormalizePath(string path)
            {
                var fullPath = Path.GetFullPath(path);
                if (File.Exists(fullPath) || Path.HasExtension(fullPath))
                    fullPath = Path.GetDirectoryName(fullPath) ?? fullPath;
                return fullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }

            var dirA = NormalizePath(pathA);
            var dirB = NormalizePath(pathB);

            return dirA.StartsWith(dirB + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
                   || string.Equals(dirA, dirB, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }
}
