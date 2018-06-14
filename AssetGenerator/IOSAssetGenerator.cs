using System;
using System.IO;
using System.Threading.Tasks;
using SkiaSharp.Extended.Svg;

namespace AssetGenerator
{
    public class IOSAssetGenerator : IAssetGenerator
    {
        public async Task CreateAsset(string filepath, string filename, string destinationDirectory, int quality)
        {
            for (var i = 1; i < 4; i++)
            {
                try
                {
                    var svg = new SKSvg();
                    try
                    {
                        svg.Load(filepath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Unexpected error when parsing asset: {filepath}");
                        Console.WriteLine("Error: " + e.Message);
                        Console.WriteLine("Exiting with error 1");
                        Environment.Exit(1);
                    }

                    string newFilename;
                    if (i == 1)
                        newFilename = filename + ".png";
                    else
                        newFilename = filename + $"@{i}x.png";

                    var width = (int) (svg.CanvasSize.Width * i);
                    var height = (int) (svg.CanvasSize.Height * i);
                    await PngHelper.GeneratePng(width, height, filepath, Path.Combine(destinationDirectory, newFilename), quality);
                    Console.WriteLine($"Successfully created asset: {Path.GetFileName(newFilename)}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to generate asset: {filename}");
                    Console.WriteLine("Error: " + e.Message);
                    Console.WriteLine("Exiting with error 1");
                    Environment.Exit(1);
                }
            }
        }
    }
}