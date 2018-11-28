using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BenMann.Docusign
{
    public class Utils
    {
        public static string TrimFilePath(string initialPath, string absolutePath)
        {
            if (initialPath.StartsWith(absolutePath))
            {
                return initialPath.Remove(0, absolutePath.Length).TrimStart('\\');
            }

            return initialPath;
        }

        /// <summary>
        /// Save the content of an HTTP response to file
        /// </summary>
        /// <param name="content">the HTTPContent response</param>
        /// <param name="filename">output file</param>
        /// <param name="overwrite">true to overwrite the file. If false and file already exists through excepetion</param>
        /// <returns>async task</returns>
        public static Task ReadAsFileAsync(HttpContent content, string filename, bool overwrite = true)
        {
            string pathname = Path.GetFullPath(filename);
            if (!overwrite && File.Exists(filename))
            {
                throw new InvalidOperationException(string.Format("File {0} already exists.", pathname));
            }

            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(pathname, FileMode.Create, FileAccess.Write, FileShare.None);
                return content.CopyToAsync(fileStream).ContinueWith(
                    (copyTask) =>
                    {
                        fileStream.Close();
                    });
            }
            catch
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }

                throw;
            }
        }
    }
}
