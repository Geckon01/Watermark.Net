using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace Watermark.Net.src.WatermarkNet.Models.Styling
{
    public class WatermarkStyle
    {
        /// <summary>
        /// Gets or sets the transparency level of the watermark.
        /// Range: 0.0 (fully transparent) to 1.0 (completely opaque).
        /// Default: 1.0
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when value is outside the 0.0-1.0 range.
        /// </exception>
        public float Opacity { get; set; }

        public bool Pave { get; set; }
        public Color Color { get; set; }
    }
}
