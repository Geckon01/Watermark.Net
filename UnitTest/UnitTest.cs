using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Watermark.Net.src.WatermarkNet.Core;
using Watermark.Net.src.WatermarkNet.Types;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TextWatermarkTest()
        {
            var watermarker = new Watermarker();
            var watermark = new TextWatermark();

            watermark.Text = "Test";
            watermark.Color = Color.White;
            watermark.Font = SystemFonts.CreateFont("Arial", 1);
            watermark.Position = Watermark.Net.src.WatermarkNet.Enums.ImagePosition.BottomCenter;
            watermark.RotateAngle = 90;
            var resultedImage = watermarker.ProcessImage("TestImages/2.png", "test/text", watermark);

            Assert.IsTrue(File.Exists(resultedImage.Path));
            Assert.IsNotNull(resultedImage);
        }

        [TestMethod]
        public void ImageWatermarkTest()
        {
            var watermarker = new Watermarker();
            var watermark = new ImageWatermark();

            watermark.ImagePath = "TestImages/sample_wm.png";
            watermark.Position = Watermark.Net.src.WatermarkNet.Enums.ImagePosition.Center;
            watermark.Scale = 1;
            var resultedImage = watermarker.ProcessImage("TestImages/2.png", "test/image", watermark);

            Assert.IsTrue(File.Exists(resultedImage.Path));
            Assert.IsNotNull(resultedImage);
        }

        [TestMethod]
        public void TextWatermarkDirectoryProccessTest()
        {
            var watermarker = new Watermarker("test/text/pave");
            var watermark = new TextWatermark();

            watermark.Text = "Test";
            watermark.Color = Rgba32.ParseHex("FFFFFF50");
            watermark.Font = SystemFonts.CreateFont("Arial", 14);
            watermark.Scale = 1f;
            watermark.Position = Watermark.Net.src.WatermarkNet.Enums.ImagePosition.TopLeft;
            watermark.Pave = true;
            watermarker.ProcessDirectory("TestImages", watermark);

            Assert.IsTrue(Directory.GetFiles(watermarker.OutputDir)?.Length > 0);
        }

        [TestMethod]
        public void ImageWatermarkDirectoryProccessTest()
        {
            var watermarker = new Watermarker("test/image/pave");
            var watermark = new ImageWatermark();

            watermark.ImagePath = "TestImages/sample_wm.png";
            watermark.Position = Watermark.Net.src.WatermarkNet.Enums.ImagePosition.Center;
            watermark.Scale = 1;
            watermark.Pave = true;
            watermarker.ProcessDirectory("TestImages", watermark);

            Assert.IsTrue(Directory.GetFiles(watermarker.OutputDir)?.Length > 0);
        }
    }
}