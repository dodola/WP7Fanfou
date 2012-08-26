using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Phone;

namespace MetroFanfou.Helper
{
    public static class ImageHelper
    {
        /// <summary>
        /// 压缩图片，只调整质量，不调整分辨率
        /// </summary>
        /// <param name="source">图片流</param>
        /// <param name="quality">质量1-100</param>
        public static Stream Compression(Stream source)
        {
            int quality = AppSetting.ImageQuality;
            source.Seek(0, SeekOrigin.Begin);
            double p = quality / 100.0;
            WriteableBitmap writeableBitmap = PictureDecoder.DecodeJpeg(source);
            double width = writeableBitmap.PixelWidth * p;
            double height = writeableBitmap.PixelHeight * p;
            var outstream = new MemoryStream();
            writeableBitmap.SaveJpeg(outstream, (int)width, (int)height, 0, quality);
            outstream.Seek(0, SeekOrigin.Begin);
            return outstream;
        }

        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="source"></param>
        /// <param name="quality"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Stream Compression(Stream source, int quality, int width, int height)
        {
            source.Seek(0, SeekOrigin.Begin);
            WriteableBitmap writeableBitmap = PictureDecoder.DecodeJpeg(source);
            var outstream = new MemoryStream();
            writeableBitmap.SaveJpeg(outstream, width, height, 0, quality);
            outstream.Seek(0, SeekOrigin.Begin);
            return outstream;
        }

        /// <summary>
        /// 压缩图片并保存
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="source"></param>
        public static ImageSource SaveThumbnail(string fileName, Stream source)
        {
            Stream compressSource = Compression(source, 50, 40, 40);

            SaveImage(fileName, compressSource);
            var compressImage = new BitmapImage();
            compressImage.SetSource(compressSource);
            return compressImage;

        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="source"></param>
        public static void SaveImage(string fileName, Stream source)
        {
            using (IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (
                    IsolatedStorageFileStream targetStream = isStore.OpenFile(fileName, FileMode.Create,
                                                                              FileAccess.Write))
                {
                    //设置缓冲区
                    var readBuffer = new byte[4096];
                    int bytesRead = -1;

                    // 保存到isolatestorage
                    while ((bytesRead = source.Read(readBuffer, 0, readBuffer.Length)) > 0)
                    {
                        targetStream.Write(readBuffer, 0, bytesRead);
                    }
                }
            }
        }
    }
}