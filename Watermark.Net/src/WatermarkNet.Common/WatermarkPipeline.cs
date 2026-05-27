using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Watermark.Net.src.WatermarkNet.Models.Definitions;

namespace Watermark.Net.src.WatermarkNet.Core
{
    /// <summary>
    /// Orchestrates the watermark processing pipeline.
    /// Coordinates file I/O (via <see cref="IFileManager"/>) and rendering (via <see cref="ImageRenderer"/>)
    /// to provide high-level operations for processing single images and directories.
    /// </summary>
    public class WatermarkPipeline
    {
        private readonly IFileManager _fileManager;
        private readonly ImageRenderer _renderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkPipeline"/> class.
        /// </summary>
        /// <param name="fileManager">File system abstraction for I/O operations.</param>
        /// <param name="renderer">Pure rendering engine for watermark application.</param>
        public WatermarkPipeline(IFileManager fileManager, ImageRenderer renderer)
        {
            _fileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager));
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        /// <summary>
        /// Applies an image watermark to a single image and saves the result.
        /// </summary>
        /// <param name="imagePath">Path to the source image file.</param>
        /// <param name="outputDirectory">Directory where the processed image will be saved.</param>
        /// <param name="watermark">Image watermark configuration.</param>
        /// <returns>Result information about the processed image.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the source image or watermark image is missing.</exception>
        public ResultImage ProcessImage(string imagePath, string outputDirectory, ImageWatermark watermark)
        {
            ArgumentNullException.ThrowIfNull(watermark);

            _fileManager.ValidateFileExists(imagePath);
            _fileManager.ValidateFileExists(watermark.ImagePath);
            _fileManager.EnsureDirectoryExists(outputDirectory);

            using var targetImage = _fileManager.LoadImage(imagePath);
            using var watermarkImage = _fileManager.LoadImage(watermark.ImagePath);

            var scaledWmWidth = (int)Math.Round(watermarkImage.Width * watermark.Layout.Scale);
            var scaledWmHeight = (int)Math.Round(watermarkImage.Height * watermark.Layout.Scale);
            watermarkImage.Mutate(x => x.Resize(new Size(scaledWmWidth, scaledWmHeight)));

            using var markedImage = _renderer.ApplyImageWatermark(targetImage, watermarkImage, watermark);

            var outputPath = _fileManager.CombinePath(outputDirectory, Path.GetFileName(imagePath));
            _fileManager.SaveImage(markedImage, outputPath);

            return new ResultImage(markedImage, outputPath);
        }

        /// <summary>
        /// Applies a text watermark to a single image and saves the result.
        /// </summary>
        /// <param name="imagePath">Path to the source image file.</param>
        /// <param name="outputDirectory">Directory where the processed image will be saved.</param>
        /// <param name="watermark">Text watermark configuration.</param>
        /// <returns>Result information about the processed image.</returns>
        /// <exception cref="FileNotFoundException">Thrown when the source image is missing.</exception>
        public ResultImage ProcessImage(string imagePath, string outputDirectory, TextWatermark watermark)
        {
            ArgumentNullException.ThrowIfNull(watermark);

            _fileManager.ValidateFileExists(imagePath);
            _fileManager.EnsureDirectoryExists(outputDirectory);

            using var targetImage = _fileManager.LoadImage(imagePath);
            using var markedImage = _renderer.ApplyTextWatermark(targetImage, watermark);

            var outputPath = _fileManager.CombinePath(outputDirectory, Path.GetFileName(imagePath));
            _fileManager.SaveImage(markedImage, outputPath);

            return new ResultImage(markedImage, outputPath);
        }

        /// <summary>
        /// Processes all images in a directory, applying the specified image watermark to each.
        /// </summary>
        /// <param name="directory">Source directory containing images to process.</param>
        /// <param name="outputDirectory">Directory where processed images will be saved.</param>
        /// <param name="watermark">Image watermark configuration.</param>
        /// <returns>A list of result information for each processed image.</returns>
        public List<ResultImage> ProcessDirectory(string directory, string outputDirectory, ImageWatermark watermark)
        {
            ArgumentNullException.ThrowIfNull(watermark);

            var processedImages = new List<ResultImage>();

            foreach (var imageFile in _fileManager.EnumerateFiles(directory))
            {
                var resultedImage = ProcessImage(imageFile, outputDirectory, watermark);
                processedImages.Add(resultedImage);
            }

            return processedImages;
        }

        /// <summary>
        /// Processes all images in a directory, applying the specified text watermark to each.
        /// </summary>
        /// <param name="directory">Source directory containing images to process.</param>
        /// <param name="outputDirectory">Directory where processed images will be saved.</param>
        /// <param name="watermark">Text watermark configuration.</param>
        /// <returns>A list of result information for each processed image.</returns>
        public List<ResultImage> ProcessDirectory(string directory, string outputDirectory, TextWatermark watermark)
        {
            ArgumentNullException.ThrowIfNull(watermark);

            var processedImages = new List<ResultImage>();

            foreach (var imageFile in _fileManager.EnumerateFiles(directory))
            {
                var resultedImage = ProcessImage(imageFile, outputDirectory, watermark);
                processedImages.Add(resultedImage);
            }

            return processedImages;
        }
    }
}
