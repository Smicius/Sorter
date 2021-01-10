using System.IO;

namespace Sorter.UnitTests.TestHelpers
{
  public static class StreamHelper
  {
    public static Stream Create(string content)
    {
      var stream = new MemoryStream();
      using (var writer = new StreamWriter(stream, null, -1, true))
      {
        writer.Write(content);
        writer.Flush();
      }
      stream.Position = 0;
      return stream;
    }

    public static string ToString(Stream stream)
    {
      stream.Position = 0;
      using (var reader = new StreamReader(stream))
      {
        return reader.ReadToEnd();
      }
    }
  }
}
