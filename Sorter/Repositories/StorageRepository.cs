using Sorter.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sorter.Repositories
{
  public interface IStorageRepository
  {
    void Save(int[] numberSequence, string filePath);
    int[] Load(string filePath);
  }

  public class StorageRepository : IStorageRepository
  {
    private const char Separator = ' ';

    private readonly IFileHelper FileHelper;

    public StorageRepository(IFileHelper fileHelper)
    {
      FileHelper = fileHelper;
    }

    public void Save(int[] numberSequence, string filePath)
    {
      using (var stream = FileHelper.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
      using (var writer = new StreamWriter(stream))
      {
        for (var index = 0; index < numberSequence.Length; index++)
        {
          var number = numberSequence[index];
          writer.Write(number);

          if (index != numberSequence.Length - 1)
            writer.Write(Separator);
        }
        writer.Flush();
      }
    }

    public int[] Load(string filePath)
    {
      var numberSequence = new List<int>();

      using (var stream = FileHelper.OpenRead(filePath))
      using (var reader = new StreamReader(stream))
      {
        var numberStrings = reader.ReadToEnd().Split(Separator);

        foreach (var numberString in numberStrings)
        {
          if (!int.TryParse(numberString, out var number))
            throw new Exception($"'{numberString}' couldn't be parsed to a valid number.");

          numberSequence.Add(number);
        }
      }
      return numberSequence.ToArray();
    }
  }
}
