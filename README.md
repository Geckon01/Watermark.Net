# Watermark.Net
[![.NET](https://github.com/Geckon01/Watermark.Net/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Geckon01/Watermark.Net/actions/workflows/dotnet.yml)
![NuGet Version](https://img.shields.io/nuget/v/Watermark.Net)
![NuGet Downloads](https://img.shields.io/nuget/dt/Watermark.Net?link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FWatermark.Net%2F)
![Lecense](https://img.shields.io/badge/license-MIT-green)
![GitHub last commit](https://img.shields.io/github/last-commit/Geckon01/Watermark.Net?display_timestamp=author)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/e6340e249ad743bc99c1745aaa0a9838)](https://app.codacy.com/gh/Geckon01/Watermark.Net/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)

**Watermark.Net** is an open-source .NET library for programmatically adding text and image watermarks to images. Built on [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp), it provides a clean, extensible API for all your watermarking needs — from a single file to entire directory batches. Whether you need to protect copyrights, brand your media, or mark drafts, Watermark.Net makes image watermarking in C# simple and efficient.

### Features

- 🖼️ **Multi-format support** — JPEG, PNG, BMP, GIF
- ✏️ **Text watermarks** — custom fonts, colors, sizes, rotation, and 9-zone positioning
- 🖌️ **Image watermarks** — PNG transparency, scaling, and opacity control (0.0–1.0)
- 🧱 **Pave (tiling) mode** — repeat the watermark across the entire image surface
- 🧩 **9 preset positions** — TopLeft, TopCenter, TopRight, CenterLeft, Center, CenterRight, BottomLeft, BottomCenter, BottomRight
- 🎨 **Custom styling** — background color, opacity, rotation angle, and padding
- 📁 **Batch processing** — watermark all images in a directory with a single method call
- 🔌 **Dependency injection ready** — `IFileManager` abstraction for testing and custom file I/O
- 🔄 **Backward compatible** — legacy `Watermarker` API still supported (marked as `[Obsolete]`)

---

## Installation

Install the [NuGet package](https://www.nuget.org/packages/Watermark.Net/):

```bash
dotnet add package Watermark.Net
```

Requires **.NET 8** or later.

---

## Quick Start

### Add a Text Watermark

```csharp
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using Watermark.Net.src.WatermarkNet.Core;
using Watermark.Net.src.WatermarkNet.Enums;
using Watermark.Net.src.WatermarkNet.Models.Definitions;

var pipeline = new WatermarkPipeline(new FileManager(), new ImageRenderer());

var watermark = new TextWatermark
{
    Text = "WATERMARK",
    Font = SystemFonts.CreateFont("Arial", 36),
    Layout = { Position = ImagePosition.BottomRight, Scale = 0.5f },
    Style = { Color = Color.White }
};

ResultImage result = pipeline.ProcessImage("input.jpg", "output", watermark);
Console.WriteLine($"Saved to: {result.Path}");
```

### Add an Image Watermark

```csharp
using SixLabors.ImageSharp;
using Watermark.Net.src.WatermarkNet.Core;
using Watermark.Net.src.WatermarkNet.Enums;
using Watermark.Net.src.WatermarkNet.Models.Definitions;

var pipeline = new WatermarkPipeline(new FileManager(), new ImageRenderer());

var watermark = new ImageWatermark
{
    ImagePath = "logo.png",
    Layout = { Scale = 0.3f, Position = ImagePosition.Center },
    Style = { Opacity = 0.7f, Pave = true }
};

List<ResultImage> results = pipeline.ProcessDirectory("images", "output", watermark);
Console.WriteLine($"Processed {results.Count} images");
```

---

## Architecture

Watermark.Net follows a clean **pipeline architecture** with clear separation of concerns:

```
┌──────────────────────────────────────┐
│           WatermarkPipeline          │  ← Orchestrator layer
│  (coordinates I/O and rendering)     │
├──────────────────────────────────────┤
│  IFileManager / FileManager          │  ← File I/O abstraction
│  (loading, saving, enumeration)      │
├──────────────────────────────────────┤
│           ImageRenderer              │  ← Pure rendering engine
│  (image processing, no I/O)          │
└──────────────────────────────────────┘
```

- **[`WatermarkPipeline`](Watermark.Net/src/WatermarkNet.Common/WatermarkPipeline.cs)** — the main entry point that orchestrates file operations and rendering.
- **[`IFileManager`](Watermark.Net/src/WatermarkNet.Common/IFileManager.cs)** / **[`FileManager`](Watermark.Net/src/WatermarkNet.Common/FileManager.cs)** — abstracts all file system I/O. Swap this for custom storage or unit testing.
- **[`ImageRenderer`](Watermark.Net/src/WatermarkNet.Common/ImageRenderer.cs)** — a pure image processing engine. Accepts in-memory `Image` objects, returns processed `Image` instances. No file I/O dependency.

---

## Configuration Reference

### [`WatermarkLayout`](Watermark.Net/src/WatermarkNet.Models/Layout/WatermarkLayout.cs)

Controls *where* and *how big* the watermark appears.

| Property     | Type           | Default  | Description                                |
|--------------|----------------|----------|--------------------------------------------|
| `Position`   | `ImagePosition`| `TopLeft`| One of 9 predefined positions              |
| `Scale`      | `float`        | `0`      | Scale factor relative to image size        |
| `RotateAngle`| `int`          | `0`      | Rotation angle in degrees                  |
| `Padding`    | `float`        | `0`      | Padding space around the watermark (px)    |

### [`WatermarkStyle`](Watermark.Net/src/WatermarkNet.Models/Styling/WatermarkStyle.cs)

Controls *how* the watermark looks.

| Property  | Type    | Default  | Description                                    |
|-----------|---------|----------|------------------------------------------------|
| `Opacity` | `float` | `0`      | Transparency level — `0.0` (transparent) to `1.0` (opaque) |
| `Pave`    | `bool`  | `false`  | When `true`, tiles the watermark across the image |
| `Color`   | `Color` | `default`| Background or text color                       |

### [`ImagePosition`](Watermark.Net/src/WatermarkNet.Enums/ImagePosition.cs)

All 9 available positions:

| Name           | Description        |
|----------------|--------------------|
| `TopLeft`      | Top-left corner    |
| `TopCenter`    | Top-center         |
| `TopRight`     | Top-right corner   |
| `CenterLeft`   | Middle-left        |
| `Center`       | Exact center       |
| `CenterRight`  | Middle-right       |
| `BottomLeft`   | Bottom-left corner |
| `BottomCenter` | Bottom-center      |
| `BottomRight`  | Bottom-right corner|

### Model types

| Type                                              | Description                                    |
|---------------------------------------------------|------------------------------------------------|
| [`TextWatermark`](Watermark.Net/src/WatermarkNet.Models/Definitions/TextWatermark.cs)   | Text watermark configuration (`Text`, `Font`)  |
| [`ImageWatermark`](Watermark.Net/src/WatermarkNet.Models/Definitions/ImageWatermark.cs) | Image watermark configuration (`ImagePath`)    |
| [`ResultImage`](Watermark.Net/src/WatermarkNet.Models/Definitions/ResultImage.cs)       | Output result (`.Image` + `.Path`)             |
| [`IWatermarkDefinition`](Watermark.Net/src/WatermarkNet.Models/Definitions/IWatermarkDefinition.cs) | Base interface for custom watermark definitions |

---

## Advanced Usage

### Pave (Tiling) Mode

Tile the watermark across every position on the image:

```csharp
var watermark = new TextWatermark
{
    Text = "DRAFT",
    Font = SystemFonts.CreateFont("Arial", 36),
    Style = { Color = Color.Red, Pave = true },
    Layout = { Scale = 0.3f }
};
```

### Opacity Control

Create a subtle, semi-transparent watermark:

```csharp
var watermark = new ImageWatermark
{
    ImagePath = "logo.png",
    Style = { Opacity = 0.3f },  // 30% opacity
    Layout = { Position = ImagePosition.Center, Scale = 0.5f }
};
```

### Custom Positioning & Rotation

Place a rotated watermark with padding:

```csharp
var watermark = new TextWatermark
{
    Text = "CONFIDENTIAL",
    Font = SystemFonts.CreateFont("Arial", 24),
    Layout =
    {
        Position = ImagePosition.TopLeft,
        RotateAngle = 45,
        Padding = 20
    }
};
```

### Dependency Injection

Integrate with your DI container for better testability:

```csharp
services.AddSingleton<IFileManager, FileManager>();
services.AddSingleton<ImageRenderer>();
services.AddTransient<WatermarkPipeline>();
```

### Batch Directory Processing

Process all images in a directory with a single call:

```csharp
var results = pipeline.ProcessDirectory("input_photos", "watermarked", textWatermark);

foreach (var result in results)
{
    Console.WriteLine($"✓ {result.Path}");
}
```

---

## Contributing

Contributions are welcome! To ensure a smooth collaboration:

1. **Fork** the repository and create a feature branch.
2. **Write tests** for any new functionality (see the [`UnitTest`](UnitTest/UnitTest.cs) project).
3. **Run all tests** before opening a pull request — make sure they pass.
4. **Open a PR** with a clear description of the changes.

All tests use sample images located in [`UnitTest/TestImages/`](UnitTest/TestImages/).

---

## License

Watermark.Net is licensed under the [MIT License](LICENSE).
