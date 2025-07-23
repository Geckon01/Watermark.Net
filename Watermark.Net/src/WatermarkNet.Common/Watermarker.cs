using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Watermark.Net.src.WatermarkNet.Enums;
using Watermark.Net.src.WatermarkNet.Types;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Image = SixLabors.ImageSharp.Image;

namespace Watermark.Net.src.WatermarkNet.Core
{
    public class Watermarker
    {
        private string _outputDir;

        /// <summary>
        /// Gets or sets the output directory for processed images.
        /// </summary>
        public string OutputDir { get { return _outputDir; } set { _outputDir = value;  } }
        public Watermarker()
        { 
        
        }

        public Watermarker(string outputDir)
        {
            _outputDir = outputDir;
        }

        /// <summary>
        /// Processes all images in a directory applying watermark.
        /// </summary>
        /// <param name="directory">Source directory containing images to process.</param>
        /// <param name="watermark">Watermark configuration.</param>
        /// <returns>List of processed images with watermark information.</returns>
        public List<WmarkedImage> ProcessDirectory<T>(string directory, T watermark)
            where T : WatermarkImageBase
        {
            List<WmarkedImage> processedImages = new List<WmarkedImage>();
            foreach (var imageFile in Directory.GetFiles(directory))
            {
                //Do not process directories and hidden files
                if (File.GetAttributes(imageFile).HasFlag(FileAttributes.Directory) || File.GetAttributes(imageFile).HasFlag(FileAttributes.Hidden))
                    continue;

                WmarkedImage? resultedImage = null;
                if (typeof(T).IsAssignableTo(typeof(ImageWatermark)))
                {
                    var concreateWatermark = (ImageWatermark)Convert.ChangeType(watermark, typeof(ImageWatermark));
                    resultedImage = ProcessImage(imageFile, this.OutputDir, concreateWatermark);
                }
                if (typeof(T).IsAssignableTo(typeof(TextWatermark)))
                {
                    var concreateWatermark = (TextWatermark)Convert.ChangeType(watermark, typeof(TextWatermark));
                    resultedImage = ProcessImage(imageFile, this.OutputDir, concreateWatermark);
                }
                if (resultedImage != null)
                    processedImages.Add(resultedImage);
            }
            return processedImages;
        }

        /// <summary>
        /// Applies an image watermark to a single image.
        /// </summary>
        /// <param name="imagePath">Path to source image file.</param>
        /// <param name="outputDirectory">Output directory for processed image.</param>
        /// <param name="watermark">Image watermark configuration.</param>
        /// <returns>Processed image information or null on failure.</returns>
        /// <exception cref="FileNotFoundException">Thrown when source image or watermark image is missing.</exception>
        public WmarkedImage? ProcessImage(string imagePath, string outputDirectory, ImageWatermark watermark)
        {
            if (!File.Exists(imagePath)) { throw new FileNotFoundException("Source file not found!", imagePath); }
            if (!File.Exists(watermark.ImagePath)) { throw new FileNotFoundException("Watermark file not found!", imagePath); }
            if (!Directory.Exists(outputDirectory)) { Directory.CreateDirectory(outputDirectory); }

            WmarkedImage? resultedImage = null;
            using (var targetImage = Image.Load(imagePath))
            using (var watermarkImage = Image.Load(watermark.ImagePath))
            {
                var scaledWmWidth = (int)Math.Round(watermarkImage.Width * watermark.Scale);
                var scaledWmHeight = (int)Math.Round(watermarkImage.Height * watermark.Scale);
                watermarkImage.Mutate(x => x.Resize(new Size(scaledWmWidth, scaledWmHeight)));

                using (var markedImage = targetImage.Clone(ctx => this.ApplyScalingWaterMarkImage(ctx, watermark, watermarkImage, targetImage)))
                {
                    resultedImage = new WmarkedImage(markedImage, outputDirectory + Path.DirectorySeparatorChar + Path.GetFileName(imagePath));
                    markedImage.Save(resultedImage.Path);
                }
            }
            return resultedImage;
        }

        /// <summary>
        /// Applies a text watermark to a single image.
        /// </summary>
        /// <param name="imagePath">Path to source image file.</param>
        /// <param name="outputDirectory">Output directory for processed image.</param>
        /// <param name="watermark">Text watermark configuration.</param>
        /// <returns>Processed image information or null on failure.</returns>
        /// <exception cref="FileNotFoundException">Thrown when source image is missing.</exception>
        public WmarkedImage? ProcessImage(string imagePath, string outputDirectory, TextWatermark watermark)
        {
            if(!File.Exists(imagePath)) { throw new FileNotFoundException("Source file not found!", imagePath); }
            if (!Directory.Exists(outputDirectory)) { Directory.CreateDirectory(outputDirectory); }

            WmarkedImage? resultedImage = null;
            using (var targetImage = Image.Load(imagePath))
            {
                using (var markedImage = targetImage.Clone(ctx => this.ApplyScalingWaterMarkText(ctx, watermark)))
                {
                    resultedImage = new WmarkedImage(markedImage, outputDirectory + Path.DirectorySeparatorChar + Path.GetFileName(imagePath));
                    markedImage.Save(resultedImage.Path);
                }
            }
            return resultedImage;
        }

