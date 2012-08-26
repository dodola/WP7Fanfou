using System.IO;

namespace FanFou.SDK.Objects
{
    /// <summary>
    /// 上传文件
    /// </summary>
    public class UploadFile
    {
        /// <summary>
        /// 根据文件名、文件类型与文件流实现化
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="stream">文件数据流</param>
        public UploadFile(string fileName, Stream stream)
        {
            this.FileName = Path.GetFileName(fileName);
            this.ContentType = GetContentType(this.FileName);
            this.FileStream = stream;
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// 本地文件路径
        /// </summary>
        private string FilePath { get; set; }
        /// <summary>
        /// 文件数据流
        /// </summary>
        public Stream FileStream { get;private set; }

        /// <summary>
        /// 将当前的文件数据写入到某个数据流中
        /// </summary>
        /// <param name="stream"></param>
        public void WriteTo(Stream stream)
        {
            byte[] buffer = new byte[512];
            int size = 0;            
            
            if (this.FileStream != null)
            {
                //写入文件流
                while ((size = this.FileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, size);
                }
            }

            if (!string.IsNullOrEmpty(this.FilePath)
                && File.Exists(this.FilePath))
            {
                //写入本地文件流
                using (var reader = new FileStream(this.FilePath, FileMode.Open, FileAccess.Read))
                {
                    while ((size = reader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        stream.Write(buffer, 0, size);
                    }
                }
            }

        }

        /// <summary>
        /// 根据文件扩展名获取文件类型
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        private static string GetContentType(string fileName)
        {
            var fileExt =Path.GetExtension(fileName);
            return GetCommonFileContentType(fileExt);
        }
        /// <summary>
        /// 获取通用文件的文件类型
        /// </summary>
        /// <param name="fileExt">文件扩展名.如".jpg",".gif"等</param>
        /// <returns></returns>
        private static string GetCommonFileContentType(string fileExt)
        {
            switch (fileExt)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".gif":
                    return "image/gif";
                case ".bmp":
                    return "image/bmp";
                case ".png":
                    return "image/png";
                default:
                    return "application/octetstream";
            }
        }
    }
}
