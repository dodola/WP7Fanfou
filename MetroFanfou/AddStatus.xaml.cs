using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using FanFou.SDK.API;
using FanFou.SDK.Objects;
using MetroFanfou.Controls;
using MetroFanfou.Helper;
using MetroFanfou.common;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace MetroFanfou
{
    public partial class AddStatus : BasePage
    {
        private readonly Photos _photos = new Photos(OauthHelper.OAuth());
        private readonly Statuses _status = new Statuses(OauthHelper.OAuth());

        private readonly SolidColorBrush _textBlock_focus = new SolidColorBrush(Colors.White);
        private readonly SolidColorBrush _textBlock_lostFocus = new SolidColorBrush(Color.FromArgb(255, 229, 229, 229));
        private UploadFile uploadFile;

        public AddStatus()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void editContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(editContent.Text))
            {
                iconDelete.Visibility = Visibility.Collapsed;
            }
            else
            {
                iconDelete.Visibility = Visibility.Visible;
            }
        }

        private void EditTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            editContent.Background = _textBlock_focus;
            inputView.Background = _textBlock_focus;
        }

        private void EditTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            editContent.Background = _textBlock_lostFocus;
            inputView.Background = _textBlock_lostFocus;
        }

        private void ShowImage_Tap(object sender, GestureEventArgs e)
        {
        }

        private void ClearContent_Tap(object sender, GestureEventArgs e)
        {
        }

        private void mTopic_Click(object sender, EventArgs e)
        {
        }

        private void mClearText_Click(object sender, EventArgs e)
        {
        }

        private void mClearPhoto_Click(object sender, EventArgs e)
        {
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            StartSystemTrayProgress();
            var t = new Thread(() =>
                                      {


                                          if (uploadFile != null)
                                              _photos.Upload(SendCallback, uploadFile, editContent.Text);
                                          else
                                              _status.UpdateStatus(SendCallback, editContent.Text);
                                          StopSystemTrayProgress();



                                      });
            t.Start();
            NavigationService.GoBack();

        }

        private void SendCallback(Status status)
        {
        }

        private void btnPhoto_Click(object sender, EventArgs e)
        {
            var photoTask = new PhotoChooserTask { ShowCamera = true };
            photoTask.Completed += PhotoChooserTask_Completed;
            photoTask.Show();
        }

        private void PhotoChooserTask_Completed(object sender, PhotoResult e)
        {
            try
            {
                if (e.ChosenPhoto != null)
                {
                    ((ApplicationBarMenuItem)ApplicationBar.MenuItems[1]).IsEnabled = true; //清空图片
                    imageIcon.Visibility = Visibility.Visible;
                    if (string.IsNullOrWhiteSpace(editContent.Text))
                    {
                        editContent.Text = "#分享照片#";
                    }
                    uploadFile = new UploadFile(e.OriginalFileName, ImageHelper.Compression(e.ChosenPhoto));

                    //保存缩略图
                    ImageSource imageSource = ImageHelper.SaveThumbnail(Const.TempThumbnailFileName, e.ChosenPhoto);

                    //保存原图
                    ImageHelper.SaveImage(Const.TempImageFileName, e.ChosenPhoto);

                    imageIcon.Source = imageSource;
                }
            }
            catch
            {
            }
        }

        private void btnMentions_Click(object sender, EventArgs e)
        {
        }

        private void btnFace_Click(object sender, EventArgs e)
        {
        }
    }
}