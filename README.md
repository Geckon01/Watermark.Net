# Watermark.Net
![NuGet Version](https://img.shields.io/nuget/v/Watermark.Net?style=for-the-badge)
![NuGet Downloads](https://img.shields.io/nuget/dt/Watermark.Net?style=for-the-badge&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FWatermark.Net%2F)
![Lecense](https://img.shields.io/badge/license-MIT-green?style=for-the-badge)
![GitHub last commit](https://img.shields.io/github/last-commit/Geckon01/Watermark.Net?display_timestamp=author&style=for-the-badge)

Watermark.Net is open source .NET library for adding text and image watermarks to images. Built on SixLabors.ImageSharp, it provides a simple yet comprehensive API for all your watermarking needs.

### Features
 - 🖼️ Multi-format support - Works with JPEG, PNG, BMP, GIF
 - ✏️ Text watermarks - Custom fonts, colors, sizes, rotations, and positioning
 - 🖌️ Image watermarks - PNG transparency support, scaling, and opacity control
 - 🧩 Positioning - 9 preset positions
 - 🧱 Pave mode - Tile watermarks across entire image
 - 📁 Batch processing - Process entire directories with single method call

# Quick Start

### Add Text Watermark
```csharp
var watermarker = new Watermarker("output");
var textWatermark = new TextWatermark
{
    Text = "WATERMARK",
    Font = SystemFonts.CreateFont("Arial", 36),
    Color = Color.White,
    Position = ImagePosition.BottomRight,
    Scale = 0.5f
};

var result = watermarker.ProcessImage("input.jpg", textWatermark); // Saves to output/input.jpg
```
### Add Image Watermark
```csharp
var watermarker = new Watermarker("output");
var imageWatermark = new ImageWatermark("logo.png")
{
    Opacity = 0.7f,
    Scale = 0.3f,
    Position = ImagePosition.Center,
    Pave = true
};

var results = watermarker.ProcessDirectory("images", imageWatermark);
```
# Contributing
Contributions are welcome! Just make sure that all tests pass before open PR.
# License
Watermark.Net is licensed under the MIT License.
