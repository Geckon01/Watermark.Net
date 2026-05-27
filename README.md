# Watermark.Net

[![.NET](https://github.com/Geckon01/Watermark.Net/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Geckon01/Watermark.Net/actions/workflows/dotnet.yml)
![NuGet Version](https://img.shields.io/nuget/v/Watermark.Net)
![NuGet Downloads](https://img.shields.io/nuget/dt/Watermark.Net?link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FWatermark.Net%2F)
![License](https://img.shields.io/badge/license-MIT-green)
![GitHub last commit](https://img.shields.io/github/last-commit/Geckon01/Watermark.Net?display_timestamp=author)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/e6340e249ad743bc99c1745aaa0a9838)](https://app.codacy.com/gh/Geckon01/Watermark.Net/dashboard)

**Watermark.Net** is a modern, cross-platform **.NET image watermarking library** for adding **text and image watermarks** to photos and graphics using C#.

Built on top of [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp), Watermark.Net provides a clean and extensible API for:

- image watermarking in .NET
- batch image processing
- copyright protection
- logo overlays
- branding automation
- draft/staging image generation

Perfect for ASP.NET applications, media pipelines, SaaS platforms, desktop tools, automation scripts, and backend image processing services.

---

## Why Watermark.Net?

- ✅ Modern **.NET 8+** architecture
- ✅ Fully **cross-platform** (Windows, Linux, macOS)
- ✅ Powered by **ImageSharp**
- ✅ Simple and clean API
- ✅ Batch directory watermarking
- ✅ Text and image watermark support
- ✅ Tiling / repeated watermark mode
- ✅ Dependency Injection friendly
- ✅ Open-source and MIT licensed

---

# Features

## Text Watermarks

Add fully customizable text overlays:

- Custom fonts
- Font sizing
- Rotation
- Opacity
- Padding
- 9-position alignment system
- Tiled/pattern mode

## Image Watermarks

Overlay logos or transparent PNG files:

- PNG transparency support
- Opacity control
- Scaling
- Positioning
- Repeated/tiled mode

## Batch Processing

Process entire folders of images with a single method call.

Supports:

- JPEG
- PNG
- BMP
- GIF

---

# Installation

Install from NuGet:

```bash
dotnet add package Watermark.Net
```

Or via Package Manager:

```powershell
Install-Package Watermark.Net
```

NuGet package:

https://www.nuget.org/packages/Watermark.Net/

Requirements:

- .NET 8 or later

---

# Quick Start

## Add a Text Watermark

```csharp
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using WatermarkNet.Core;
using WatermarkNet.Enums;
using WatermarkNet.Models.Definitions;

var pipeline = new WatermarkPipeline(new FileManager(), new ImageRenderer());

var watermark = new TextWatermark
{
    Text = "CONFIDENTIAL",
    Font = SystemFonts.CreateFont("Arial", 36),

    Layout =
    {
        Position = ImagePosition.BottomRight,
        Scale = 0.5f
    },

    Style =
    {
        Color = Color.White,
        Opacity = 0.8f
    }
};

ResultImage result = pipeline.ProcessImage(
    "input.jpg",
    "output",
    watermark
);

Console.WriteLine($"Saved to: {result.Path}");
```

---

## Add an Image Watermark

```csharp
using SixLabors.ImageSharp;
using WatermarkNet.Core;
using WatermarkNet.Enums;
using WatermarkNet.Models.Definitions;

var pipeline = new WatermarkPipeline(new FileManager(), new ImageRenderer());

var watermark = new ImageWatermark
{
    ImagePath = "logo.png",

    Layout =
    {
        Position = ImagePosition.Center,
        Scale = 0.3f
    },

    Style =
    {
        Opacity = 0.7f,
        Pave = true
    }
};

List<ResultImage> results = pipeline.ProcessDirectory(
    "images",
    "output",
    watermark
);

Console.WriteLine($"Processed {results.Count} images");
```

---

# Common Use Cases

Watermark.Net is commonly used for:

- Photo copyright protection
- E-commerce product branding
- SaaS image pipelines
- AI-generated image labeling
- CMS systems
- Social media automation
- Media processing backends
- ASP.NET image services
- Bulk image processing
- Internal document marking

---

# Architecture

Watermark.Net follows a lightweight pipeline architecture with clear separation of responsibilities.

```text
┌──────────────────────────────────────┐
│          WatermarkPipeline           │
│      Main orchestration layer        │
├──────────────────────────────────────┤
│       IFileManager / FileManager     │
│         File system abstraction      │
├──────────────────────────────────────┤
│            ImageRenderer             │
│      Pure image rendering engine     │
└──────────────────────────────────────┘
```

## Components

### `WatermarkPipeline`

Main entry point responsible for:

- file handling
- pipeline orchestration
- watermark processing

### `ImageRenderer`

Pure rendering engine:

- no filesystem dependency
- in-memory image processing
- reusable rendering logic

### `IFileManager`

Abstraction layer for:

- local files
- cloud storage integration
- testing/mocking

---

# Watermark Types

| Type | Description |
|---|---|
| `TextWatermark` | Text-based watermark configuration |
| `ImageWatermark` | Image/logo watermark configuration |
| `ResultImage` | Processing result model |
| `IWatermarkDefinition` | Base watermark abstraction |

---

# Positioning System

Supports 9 built-in alignment positions:

| Position |
|---|
| TopLeft |
| TopCenter |
| TopRight |
| CenterLeft |
| Center |
| CenterRight |
| BottomLeft |
| BottomCenter |
| BottomRight |

---

# Advanced Usage

## Tiled / Pave Mode

Repeat watermark across the entire image:

```csharp
var watermark = new TextWatermark
{
    Text = "DRAFT",
    Font = SystemFonts.CreateFont("Arial", 36),

    Style =
    {
        Color = Color.Red,
        Pave = true
    },

    Layout =
    {
        Scale = 0.3f
    }
};
```

---

## Rotation & Padding

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

---

## Dependency Injection

```csharp
services.AddSingleton<IFileManager, FileManager>();

services.AddSingleton<ImageRenderer>();

services.AddTransient<WatermarkPipeline>();
```

---

## Batch Directory Processing

```csharp
var results = pipeline.ProcessDirectory(
    "input_photos",
    "watermarked",
    textWatermark
);

foreach (var result in results)
{
    Console.WriteLine($"✓ {result.Path}");
}
```

---

# Roadmap

Planned improvements:

- Async API support
- Stream-based processing
- SVG watermark support
- Better tiling engine
- Additional blend modes
- Performance optimizations

---

# Contributing

Contributions are welcome.

To contribute:

1. Fork the repository
2. Create a feature branch
3. Add or update tests
4. Run the test suite
5. Open a Pull Request

Test assets are located in:

```text
UnitTest/TestImages/
```

---

# License

Watermark.Net is licensed under the MIT License.

See the [LICENSE](LICENSE) file for details.

---

# Keywords

.NET watermark library, C# watermark library, ImageSharp watermarking, image watermarking .NET, text watermark C#, image overlay library, batch image processing, ASP.NET image watermarking, cross-platform image processing, watermark NuGet package
