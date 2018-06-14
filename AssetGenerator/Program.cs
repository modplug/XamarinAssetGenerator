using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using SkiaSharp.Extended.Svg;

namespace AssetGenerator
{
    public class XamarinAssetGenerator
    {
        private const string Android = "Android";
        private const string iOS = "iOS";

        private static async Task<int> Main(string[] args)
        {
            var mode = "ios";
            var sourceDirectory = Directory.GetCurrentDirectory();
            var destinationDirectory = Directory.GetCurrentDirectory();
            var result = Parser.Default.ParseArguments<Options>(args);
            var quality = 80;

            result.WithParsed(options =>
                {
                    mode = options.Mode;
                    if (!string.IsNullOrEmpty(options.SourceFolderPath))
                    {
                        sourceDirectory = Path.Combine(sourceDirectory, options.SourceFolderPath);
                        if (!Directory.Exists(sourceDirectory))
                        {
                            Console.WriteLine("Source directory does not exist");
                            Environment.Exit(1);
                        }
                    }

                    if (!string.IsNullOrEmpty(options.DestinationFolderPath))
                    {
                        destinationDirectory = options.DestinationFolderPath;
                        if (!Directory.Exists(destinationDirectory))
                        {
                            Console.WriteLine("Destination directory does not exist");
                            Environment.Exit(1);
                        }
                    }

                    if (quality < 1 || quality > 100)
                    {
                        Console.WriteLine("Quality must be between 1..100");
                        Environment.Exit(1);
                    }
                    quality = options.Quality;
                })
                .WithNotParsed(errors =>
                {
                    foreach (var error in errors) Console.WriteLine("Error: " + error.Tag);

                    Environment.Exit(1);
                });

            var currentMode = mode == iOS.ToLowerInvariant() ? DeviceType.iOS : DeviceType.Android;
            var files = Directory.GetFiles(sourceDirectory, "*.svg");
            if (files.Length == 0)
            {
                Console.WriteLine("No .svg files found in directory");
                Environment.Exit(1);
            }

            await GenerateAssets(files, currentMode, destinationDirectory, quality);
            return await Task.FromResult(0);
        }

        private static async Task GenerateAssets(string[] files, DeviceType mode, string destinationDirectory,
            int quality)
        {
            // It fails to create the first file
            var firstItem = files[0];
            var initalLoad = new SKSvg();
            initalLoad.Load(firstItem);
            await PngHelper.GeneratePng((int) initalLoad.CanvasSize.Width, (int) initalLoad.CanvasSize.Height,
                firstItem, "temp", 1);
            File.Delete("temp");

            foreach (var filepath in files.OrderBy(s => s).ToList())
            {
                Console.WriteLine($"Creating assets from {filepath}");

                var filename = Path.GetFileNameWithoutExtension(filepath);
                if (mode == DeviceType.iOS)
                {
                    var generator = new IOSAssetGenerator();
                    await generator.CreateAsset(filepath, filename, destinationDirectory, quality);
                }
                else
                {
                    var generator = new AndroidAssetGenerator();
                    await generator.CreateAsset(filepath, filename, destinationDirectory, quality);
                }
            }
        }
    }
}