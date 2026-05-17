using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watermark.Net.src.WatermarkNet.Models.Definitions
{
    public class ResultImage
    {
        public string Path { get; }
        public Image Image { get; }
        public ResultImage(Image image, string path)
        {
            this.Image = image;
            this.Path = path;
        }
    }
}
