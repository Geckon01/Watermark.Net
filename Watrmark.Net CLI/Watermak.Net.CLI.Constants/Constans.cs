using SixLabors.Fonts;
using Watermark.Net.src.WatermarkNet.Enums;

namespace Watrmark.Net_CLI.Watermak.Net.CLI.Constants
{
    internal class Constans
    {
        public static readonly SixLabors.ImageSharp.Color DefaultTextColor = SixLabors.ImageSharp.Color.LightGray;
        public static readonly SixLabors.ImageSharp.Color DefaultBackroundColor = SixLabors.ImageSharp.Color.White;
        public static readonly ImagePosition DefaultWatermarkPosition = ImagePosition.Center;
        public static readonly Font DefaultWatermarkFont = SystemFonts.CreateFont("Tahoma", 14);
        public static readonly float DefaultWatermarkScale = 1;
    }
}
