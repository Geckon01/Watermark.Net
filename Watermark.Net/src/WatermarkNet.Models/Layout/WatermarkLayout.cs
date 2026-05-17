using Watermark.Net.src.WatermarkNet.Enums;

namespace Watermark.Net.src.WatermarkNet.Models.Layout
{
    public class WatermarkLayout
    {
        public ImagePosition Position { get; set; }

        public float Scale { get; set; }

        public int RotateAngle { get; set; }

        /// <summary>
        /// Gets or sets the padding space around the watermark in pixels.
        /// Default: 10px
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when value is negative.</exception>
        public float Padding { get; set; }
    }
}
