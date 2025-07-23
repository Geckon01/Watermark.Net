using SixLabors.ImageSharp;

namespace Watermark.Net.src.WatermarkNet.Types
{
    internal class SourceImage
    {
        private string _path;
        private Image _image;

        public Image Image => _image;
        public String Path => _path;
    }
}
