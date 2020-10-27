using System.IO;
using Winista.Mime;

namespace ProposalPolmanAstra.Classes
{
    public class MimeDetector
    {
        public static string getMimeType(string path, string filename)
        {
            try
            {
                MimeTypes g_MimeTypes = new MimeTypes(path + "mime-types.xml");
                sbyte[] fileData = null;
                using (FileStream srcFile = new FileStream(path + filename, FileMode.Open))
                {
                    byte[] data = new byte[srcFile.Length];
                    srcFile.Read(data, 0, (int)srcFile.Length);
                    fileData = SupportUtil.ToSByteArray(data);
                }
                MimeType oMimeType = g_MimeTypes.GetMimeType(fileData);
                return oMimeType.Name;
            }
            catch { return "unknown/unknown"; }
        }
    }
}