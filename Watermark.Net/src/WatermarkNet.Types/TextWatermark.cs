using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace Watermark.Net.src.WatermarkNet.Types
{
    /// <summary>
    /// Represents a configurable text-based watermark with styling, positioning, and rendering options.
    /// </summary>
    public class TextWatermark : WatermarkImageBase
    {
        private string _text;
        private Font _font;
        private float _padding;
        private Color _color;

        /// <summary>
        /// Gets or sets the color of the text watermark.
        /// Default: White (#FFFFFF)
        /// </summary>
        public Color Color
        {
            get => _color;
            set => _color = value;
        }

        /// <summary>
        /// Gets or sets the text content for the watermark.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when value is null or whitespace.</exception>
        public string Text
        {
            get => _text;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Watermark text cannot be empty", nameof(Text));
                _text = value;
            }
        }

        /// <summary>
        /// Gets or sets the font used to render the text watermark.
        /// Default: Arial 12pt
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when value is null.</exception>
        public Font Font
        {
            get => _font;
            set => _font = value ?? throw new ArgumentNullException(nameof(Font));
        }

        /// <summary>
        /// Gets or sets the padding space around the text watermark in pixels.
        /// Default: 10px
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when value is negative.</exception>
        public float Padding
        {
            get => _padding;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(Padding), "Padding cannot be negative");
                _padding = value;
            }
        }

        /// <summary>
        /// Gets or sets the rotation angle for the text watermark in degrees.
        /// Default: 0 (no rotation)
        /// </summary>
        public float Rotation { get; set; } = 0;

        /// <summary>
        /// Initializes a new instance of the TextWatermark class with default values.
        /// </summary>
        public TextWatermark()
        {
            var availableFont = SystemFonts.Families.FirstOrDefault();
            if (availableFont == default)
            {
                throw new Exception("No available fonts found in the system");
            }
            _color = Color.White;
            _font = availableFont.CreateFont(1);
            _padding = 10f;
        }
    }
}