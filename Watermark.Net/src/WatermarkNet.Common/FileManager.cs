using SixLabors.ImageSharp;

namespace Watermark.Net.src.WatermarkNet.Core
{
    /// <summary>
    /// Default implementation of <see cref="IFileManager"/>.
    /// Encapsulates all file system operations (loading, saving, validation, enumeration).
    /// </summary>
    public class FileManager : IFileManager
    {
        /// <inheritdoc />
        public Image LoadImage(string path)
        {
            return Image.Load(path);
        }

        /// <inheritdoc />
        public void SaveImage(Image image, string path)
        {
            image.Save(path);
        }

        /// <inheritdoc />
        public IEnumerable<string> EnumerateFiles(string directory)
        {
            foreach (var filePath in Directory.GetFiles(directory))
            {
                var attributes = File.GetAttributes(filePath);
                bool isDirectory = attributes.HasFlag(FileAttributes.Directory);
                bool isHidden = attributes.HasFlag(FileAttributes.Hidden);

                if (!isDirectory && !isHidden)
                {
                    yield return filePath;
                }
            }
        }

        /// <inheritdoc />
        public void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <inheritdoc />
        public void ValidateFileExists(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Source file not found!", path);
            }
        }

        /// <inheritdoc />
        public string CombinePath(string directory, string fileName)
        {
            return directory + Path.DirectorySeparatorChar + fileName;
        }
    }
}
