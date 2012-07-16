using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Phone;

namespace MetroFanfou.Helper
{
    public static class ImageHelper
    {
        /// <summary>
        /// 压缩图片，只调整质量，不调整分辨率
        /// </summary>
        /// <param name="soucre">图片流</param>
        /// <param name="quality">质量1-100</param>
        public static Stream Compression(Stream soucre)
        {
            var quality = AppSetting.ImageQuality;
            soucre.Seek(0, SeekOrigin.Begin);
            var p = quality / 100.0;
            var writeableBitmap = PictureDecoder.DecodeJpeg(soucre);
            var width = writeableBitmap.PixelWidth*p;
            var height = writeableBitmap.PixelHeight*p;
            var outstream = new MemoryStream();
            writeableBitmap.SaveJpeg(outstream, (int)width, (int)height, 0, quality);
            outstream.Seek(0, SeekOrigin.Begin);
            return outstream;
        } 
    }
}
