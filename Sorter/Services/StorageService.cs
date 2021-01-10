using Sorter.Repositories;

namespace Sorter.Services
{
  public interface IStorageService
  {
    void Save(int[] numberSequence, string filePath);
    int[] Load(string filePath);
  }

  public class StorageService : IStorageService
  {
    private readonly IStorageRepository StorageRepository;

    public StorageService(IStorageRepository storageRepository)
    {
      StorageRepository = storageRepository;
    }

    public void Save(int[] numberSequence, string filePath)
    {
      StorageRepository.Save(numberSequence, filePath);
    }

    public int[] Load(string filePath) => StorageRepository.Load(filePath);
  }
}
