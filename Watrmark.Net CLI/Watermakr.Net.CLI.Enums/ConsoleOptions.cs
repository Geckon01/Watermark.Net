using Watrmark.Net_CLI.Watermark.Net.CLI.Models;
using Watermark.Net.src.WatermarkNet.Enums;
using CommandLine;

namespace Watrmark.Net_CLI.Watermakr.Net.CLI.Enums
{
    internal class ConsoleOptions
    {
        [Option("type", Required = true, HelpText = "")]
        public WatermarkType WatermarkType { get; set; }

        [Option('f', "file", Required = false, HelpText = "")]
        public string? FilePath { get; set; }

        [Option('d', "directory", Group = "wmoptions", HelpText = "")]
        public string? DirectoryPath { get; set; }

        [Option('w', "watermark", Group = "wmoptions", HelpText = "")]
        public string? WatermarkPath { get; set; }

        [Option('o', "output", Required = true, HelpText = "")]
        public string? OutputPath { get; set; }

        [Option("text", Group = "wmoptions", HelpText = "")]
        public string? WatermarkText { get; set; }

        [Option('c', "color", Required = false, HelpText = "")]
        public SixLabors.ImageSharp.Color? WatermarkColor { get; set; }

        [Option('b', "wmbackroud", Required = false, HelpText = "")]
        public SixLabors.ImageSharp.Color? WatermarkBackround { get; set; }

        [Option('s', "scale", Required = false, HelpText = "")]
        public float? WatermarkScale { get; set; }

        [Option('p', "position", Required = false, HelpText = "")]
        public ImagePosition? WatermarkPositon { get; set; }

        [Option("threads", Required = false, HelpText = "")]
        public int? ThreadsNumber { get; set; }
    }
}
