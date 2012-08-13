using System.Windows.Controls;
using System.Windows.Media;
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Net.NetworkInformation;

namespace MetroFanfou.Helper
{
    public class Tools
    {
        public static bool IsEnableNetWorkConnection()
        {
            return DeviceNetworkInformation.IsNetworkAvailable;
        }

        public static bool IsWifi()
        {
            var type = NetworkInterface.NetworkInterfaceType;

            return (type == NetworkInterfaceType.Wireless80211) || type == NetworkInterfaceType.Ethernet;
        }

        // Fields
        public static PopToastCompleted OnPopToastCompleted;

        private static bool _showFlag;

        // Methods
        public static void PopToast(string title, string content = "", int MillisecondsUntilHidden = 0x3e8)
        {
            if (!_showFlag)
            {
                _showFlag = true;
                ToastPrompt prompt = new ToastPrompt
                {
                    MillisecondsUntilHidden = MillisecondsUntilHidden,
                    Title = title,
                    Message = content
                };
                prompt.FontSize = 24.0;
                prompt.TextOrientation = 0;
                prompt.Foreground = new SolidColorBrush(Colors.White);
                prompt.Background = new SolidColorBrush(Colors.Black);
                prompt.Completed += ToastCompleted;
                prompt.Show();
            }
        }

        public static void ToastCompleted(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            _showFlag = false;
            if (OnPopToastCompleted != null)
            {
                OnPopToastCompleted();
            }
            OnPopToastCompleted = null;
        }

        public delegate void PopToastCompleted();

        public static void SavePicture(Image img)
        {
            //PhotoSet set = this.CurrentPhoto();
            //if (null == set)
            //{
            //    CustomControls.PopToast("图片不存在，无法保存！", "", 0x3e8);
            //}
            //else
            //{
            //    ImageCacherEngine.imgSuffixString = string.Format("480x{0}", 720);
            //    string filename = ImageCacherEngine.FileCacheName(set.ImageUri);
            //    if (IsoStore.FileExists(filename))
            //    {
            //        Stream source = IsoStore.StreamFileFromIsoStore(filename);
            //        if (source.Length > 0L)
            //        {
            //            Picture picture = new MediaLibrary().SavePicture(filename.Substring(7, filename.Length - 7), source);
            //            CustomControls.PopToast("保存成功！", "", 0x3e8);
            //        }
            //    }
            //    else
            //    {
            //        CustomControls.PopToast("图片不存在，无法保存！", "", 0x3e8);
            //    }
            //}
        }
    }
}