        /// <summary>
        /// Calculates origin point for text watermark based on position and size.
        /// </summary>
        /// <param name="width">Target image width.</param>
        /// <param name="height">Target image height.</param>
        /// <param name="watermarkSize">Watermark text size.</param>
        /// <param name="position">Position on target image.</param>
        /// <returns>Calculated origin point coordinates.</returns>
        private PointF CalcWatermarkOrigin(int width, int height, float watermarkSize, ImagePosition position)
        {
            var origin = new PointF(0, 0);
            //Static value 1 pt is 72 px per inch
            var pixelsPerInch = 72;
            var wmHeight = watermarkSize / pixelsPerInch;

            switch (position)
            {
                case ImagePosition.TopLeft:
                    origin = new PointF(watermarkSize / 2, wmHeight / 2);
                    break;
                case ImagePosition.TopCenter:
                    origin = new PointF(width / 2, wmHeight / 2);
                    break;
                case ImagePosition.TopRight:
                    origin = new PointF(width - watermarkSize * 2, wmHeight / 2);
                    break;
                case ImagePosition.CenterLeft:
                    origin = new PointF(watermarkSize / 2, height / 2.5f);
                    break;
                case ImagePosition.Center:
                    origin = new PointF(width / 2, height / 2.5f);
                    break;
                case ImagePosition.CenterRight:
                    origin = new PointF(width - watermarkSize * 2, height / 2.5f);
                    break;
                case ImagePosition.BottomLeft:
                    origin = new PointF(watermarkSize / 2, height - watermarkSize * 2);
                    break;
                case ImagePosition.BottomCenter:
                    origin = new PointF(width / 2, height - watermarkSize * 2);
                    break;
                case ImagePosition.BottomRight:
                    origin = new PointF(width - watermarkSize * 2, height - watermarkSize * 2);
                    break;
                default:
                    break;
            }
            return origin;
        }

        /// <summary>
        /// Calculates origin point for image watermark based on position and dimensions.
        /// </summary>
        /// <param name="width">Target image width.</param>
        /// <param name="height">Target image height.</param>
        /// <param name="wmWidth">Watermark image width.</param>
        /// <param name="wmHeight">Watermark image height.</param>
        /// <param name="position">Position on target image.</param>
        /// <returns>Calculated origin point coordinates.</returns>
        private Point CalcWatermarkOrigin(int width, int height, int wmWidth,int wmHeight, ImagePosition position)
        {
            var origin = new Point(0, 0);

            var wmPaddingX = width - wmWidth;
            var paddingSide = wmPaddingX / 2;

            switch (position)
            {
                case ImagePosition.TopLeft:
                    origin = new Point(wmWidth / 2, wmHeight / 2);
                    break;
                case ImagePosition.TopCenter:
                    origin = new Point((int)paddingSide, wmHeight / 2);
                    break;
                case ImagePosition.TopRight:
                    origin = new Point(width - wmWidth * 2, wmHeight / 2);
                    break;
                case ImagePosition.CenterLeft:
                    origin = new Point(wmWidth / 2, height / 2);
                    break;
                case ImagePosition.Center:
                    origin = new Point((int)paddingSide, (int)(height / 2));
                    break;
                case ImagePosition.CenterRight:
                    origin = new Point(width - wmWidth * 2, height / 2);
                    break;
                case ImagePosition.BottomLeft:
                    origin = new Point(wmWidth / 2, height - wmHeight);
                    break;
                case ImagePosition.BottomCenter:
                    origin = new Point((int)paddingSide, height - wmHeight);
                    break;
                case ImagePosition.BottomRight:
                    origin = new Point(width - wmWidth * 2, height - wmHeight);
                    break;
                default:
                    break;
            }
            return origin;
        }

        /// <summary>
        /// Determines horizontal alignment based on watermark position.
        /// </summary>
        /// <param name="imagePosition">Watermark position on image.</param>
        /// <returns>Corresponding horizontal alignment setting.</returns>
        private HorizontalAlignment HorizontalAlignmentFromPosition(ImagePosition imagePosition) 
        {
            switch (imagePosition)
            {
                case ImagePosition.TopCenter:
                    return HorizontalAlignment.Center;
                    break;
                case ImagePosition.Center:
                    return HorizontalAlignment.Center;
                    break;
                case ImagePosition.BottomCenter:
                    return HorizontalAlignment.Center;
                    break;
            }
            return HorizontalAlignment.Left; 
        }

