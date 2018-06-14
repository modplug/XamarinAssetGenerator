using System.IO;
using System.Threading.Tasks;
using SkiaSharp;
using SKSvg = SkiaSharp.Extended.Svg.SKSvg;

namespace AssetGenerator
{
    public class PngHelper
    {
        public static async Task GeneratePng(int width, int height, string filepath, string filename, int quality)
        {
            var svg2 = new SKSvg(new SKSize(width, height));
            svg2.Load(filepath);

            using (var image = SKImage.FromPicture(svg2.Picture, new SKSizeI(width, height)))
            {
                using (var data = image.Encode(SKEncodedImageFormat.Png, quality))
                {
                    // save the data to a stream
                    using (var stream = File.OpenWrite(filename))
                    {
                        data.SaveTo(stream);
                        await stream.FlushAsync();
                    }
                }
            }
        }
    }
}