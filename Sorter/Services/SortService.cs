namespace Sorter.Services
{
  public interface ISortService
  {
    void SaveSorted(int[] numberSequence);
    int[] LoadLatest();
  }

  public class SortService : ISortService
  {
    private const string LatestSequenceFilePath = "result.txt";

    private readonly IStorageService StorageService;

    public SortService(IStorageService storageService)
    {
      StorageService = storageService;
    }

    public void SaveSorted(int[] numberSequence)
    {
      Sort(numberSequence);
      StorageService.Save(numberSequence, LatestSequenceFilePath);
    }

    public int[] LoadLatest() => StorageService.Load(LatestSequenceFilePath);

    private static void Sort(int[] numberSequence)
    {
      // Selection Sort:
      // TODO: Use algorithm which at worst case has O(N*log(N)) complexity
      for (var i = 0; i < numberSequence.Length; i++)
        for (var j = i + 1; j < numberSequence.Length; j++)
          if (numberSequence[i] > numberSequence[j])
            Swap(ref numberSequence[i], ref numberSequence[j]);
    }

    private static void Swap(ref int firstNumber, ref int secondNumber)
    {
      var temp = firstNumber;
      firstNumber = secondNumber;
      secondNumber = temp;
    }
  }
}
