using System;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework;

namespace MetroFanfou.Helper
{
    public static class IsolatedHelper
    {
        /// <summary>
        /// 转播或者回复微博时的目标微博存储KEY
        /// </summary>
        public static string TargetTweetKey = "Fanfou.Helper.IsolatedHelper.TargetTweetKey";

        /// <summary>
        /// 话题存储的KEY
        /// </summary>
        public static string HuaTiKey = "Fanfou.Helper.IsolatedHelper.HuaTiKey";

        /// <summary>
        /// 图片质量
        /// </summary>
        public static string PictureQualitykey = "Fanfou.Helper.IsolatedHelper.PictureQualitykey";

        /// <summary>
        /// 主界面检查更新
        /// </summary>
        public static string CheckUpdateCountSpanKey = "Fanfou.Helper.IsolatedHelper.CheckUpdateCountSpanKey";

        /// <summary>
        /// 主界面检查更新
        /// </summary>
        public static string ExitAppConfirmKey = "Fanfou.Helper.IsolatedHelper.ExitAppConfirmKey";

        /// <summary>
        /// 帐号
        /// </summary>
        public static string AccountNameKey = "Fanfou.Helper.IsolatedHelper.AccountNameKey";

        /// <summary>
        /// 后台进程
        /// </summary>
        public static string ScheduledAgentKey = "Fanfou.Helper.IsolatedHelper.ScheduledAgentKey";

        /// <summary>
        /// 微博草稿
        /// </summary>
        public static string TweetDraftKey = "Fanfou.Helper.IsolatedHelper.TweetDraftKey";

        /// <summary>
        /// 主题
        /// </summary>
        public static string ThemeKey = "Fanfou.Helper.IsolatedHelper.ThemeKey";

        /// <summary>
        /// 页面数量
        /// </summary>
        public static string PageCount = "Fanfou.Helper.IsolatedHelper.PageCount";

        public static string LastId = "Fanfou.Helper.IsolatedHelper.LastId";

        #region 保存文件用

        // Fields
        public static bool CacheClearing;

        // Methods
        public static void ClearDirectoryFiles(string directoryName)
        {
            IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
            foreach (string str in userStoreForApplication.GetFileNames(directoryName + @"\*"))
            {
                try
                {
                    userStoreForApplication.DeleteFile(directoryName + @"\" + str);
                }
                catch (Exception)
                {
                }
            }
        }

        public static void DeleteFile(string filename)
        {
            IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
            try
            {
                userStoreForApplication.DeleteFile(filename);
            }
            catch
            {
            }
        }

        public static bool FileExists(string filename)
        {
            IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
            using (userStoreForApplication)
            {
                return userStoreForApplication.FileExists(filename);
            }
        }

        public static long GetDirectorySize(string directoryName)
        {
            IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
            long num = 0L;
            foreach (string str in userStoreForApplication.GetFileNames(directoryName + @"\*"))
            {
                if (CacheClearing)
                {
                    return num;
                }
                if (FileExists(directoryName + @"\" + str))
                {
                    Stream stream = userStoreForApplication.OpenFile(directoryName + @"\" + str, FileMode.Open,
                                                                     FileAccess.Read, FileShare.Read);
                    num += stream.Length;
                }
            }
            return num;
        }

        public static string GetFileAsString(string filename)
        {
            var reader = new StreamReader(StreamFileFromIsoStore(filename));
            return reader.ReadToEnd();
        }

        public static byte[] ReadFully(Stream stream, int initialLength)
        {
            int num2;
            if (initialLength < 1)
            {
                initialLength = 0x8000;
            }
            var buffer = new byte[initialLength];
            int offset = 0;
            while ((num2 = stream.Read(buffer, offset, buffer.Length - offset)) > 0)
            {
                offset += num2;
                if (offset == buffer.Length)
                {
                    int num3 = stream.ReadByte();
                    if (num3 == -1)
                    {
                        return buffer;
                    }
                    var buffer2 = new byte[buffer.Length*2];
                    Array.Copy(buffer, buffer2, buffer.Length);
                    buffer2[offset] = (byte) num3;
                    buffer = buffer2;
                    offset++;
                }
            }
            var destinationArray = new byte[offset];
            Array.Copy(buffer, destinationArray, offset);
            return destinationArray;
        }

        public static void SaveFileToLocal(string fileName, string soursefileName)
        {
            if (!FileExists(fileName))
            {
                try
                {
                    SaveToIsoStore(fileName, TitleContainer.OpenStream(soursefileName));
                }
                catch (Exception)
                {
                }
            }
        }

        public static void SaveStringToLocal(string fileName, string data)
        {
            IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
            if (FileExists(fileName))
            {
                DeleteFile(fileName);
            }
            try
            {
                using (var writer = new StreamWriter(userStoreForApplication.OpenFile(fileName, FileMode.Create)))
                {
                    writer.Write(data);
                }
            }
            catch (Exception)
            {
            }
        }

        public static void SaveToIsoStore(string fileName, byte[] data)
        {
            IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
            try
            {
                using (var writer = new BinaryWriter(userStoreForApplication.CreateFile(fileName)))
                {
                    writer.Write(data);
                    writer.Close();
                }
            }
            catch
            {
            }
        }

        public static void SaveToIsoStore(string fileName, Stream stream)
        {
            byte[] data = ReadFully(stream, (int) stream.Length);
            stream.Close();
            SaveToIsoStore(fileName, data);
        }

        public static Stream StreamFileFromIsoStore(string filename)
        {
            Stream stream;
            IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFile file2 = userStoreForApplication;
            try
            {
                stream = userStoreForApplication.OpenFile(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch
            {
                stream = null;
            }
            finally
            {
                if (file2 != null)
                {
                    file2.Dispose();
                }
            }
            return stream;
        }

        #endregion 保存文件用
    }
}