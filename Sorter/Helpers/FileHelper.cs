using System.IO;

namespace Sorter.Helpers
{
  public interface IFileHelper
  {
    bool Exists(string path);
    Stream Open(string path, FileMode mode, FileAccess access, FileShare share);
    Stream OpenRead(string path);
  }

  public class FileHelper : IFileHelper
  {
    public bool Exists(string path) => File.Exists(path);

    public Stream Open(string path, FileMode mode, FileAccess access, FileShare share) => File.Open(path, mode, access, share);

    public Stream OpenRead(string path) => File.OpenRead(path);
  }
}
