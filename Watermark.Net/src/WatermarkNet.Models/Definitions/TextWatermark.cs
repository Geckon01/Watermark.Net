using SixLabors.Fonts;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Watermark.Net.src.WatermarkNet.Models.Layout;
using Watermark.Net.src.WatermarkNet.Models.Styling;

namespace Watermark.Net.src.WatermarkNet.Models.Definitions
{
    public class TextWatermark : IWatermarkDefinition
    {
        public required string Text { get; set; }

        public required Font Font { get; set; }

        public WatermarkLayout Layout { get; init; }
            = new();

        public WatermarkStyle Style { get; init; }
            = new();
    }
}
