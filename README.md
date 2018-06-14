# XamarinAssetGenerator
Converts image resources from svg to png and scales the images to desired size


Common usage:
mono AssetGenerator.exe -d destinationfolder -m iOS -q 80 -s sourcefolder

  -m, --mode           Required. Specify which assets to build. ios or android

  -s, --source         Specify source folder

  -d, --destination    Specify destination folder

  -q, --quality        (Default: 80) Specify quality of rendered png files (100 is max)

  --help               Display this help screen.

  --version            Display version information.
