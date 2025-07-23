using SixLabors.ImageSharp;

namespace Watermark.Net.src.WatermarkNet.Types
{
    public class WmarkedImage
    {
        public string Path { get; }
        public Image Image { get; }
        public WmarkedImage(Image image, string path) 
        {
            this.Image = image;
            this.Path= path;
        }
    }
}
