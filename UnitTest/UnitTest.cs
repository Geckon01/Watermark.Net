using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Watermark.Net.src.WatermarkNet.Core;
using Watermark.Net.src.WatermarkNet.Enums;
using Watermark.Net.src.WatermarkNet.Models.Definitions;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TextWatermarkLegacyTest()
        {
            var watermarker = new Watermarker();

            var availableFont = SystemFonts.Families.FirstOrDefault();
            if (availableFont == default)
            {
                throw new Exception("No available fonts found in the system");
            }

            var watermark = new TextWatermark{
                Font = availableFont.CreateFont(1),
                Text = "Test",
                Style = { Color = Color.White },
                Layout = { Position = ImagePosition.BottomCenter , RotateAngle = 90 }
            };

            var resultedImage = watermarker.ProcessImage("TestImages/2.png", "test/text", watermark);

            Assert.IsNotNull(resultedImage);
            Assert.IsTrue(File.Exists(resultedImage.Path));
        }

        [TestMethod]
        public void ImageWatermarkLegacyTest()
        {
            var watermarker = new Watermarker();
            var watermark = new ImageWatermark { 
                ImagePath = "TestImages/sample_wm.png",
                Layout = { Position = ImagePosition.Center, Scale = 1 },
                Style = { Opacity = 1 }
            };

            var resultedImage = watermarker.ProcessImage("TestImages/2.png", "test/image", watermark);

            Assert.IsNotNull(resultedImage);
            Assert.IsTrue(File.Exists(resultedImage.Path));
        }

        [TestMethod]
        public void TextWatermarkDirectoryProccessLegacyTest()
        {
            var watermarker = new Watermarker("test/text/pave");

            var availableFont = SystemFonts.Families.FirstOrDefault();
            if (availableFont == default)
            {
                throw new Exception("No available fonts found in the system");
            }

            var watermark = new TextWatermark { 
                Text = "Test",
                Font = availableFont.CreateFont(1),
                Style = { Color = Rgba32.ParseHex("FFFFFF50"), Pave = true },
                Layout = { Scale = 1f, Position = ImagePosition.TopLeft }
            };

            watermarker.ProcessDirectory("TestImages", watermark);

            Assert.IsTrue(Directory.GetFiles(watermarker.OutputDir)?.Length > 0);
        }

        [TestMethod]
        public void ImageWatermarkDirectoryProccessLegacyTest()
        {
            var watermarker = new Watermarker("test/image/pave");
            var watermark = new ImageWatermark { 
                ImagePath = "TestImages/sample_wm.png",
                Layout = { Position = ImagePosition.Center, Scale = 1},
                Style = { Pave = true, Opacity = 1}
            };

            watermarker.ProcessDirectory("TestImages", watermark);

            Assert.IsTrue(Directory.GetFiles(watermarker.OutputDir)?.Length > 0);
        }

        [TestMethod]
        public void TextWatermarkPipelineTest()
        {
            var fileManager = new FileManager();
            var renderer = new ImageRenderer();
            var pipeline = new WatermarkPipeline(fileManager, renderer);

            var availableFont = SystemFonts.Families.FirstOrDefault();
            if (availableFont == default)
            {
                throw new Exception("No available fonts found in the system");
            }

            var watermark = new TextWatermark{
                Font = availableFont.CreateFont(1),
                Text = "Test",
                Style = { Color = Color.White },
                Layout = { Position = ImagePosition.BottomCenter , RotateAngle = 90 }
            };

            var resultedImage = pipeline.ProcessImage("TestImages/2.png", "test/pipeline/text", watermark);

            Assert.IsNotNull(resultedImage);
            Assert.IsTrue(File.Exists(resultedImage.Path));
        }

        [TestMethod]
        public void ImageWatermarkPipelineTest()
        {
            var fileManager = new FileManager();
            var renderer = new ImageRenderer();
            var pipeline = new WatermarkPipeline(fileManager, renderer);

            var watermark = new ImageWatermark { 
                ImagePath = "TestImages/sample_wm.png",
                Layout = { Position = ImagePosition.Center, Scale = 1 },
                Style = { Opacity = 1 }
            };

            var resultedImage = pipeline.ProcessImage("TestImages/2.png", "test/pipeline/image", watermark);

            Assert.IsNotNull(resultedImage);
            Assert.IsTrue(File.Exists(resultedImage.Path));
        }

        [TestMethod]
        public void TextWatermarkDirectoryPipelineTest()
        {
            var fileManager = new FileManager();
            var renderer = new ImageRenderer();
            var pipeline = new WatermarkPipeline(fileManager, renderer);

            var availableFont = SystemFonts.Families.FirstOrDefault();
            if (availableFont == default)
            {
                throw new Exception("No available fonts found in the system");
            }

            var watermark = new TextWatermark { 
                Text = "Test",
                Font = availableFont.CreateFont(1),
                Style = { Color = Rgba32.ParseHex("FFFFFF50"), Pave = true },
                Layout = { Scale = 1f, Position = ImagePosition.TopLeft }
            };

            var results = pipeline.ProcessDirectory("TestImages", "test/pipeline/text/dir", watermark);

            Assert.IsTrue(results.Count > 0);
        }

        [TestMethod]
        public void ImageWatermarkDirectoryPipelineTest()
        {
            var fileManager = new FileManager();
            var renderer = new ImageRenderer();
            var pipeline = new WatermarkPipeline(fileManager, renderer);

            var watermark = new ImageWatermark { 
                ImagePath = "TestImages/sample_wm.png",
                Layout = { Position = ImagePosition.Center, Scale = 1},
                Style = { Pave = true, Opacity = 1}
            };

            var results = pipeline.ProcessDirectory("TestImages", "test/pipeline/image/dir", watermark);

            Assert.IsTrue(results.Count > 0);
        }
    }
}
