using SixLabors.ImageSharp;

namespace Watermark.Net.src.WatermarkNet.Core
{
    /// <summary>
    /// Abstraction for file system operations
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        /// Loads an image from the specified path.
        /// </summary>
        Image LoadImage(string path);

        /// <summary>
        /// Saves an image to the specified path.
        /// </summary>
        void SaveImage(Image image, string path);

        /// <summary>
        /// Enumerates all files in a directory (non-recursive, top-level only).
        /// Skips directories and hidden files.
        /// </summary>
        IEnumerable<string> EnumerateFiles(string directory);

        /// <summary>
        /// Creates the directory if it does not exist.
        /// </summary>
        void EnsureDirectoryExists(string path);

        /// <summary>
        /// Throws <see cref="FileNotFoundException"/> if the file does not exist.
        /// </summary>
        void ValidateFileExists(string path);

        /// <summary>
        /// Combines a directory path with a file name using the platform separator.
        /// </summary>
        string CombinePath(string directory, string fileName);
    }
}
