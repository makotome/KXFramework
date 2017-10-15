using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;

namespace KEngine.Lib
{
    public class KGZipAdapter
    {

        /// <summary>
        /// 压缩字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] CompressString(string rawData)
        {
            MemoryStream ms = new MemoryStream();

            GZipOutputStream compressedzipStream = new GZipOutputStream(ms);
            byte[] data = Encoding.UTF8.GetBytes(rawData);//Util.stringToBytes(strData);
            int size = data.Length;

            compressedzipStream.Write(data, 0, data.Length);
            compressedzipStream.Finish();
            compressedzipStream.Close();
            byte[] result = ms.ToArray();
            return result;
        }

        /// <summary>
        /// 解压字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DecompressString(byte[] data)
        {
            int size_before = data.Length;

            MemoryStream ms = new MemoryStream(data);
            GZipInputStream compressedzipStream = new GZipInputStream(ms);
            MemoryStream outBuffer = new MemoryStream();
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            compressedzipStream.Close();
            byte[] result = outBuffer.ToArray();
            return Encoding.UTF8.GetString(result);
        }
    }
}