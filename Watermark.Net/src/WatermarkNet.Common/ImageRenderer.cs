using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Watermark.Net.src.WatermarkNet.Enums;
using Watermark.Net.src.WatermarkNet.Models.Definitions;

namespace Watermark.Net.src.WatermarkNet.Core
{
    /// <summary>
    /// Pure rendering engine for watermark operations.
    /// Contains only image processing logic — no file I/O, no directory traversal.
    /// Accepts in-memory <see cref="Image"/> objects and returns processed <see cref="Image"/> instances.
    /// </summary>
    public class ImageRenderer
    {
        /// <summary>
        /// Applies a text watermark to the target image.
        /// </summary>
        /// <param name="targetImage">Source image to watermark.</param>
        /// <param name="watermark">Text watermark configuration.</param>
        /// <returns>A new image instance with the watermark applied.</returns>
        public Image ApplyTextWatermark(Image targetImage, TextWatermark watermark)
        {
            return targetImage.Clone(ctx => ApplyScalingWaterMarkText(ctx, watermark));
        }

        /// <summary>
        /// Applies an image watermark to the target image.
        /// </summary>
        /// <param name="targetImage">Source image to watermark.</param>
        /// <param name="watermarkImage">The watermark image (already loaded).</param>
        /// <param name="watermark">Image watermark configuration.</param>
        /// <returns>A new image instance with the watermark applied.</returns>
        public Image ApplyImageWatermark(Image targetImage, Image watermarkImage, ImageWatermark watermark)
        {
            return targetImage.Clone(ctx => ApplyScalingWaterMarkImage(ctx, watermark, watermarkImage, targetImage));
        }

        /// <summary>
        /// Calculates origin point for text watermark based on position and size.
        /// </summary>
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
        private Point CalcWatermarkOrigin(int width, int height, int wmWidth, int wmHeight, ImagePosition position)
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
        private HorizontalAlignment HorizontalAlignmentFromPosition(ImagePosition imagePosition)
        {
            switch (imagePosition)
            {
                case ImagePosition.TopCenter:
                    return HorizontalAlignment.Center;
                case ImagePosition.Center:
                    return HorizontalAlignment.Center;
                case ImagePosition.BottomCenter:
                    return HorizontalAlignment.Center;
                default:
                    return HorizontalAlignment.Center;
            }
        }

        /// <summary>
        /// Applies text watermark to image with automatic scaling and positioning.
        /// </summary>
        private IImageProcessingContext ApplyScalingWaterMarkText(IImageProcessingContext processingContext, TextWatermark watermark)
        {
            Size imgSize = processingContext.GetCurrentSize();
            FontRectangle size = TextMeasurer.MeasureSize(watermark.Text, new TextOptions(watermark.Font));

            // Find out how much we need to scale the text to fill the space (up or down)
            float scalingFactor = Math.Min(imgSize.Width / size.Width, imgSize.Height / size.Height);

            // Create a new font
            SixLabors.Fonts.Font scaledFont = new SixLabors.Fonts.Font(watermark.Font, scalingFactor / 16 * (watermark.Font.Size * watermark.Layout.Scale));

            //If set, apply backround color
            if (watermark.Style.Color != null)
                processingContext.BackgroundColor((Color)watermark.Style.Color);

            var textOptions = new RichTextOptions(scaledFont)
            {
                ColorFontSupport = ColorFontSupport.MicrosoftColrFormat,
                Origin = CalcWatermarkOrigin(imgSize.Width, imgSize.Height, scaledFont.Size, watermark.Layout.Position),
                HorizontalAlignment = HorizontalAlignmentFromPosition(watermark.Layout.Position),
                VerticalAlignment = VerticalAlignment.Top,
            };

            if (watermark.Style.Pave)
            {
                foreach (ImagePosition position in (ImagePosition[])Enum.GetValues(typeof(ImagePosition)))
                {
                    textOptions.Origin = CalcWatermarkOrigin(imgSize.Width, imgSize.Height, scaledFont.Size, position);
                    textOptions.HorizontalAlignment = HorizontalAlignmentFromPosition(position);
                    processingContext.DrawText(textOptions, watermark.Text, watermark.Style.Color);
                }
                return processingContext;
            }
            return processingContext
                .DrawText(textOptions, watermark.Text, watermark.Style.Color);
        }

        /// <summary>
        /// Applies image watermark to target image with scaling and positioning.
        /// </summary>
        private IImageProcessingContext ApplyScalingWaterMarkImage(IImageProcessingContext processingContext, ImageWatermark watermark, Image watermarkImage, Image targetImage)
        {
            var scaleFactor = 1f;
            if (targetImage.Width > targetImage.Height)
                scaleFactor = (float)targetImage.Width / targetImage.Height;
            else
                scaleFactor = (float)targetImage.Height / targetImage.Width;

            var wmPaddingX = (targetImage.Width - watermarkImage.Width) / 2;
            var wmPaddingY = (targetImage.Height - watermarkImage.Height) / 2;
            var minWmPadding = 50;
            var scaledWmWidth = wmPaddingX > minWmPadding && wmPaddingY > minWmPadding
                ? watermarkImage.Width * scaleFactor
                : watermarkImage.Width / scaleFactor;
            var scaledWmHeight = wmPaddingX > minWmPadding && wmPaddingY > minWmPadding
                   ? watermarkImage.Height * scaleFactor
                   : watermarkImage.Height / scaleFactor;

            //If set, apply backround color
            if (watermark.Style.Color != null)
                processingContext.BackgroundColor((Color)watermark.Style.Color);

            watermarkImage.Mutate(x => x.Resize(new Size((int)scaledWmWidth, (int)scaledWmHeight)));
            var wmPositionOrigin = CalcWatermarkOrigin(targetImage.Width, targetImage.Height, watermarkImage.Width, watermarkImage.Height, watermark.Layout.Position);

            if (watermark.Style.Pave)
            {
                foreach (ImagePosition position in (ImagePosition[])Enum.GetValues(typeof(ImagePosition)))
                {
                    wmPositionOrigin = CalcWatermarkOrigin(targetImage.Width, targetImage.Height, watermarkImage.Width, watermarkImage.Height, position);
                    try
                    {
                        processingContext.DrawImage(watermarkImage, wmPositionOrigin, watermark.Style.Opacity);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                return processingContext;
            }

            return processingContext.DrawImage(watermarkImage, wmPositionOrigin, watermark.Style.Opacity);
        }
    }
}
