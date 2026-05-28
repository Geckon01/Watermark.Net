# Watermark.Net

[![.NET](https://github.com/Geckon01/Watermark.Net/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Geckon01/Watermark.Net/actions/workflows/dotnet.yml)
![NuGet Version](https://img.shields.io/nuget/v/Watermark.Net)
![NuGet Downloads](https://img.shields.io/nuget/dt/Watermark.Net?link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FWatermark.Net%2F)
![Lecense](https://img.shields.io/badge/license-MIT-green)
![GitHub last commit](https://img.shields.io/github/last-commit/Geckon01/Watermark.Net?display_timestamp=author)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/e6340e249ad743bc99c1745aaa0a9838)](https://app.codacy.com/gh/Geckon01/Watermark.Net/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)

**Watermark.Net** — современная кроссплатформенная библиотека для .NET, предназначенная для добавления текстовых и графических watermark’ов к изображениям с использованием C#.

Библиотека построена на базе SixLabors.ImageSharp и предоставляет чистый и расширяемый API для:

- добавления watermark’ов к изображениям в .NET
- пакетной обработки изображений
- защиты авторских прав
- наложения логотипов
- автоматизации брендинга
- генерации draft/staging изображений

Подходит для ASP.NET приложений, media pipeline’ов, SaaS-платформ, desktop-приложений, automation scripts и backend image processing сервисов.

---

# Почему Watermark.Net?

- ✅ Современная архитектура на .NET 8+
- ✅ Полная кроссплатформенность (Windows, Linux, macOS)
- ✅ Основана на ImageSharp
- ✅ Простой и чистый API
- ✅ Пакетная обработка директорий
- ✅ Поддержка текстовых и графических watermark’ов
- ✅ Режим tiled/repeated watermark
- ✅ Поддержка Dependency Injection
- ✅ Open-source и лицензия MIT

---

# Возможности

## Текстовые watermark’и

Поддерживается:

- пользовательские шрифты
- настройка размера текста
- поворот
- прозрачность
- отступы
- система позиционирования 3x3
- tiled/pattern режим

## Графические watermark’и

Наложение логотипов и PNG:

- поддержка прозрачности PNG
- настройка opacity
- масштабирование
- позиционирование
- tiled/repeated режим

## Пакетная обработка

Обработка целых папок изображений одним вызовом метода.

Поддерживаемые форматы:

- JPEG
- PNG
- BMP
- GIF

---

# Установка

Установка через NuGet:

```bash
dotnet add package Watermark.Net
```

Или через Package Manager:

```powershell
Install-Package Watermark.Net
```

Требования:

- .NET 8 или новее

---

# Быстрый старт

## Добавление текстового watermark

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

## Добавление графического watermark

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

# Типичные сценарии использования

Watermark.Net часто используется для:

- защиты фотографий watermark’ами
- брендирования изображений интернет-магазинов
- SaaS image pipeline’ов
- маркировки AI-generated изображений
- CMS систем
- автоматизации соцсетей
- backend image processing сервисов
- ASP.NET image service’ов
- массовой обработки изображений
- внутренних документов и черновиков

---

# Архитектура

Watermark.Net использует lightweight pipeline architecture с чётким разделением ответственности.

```text
┌──────────────────────────────────────┐
│          WatermarkPipeline           │
│       Основной orchestration layer   │
├──────────────────────────────────────┤
│      IFileManager / FileManager      │
│       Абстракция файловой системы    │
├──────────────────────────────────────┤
│           ImageRenderer              │
│       Pure rendering engine          │
└──────────────────────────────────────┘
```

---

# Типы watermark’ов

| Тип | Описание |
|---|---|
| `TextWatermark` | Конфигурация текстового watermark |
| `ImageWatermark` | Конфигурация watermark-изображения |
| `ResultImage` | Модель результата обработки |
| `IWatermarkDefinition` | Базовая абстракция watermark |

---

# Система позиционирования

Поддерживается 9 встроенных позиций:

- TopLeft
- TopCenter
- TopRight
- CenterLeft
- Center
- CenterRight
- BottomLeft
- BottomCenter
- BottomRight

---

# Продвинутое использование

## Tiled / Pave режим

Повторяет watermark по всей поверхности изображения:

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

# Contributing

Pull Request’ы и contributions приветствуются.

Для участия:

1. Сделайте fork репозитория
2. Создайте feature branch
3. Добавьте тесты
4. Запустите test suite
5. Откройте Pull Request

---

# License

Проект распространяется под лицензией MIT.