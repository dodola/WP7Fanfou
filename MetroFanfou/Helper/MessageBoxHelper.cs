using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace MetroFanfou.Helper
{
    public class MessageBoxHelper
    {
        /// <summary>
        /// 消息提示
        /// </summary>
        /// <param name="msg"></param>
        public void Show(string msg, string title, Grid layout)
        {
            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    var _popup = new Popup();
                    var _timer = new System.Windows.Threading.DispatcherTimer();

                    _popup.HorizontalOffset = 0;
                    _popup.VerticalOffset = 0;

                    var grid = new Grid();
                    grid.Width = App.Current.RootVisual.RenderSize.Width;
                    grid.Background = (SolidColorBrush)App.Current.Resources["MessageBoxBgBrush"];
                    grid.MinHeight = 80;
                    grid.Margin = new Thickness(0, 0, 0, 0);
                    var stackPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical
                    };
                    if (!string.IsNullOrWhiteSpace(title))
                    {
                        var tbTitle = new TextBlock
                        {
                            Text = title,
                            FontSize = 24,
                            Foreground = (SolidColorBrush)App.Current.Resources["MessageBoxTextBrush"],
                            Width = grid.Width,
                            Padding = new Thickness(10, 10, 10, 10),
                            Margin = new Thickness(0, 12, 0, 0)
                        };
                        stackPanel.Children.Add(tbTitle);
                    }
                    var tblock = new TextBlock
                    {
                        Text = msg,
                        FontSize = 24,
                        Foreground = (SolidColorBrush)App.Current.Resources["MessageBoxTextBrush"],
                        Width = grid.Width,
                        Padding = new Thickness(10, 10, 10, 10),
                        Margin = new Thickness(0, 0, 0, 0)
                    };
                    stackPanel.Children.Add(tblock);
                    grid.Children.Add(stackPanel);

                    _popup.Child = grid;

                    if (!layout.Children.Contains(_popup))
                    {
                        layout.Children.Add(_popup);
                    }
                    _popup.IsOpen = true;
                    _timer.Interval = new TimeSpan(0, 0, 0, 0, 2500); //  2.5 seconds
                    _timer.Tick += ((o, s) =>
                    {
                        _timer.Stop();
                        _popup.IsOpen = false;
                        layout.Children.Remove(_popup);
                        _popup = null;
                    });
                    _timer.Start();
                });
            }
            catch
            {
            }
        }
    }
}