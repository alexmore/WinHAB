using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WinHAB.Desktop.IO
{
  public static class FileExtensions
  {
    public static async Task<string> ReadAllTextAsync(this FileInfo file)
    {
      using (var sourceStream = new FileStream(file.FullName,
        FileMode.Open, FileAccess.Read, FileShare.Read,
        bufferSize: 4096, useAsync: true))
      {
        var sb = new StringBuilder();

        var buffer = new byte[0x1000];
        int numRead;
        while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
        {
          string text = Encoding.Unicode.GetString(buffer, 0, numRead);
          sb.Append(text);
        }

        return sb.ToString();
      }
    }

    public static async Task WriteAllTextAsync(this FileInfo file, string text, FileMode mode)
    {
      var encodedText = Encoding.Unicode.GetBytes(text);

      using (var sourceStream = new FileStream(file.FullName,
        mode, FileAccess.Write, FileShare.None,
        bufferSize: 4096, useAsync: true))
      {
        await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
      }
    }
  }
}