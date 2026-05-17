using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watermark.Net.src.WatermarkNet.Models.Layout;
using Watermark.Net.src.WatermarkNet.Models.Styling;

namespace Watermark.Net.src.WatermarkNet.Models.Definitions
{
    public class ImageWatermark : IWatermarkDefinition
    {
        public required string ImagePath { get; init; }

        public WatermarkLayout Layout
        {
            get;
            init;
        } = new();


        public WatermarkStyle Style
        {
            get;
            init;
        } = new();
    }
}
