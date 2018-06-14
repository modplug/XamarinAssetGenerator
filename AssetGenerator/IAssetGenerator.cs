using System.Threading.Tasks;

namespace AssetGenerator
{
    public interface IAssetGenerator
    {
        Task CreateAsset(string filepath, string filename, string destinationDirectory, int quality);
    }
}