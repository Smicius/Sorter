using System.IO;

namespace Sorter.Helpers
{
  public interface IFileHelper
  {
    Stream Open(string path, FileMode mode, FileAccess access, FileShare share);
    Stream OpenRead(string path);
  }

  public class FileHelper : IFileHelper
  {
    public Stream Open(string path, FileMode mode, FileAccess access, FileShare share) => File.Open(path, mode, access, share);

    public Stream OpenRead(string path) => File.OpenRead(path);
  }
}
