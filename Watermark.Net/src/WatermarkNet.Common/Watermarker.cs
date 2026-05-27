using SixLabors.ImageSharp;
using Watermark.Net.src.WatermarkNet.Models.Definitions;
using Image = SixLabors.ImageSharp.Image;

namespace Watermark.Net.src.WatermarkNet.Core
{
    /// <summary>
    /// Provides backward compatibility for consumers of the original <c>Watermarker</c> API.
    /// Internally delegates all operations to <see cref="WatermarkPipeline"/>,
    /// <see cref="IFileManager"/>, and <see cref="ImageRenderer"/>.
    /// </summary>
    [Obsolete("Use WatermarkPipeline with IFileManager and ImageRenderer instead. " +
        "This facade will be removed in a future version.")]
    public class Watermarker
    {
        private readonly WatermarkPipeline _pipeline;
        private readonly IFileManager _fileManager;
        private readonly ImageRenderer _renderer;
        private string _outputDir;

        /// <summary>
        /// Gets or sets the output directory for processed images.
        /// Only used by <see cref="ProcessDirectory{T}(string, T)"/> overloads
        /// that rely on the instance-level output directory.
        /// </summary>
        public string OutputDir
        {
            get { return _outputDir; }
            set { _outputDir = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Watermarker"/> class
        /// with default <see cref="FileManager"/> and <see cref="ImageRenderer"/>.
        /// </summary>
        public Watermarker()
            : this(new FileManager(), new ImageRenderer(), string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Watermarker"/> class
        /// with a specified output directory and default dependencies.
        /// </summary>
        /// <param name="outputDir">Default output directory for processed images.</param>
        public Watermarker(string outputDir)
            : this(new FileManager(), new ImageRenderer(), outputDir)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Watermarker"/> class
        /// with custom dependencies (useful for testing).
        /// </summary>
        public Watermarker(IFileManager fileManager, ImageRenderer renderer, string outputDir)
        {
            _fileManager = fileManager ?? new FileManager();
            _renderer = renderer ?? new ImageRenderer();
            _pipeline = new WatermarkPipeline(_fileManager, _renderer);
            _outputDir = outputDir;
        }

        /// <summary>
        /// Processes all images in a directory applying watermark.
        /// Uses the instance-level <see cref="OutputDir"/> as the output directory.
        /// </summary>
        [Obsolete("Use WatermarkPipeline.ProcessDirectory instead.")]
        public List<ResultImage> ProcessDirectory<T>(string directory, T watermark)
            where T : IWatermarkDefinition
        {
            List<ResultImage> processedImages = new List<ResultImage>();

            foreach (var imageFile in _fileManager.EnumerateFiles(directory))
            {
                ResultImage? resultedImage = null;

                if (watermark is ImageWatermark imageWm)
                {
                    resultedImage = _pipeline.ProcessImage(imageFile, _outputDir, imageWm);
                }
                else if (watermark is TextWatermark textWm)
                {
                    resultedImage = _pipeline.ProcessImage(imageFile, _outputDir, textWm);
                }

                if (resultedImage != null)
                    processedImages.Add(resultedImage);
            }

            return processedImages;
        }

        /// <summary>
        /// Applies an image watermark to a single image.
        /// </summary>
        [Obsolete("Use WatermarkPipeline.ProcessImage instead.")]
        public ResultImage? ProcessImage(string imagePath, string outputDirectory, ImageWatermark watermark)
        {
            try
            {
                return _pipeline.ProcessImage(imagePath, outputDirectory, watermark);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Applies a text watermark to a single image.
        /// </summary>
        [Obsolete("Use WatermarkPipeline.ProcessImage instead.")]
        public ResultImage? ProcessImage(string imagePath, string outputDirectory, TextWatermark watermark)
        {
            try
            {
                return _pipeline.ProcessImage(imagePath, outputDirectory, watermark);
            }
            catch
            {
                return null;
            }
        }
    }
}
