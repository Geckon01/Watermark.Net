using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watrmark.Net_CLI
{
    internal class Extensions
    {
        private static void ClearRow(int row)
        {
            Console.SetCursorPosition(0, row);
            Console.Write(new String(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, row);
        }

        private static void DrawTextProgressBar(double percentComplite)
        {
            int totalChunks = Console.WindowWidth / 2;

            //draw empty progress bar
            Console.CursorLeft = 0;
            Console.Write("["); //start
            Console.CursorLeft = totalChunks + 1;
            Console.Write("]"); //end
            Console.CursorLeft = 1;

            int numChunksComplete = Convert.ToInt16(totalChunks * percentComplite);

            //draw completed chunks
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("".PadRight(numChunksComplete));

            //draw incomplete chunks
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("".PadRight(totalChunks - numChunksComplete));

            //draw totals
            Console.CursorLeft = totalChunks + 5;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public static void DrawStats(string imagePath, int filesComplite, int filesTotal, Stopwatch stopwatch)
        {
            var complitePercent = Convert.ToDouble(filesComplite) / filesTotal;
            var operationsPerSecond = Convert.ToDouble(filesComplite) / stopwatch.Elapsed.TotalSeconds;

            Console.CursorVisible = false;
            ClearRow(0);
            Console.WriteLine($"Processed file: {imagePath}");
            DrawTextProgressBar(complitePercent);
            Console.Write($"{Math.Round(complitePercent * 100, 0)}% \t");
            Console.WriteLine($"{filesComplite} of {filesTotal}");
            ClearRow(2);
            Console.WriteLine($"{Math.Round(operationsPerSecond, 0)} per second");
        }

        public static void DrawCompliteStats(TimeSpan elapsedTime)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Work complite in {elapsedTime.Minutes} min. {elapsedTime.TotalSeconds} sec.");
            Console.ResetColor();
        }
    }
}