        /// <summary>
        /// Applies text watermark to image with automatic scaling and positioning.
        /// </summary>
        /// <param name="processingContext">Image processing context.</param>
        /// <param name="watermark">Text watermark configuration.</param>
        /// <returns>Processing context with applied watermark.</returns>
        private IImageProcessingContext ApplyScalingWaterMarkText(IImageProcessingContext processingContext, TextWatermark watermark)
        {
            Size imgSize = processingContext.GetCurrentSize();
            FontRectangle size = TextMeasurer.MeasureSize(watermark.Text, new TextOptions(watermark.Font));

            // Find out how much we need to scale the text to fill the space (up or down)
            float scalingFactor = Math.Min(imgSize.Width / size.Width, imgSize.Height / size.Height);

            // Create a new font
            Font scaledFont = new Font(watermark.Font, scalingFactor / 16 * (watermark.Font.Size * watermark.Scale));
            ImagePosition[] centerImagePositions = { ImagePosition.CenterLeft, ImagePosition.CenterRight, ImagePosition.Center };
            //processingContext.SetGraphicsOptions(new GraphicsOptions { AlphaCompositionMode = SixLabors.ImageSharp.PixelFormats.PixelAlphaCompositionMode.Clear});
            //If set, apply backround color
            if (watermark.BackroundColor != null)
                processingContext.BackgroundColor((Color)watermark.BackroundColor);

            var textOptions = new RichTextOptions(scaledFont)
            {
                ColorFontSupport = ColorFontSupport.MicrosoftColrFormat,
                Origin = CalcWatermarkOrigin(imgSize.Width, imgSize.Height, scaledFont.Size, watermark.Position),
                HorizontalAlignment = HorizontalAlignmentFromPosition(watermark.Position),
                VerticalAlignment = VerticalAlignment.Top,
            };

            if (watermark.Pave)
            {
               foreach (ImagePosition position in (ImagePosition[])Enum.GetValues(typeof(ImagePosition)))
                {
                    textOptions.Origin = CalcWatermarkOrigin(imgSize.Width, imgSize.Height, scaledFont.Size, position);
                    textOptions.HorizontalAlignment = HorizontalAlignmentFromPosition(position);
                    processingContext.DrawText(textOptions, watermark.Text, watermark.Color);
                }
                return processingContext;
            }
            return processingContext
                .DrawText(textOptions, watermark.Text, watermark.Color);
        }

        /// <summary>
        /// Applies image watermark to target image with scaling and positioning.
        /// </summary>
        /// <param name="processingContext">Image processing context.</param>
        /// <param name="watermark">Image watermark configuration.</param>
        /// <param name="watermarkImage">Watermark image instance.</param>
        /// <param name="targetImage">Target image being processed.</param>
        /// <returns>Processing context with applied watermark.</returns>
        private IImageProcessingContext ApplyScalingWaterMarkImage(IImageProcessingContext processingContext, ImageWatermark watermark, Image watermarkImage, Image targetImage)
        {
            var scaleFactor = 1f;
            if (targetImage.Width > targetImage.Height)
                scaleFactor = (float)targetImage.Width / targetImage.Height;
            else
                scaleFactor = (float) targetImage.Height / targetImage.Width;

            var wmPaddingX = (targetImage.Width - watermarkImage.Width) / 2;
            var wmPaddingY = (targetImage.Height - watermarkImage.Height) / 2;
            //scaleFactor = 1;
            var minWmPadding = 50;
            var scaledWmWidth = wmPaddingX > minWmPadding && wmPaddingY > minWmPadding
                ?  watermarkImage.Width * scaleFactor
                : watermarkImage.Width / scaleFactor;
            var scaledWmHeight = wmPaddingX > minWmPadding && wmPaddingY > minWmPadding
                   ? watermarkImage.Height * scaleFactor
                   : watermarkImage.Height / scaleFactor;

            //If set, apply backround color
            if (watermark.BackroundColor != null)
                processingContext.BackgroundColor((Color)watermark.BackroundColor);

            watermarkImage.Mutate(x => x.Resize(new Size((int)scaledWmWidth, (int)scaledWmHeight)));
            var wmPositionOrigin = CalcWatermarkOrigin(targetImage.Width, targetImage.Height, watermarkImage.Width, watermarkImage.Height, watermark.Position);

            if (watermark.Pave)
            {
                foreach (ImagePosition position in (ImagePosition[])Enum.GetValues(typeof(ImagePosition)))
                {
                    wmPositionOrigin = CalcWatermarkOrigin(targetImage.Width, targetImage.Height, watermarkImage.Width, watermarkImage.Height, position);
                    try
                    {
                        processingContext.DrawImage(watermarkImage, wmPositionOrigin, watermark.Opacity);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                return processingContext;
            }

            return processingContext.DrawImage(watermarkImage, wmPositionOrigin, watermark.Opacity);
        }
    }
}