using System;
using System.IO;
using System.Text;
using System.Web;

namespace BtVideo.Helpers
{
    public class LogHelper
    {
        public static void WriteLog(string tile, string q)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(HttpContext.Current.Server.MapPath("~/log/" + DateTime.Now.ToString("yyyyMMdd") + "/paymentlog.txt"));
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }
                using (StreamWriter streamWriter = new StreamWriter(fileInfo.FullName, true, Encoding.UTF8))
                {
                    streamWriter.WriteLine(DateTime.Now);
                    streamWriter.WriteLine(tile);
                    streamWriter.WriteLine(q);
                    streamWriter.WriteLine();
                    streamWriter.Close();
                }
            }
            catch
            {
            }
        }
    }
}