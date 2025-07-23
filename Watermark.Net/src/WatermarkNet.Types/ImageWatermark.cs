using System;
using System.IO;

namespace Watermark.Net.src.WatermarkNet.Types
{
    /// <summary>
    /// Represents a configurable image-based watermark with scaling, positioning, and transparency controls.
    /// </summary>
    public class ImageWatermark : WatermarkImageBase
    {
        private string _imagePath;
        private float? _opacity;

        /// <summary>
        /// Gets or sets the absolute path to the watermark image file.
        /// Supported formats: PNG, JPEG, BMP, GIF.
        /// </summary>
        /// <exception cref="FileNotFoundException">
        /// Thrown when setting a value that doesn't point to an existing file.
        /// </exception>
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (!File.Exists(value))
                    throw new FileNotFoundException("Watermark image not found", value);

                _imagePath = value;
            }
        }

        /// <summary>
        /// Gets or sets the transparency level of the watermark.
        /// Range: 0.0 (fully transparent) to 1.0 (completely opaque).
        /// Default: 1.0
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when value is outside the 0.0-1.0 range.
        /// </exception>
        public float Opacity
        {
            get => _opacity ?? 1f;
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException(nameof(Opacity),
                        "Opacity must be between 0.0 and 1.0");

                _opacity = value;
            }
        }

        /// <summary>
        /// Gets or sets the scale factor applied to the watermark image relative to the target image.
        /// Default: 0.2 (20% of target image width)
        /// </summary>
        public float Scale { get; set; } = 0.2f;

        /// <summary>
        /// Initializes a new instance of the ImageWatermark class with specified image path.
        /// </summary>
        /// <param name="imagePath">Path to the watermark image file.</param>
        public ImageWatermark(string imagePath) : this()
        {
            ImagePath = imagePath;
        }

        /// <summary>
        /// Initializes a new instance of the ImageWatermark class with default values.
        /// </summary>
        public ImageWatermark() { }

    }
}