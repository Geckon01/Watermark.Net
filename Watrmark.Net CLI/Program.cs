using CommandLine;
using Watermark.Net.src.WatermarkNet.Types;
using System.Diagnostics;
using Watermark.Net.src.WatermarkNet.Core;
using Watrmark.Net_CLI.Watermakr.Net.CLI.Enums;
using Watrmark.Net_CLI.Watermark.Net.CLI.Models;
using Watrmark.Net_CLI.Watermak.Net.CLI.Constants;
using Watrmark.Net_CLI;
using System.Runtime.InteropServices;

//The CLI version is not fully ready yet. To be implemented in future

Parser.Default.ParseArguments<ConsoleOptions>(args)
  .WithParsed<ConsoleOptions>(option => {
      switch (option.WatermarkType)
      {
          case WatermarkType.Image:
              if (option.FilePath != null)
                  ProccessSingleFile(option);
              if (option.DirectoryPath != null)
                  ProccessDirectory(option);
              break;
          case WatermarkType.Text:
              if (option.FilePath != null)
                  ProccessSingleText(option);
              else
                  ProccessDirectoryText(option);
              break;
          default:
              throw new ArgumentException("Watermark type not provided.");
      }

  })
  .WithNotParsed(error => { });

static void ProccessSingleFile(ConsoleOptions options)
{

}
static void ProccessDirectory(ConsoleOptions options)
{

}

static void ProccessSingleText(ConsoleOptions options)
{

}
static void ProccessDirectoryText(ConsoleOptions options)
{
    if (options.WatermarkText == null || options.WatermarkText == string.Empty)
        throw new ArgumentNullException("Watermark text can not be null");
    if(options.DirectoryPath == null || !Directory.Exists(options.DirectoryPath))
        throw new ArgumentNullException("Specified files directory not found");
    if (options.OutputPath == null || !Directory.Exists(options.OutputPath))
        throw new ArgumentNullException("Specified output directory not found");

    var directoryFiles = Directory.GetFiles(options.DirectoryPath);

    var filesTotal = directoryFiles.Length;
    var chunkSize = filesTotal / (options.ThreadsNumber ?? Environment.ProcessorCount);
    var filesChunks = directoryFiles.ToList().Chunk(chunkSize < 1 ? 1: chunkSize);
    var filesComplite = 0;

    var watermark = new TextWatermark{
        Text = options.WatermarkText,
        Color = options.WatermarkColor ?? Constans.DefaultTextColor,
        Position = options.WatermarkPositon ?? Constans.DefaultWatermarkPosition,
        BackroundColor = options.WatermarkBackround ?? Constans.DefaultBackroundColor,
        Font = Constans.DefaultWatermarkFont,
        Scale = options.WatermarkScale ?? Constans.DefaultWatermarkScale
    };
    var watermarker = new Watermarker();

    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    Console.Clear();

    Parallel.ForEach(filesChunks, new ParallelOptions { MaxDegreeOfParallelism = options.ThreadsNumber ?? Environment.ProcessorCount }, chunk =>
    {
        foreach (var imagePath in chunk)
        {
            var resultedImage = watermarker.ProcessImage(imagePath, options.OutputPath, watermark);

            lock (stopwatch)
            {
                Extensions.DrawStats(resultedImage.Path, ++filesComplite, filesTotal, stopwatch);
            }
        }
    });

    stopwatch.Stop();
    Extensions.DrawCompliteStats(stopwatch.Elapsed);
}