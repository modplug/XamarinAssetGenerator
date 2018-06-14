using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace AssetGenerator
{
    public class Options
    {
        [Option('m', "mode", HelpText = "Specify which assets to build. ios or android", Required = true)]
        public string Mode { get; set; }

        [Option('s', "source", Separator = '-', HelpText = "Specify source folder", Required = false)]
        public string SourceFolderPath { get; set; }

        [Option('d', "destination", Separator = '-', HelpText = "Specify destination folder", Required = false)]
        public string DestinationFolderPath { get; set; }

        [Option('q', "quality", Separator = '-', HelpText = "Specify quality of rendered png files (100 is max)", Required = false, Default = 80)]
        public int Quality { get; set; }

        [Usage(ApplicationAlias = "AssetGenerator")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Common usage",
                    new Options
                    {
                        Mode = "iOS",
                        SourceFolderPath = "sourcefolder",
                        DestinationFolderPath = "destinationfolder",
                        Quality = 80
                    });
            }
        }
    }
}