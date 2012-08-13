using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MetroFanfou.Controls
{
    public class PullDownToRefreshPanel : Control
    {
        // Fields
        private const string ActivityVisualStateGroup = "ActivityStates";

        private const string InactiveVisualState = "Inactive";
        private const string PullingDownVisualState = "PullingDown";
        private const string ReadyToReleaseVisualState = "ReadyToRelease";
        private const string RefreshingVisualState = "Refreshing";
        public static readonly DependencyProperty IsRefreshingProperty;
        public static readonly DependencyProperty PullDistanceProperty;
        public static readonly DependencyProperty PullFractionProperty;
        public static readonly DependencyProperty PullingDownTemplateProperty;
        public static readonly DependencyProperty PullThresholdProperty;
        public static readonly DependencyProperty ReadyToReleaseTemplateProperty;
        public static readonly DependencyProperty RefreshingTemplateProperty;
        private ScrollViewer _targetScrollViewer;

        // Methods
        static PullDownToRefreshPanel()
        {
            IsRefreshingProperty = DependencyProperty.Register("IsRefreshing", typeof(bool),
                                                               typeof(PullDownToRefreshPanel),
                                                               new PropertyMetadata(false));
            PullThresholdProperty = DependencyProperty.Register("PullThreshold", typeof(double),
                                                                typeof(PullDownToRefreshPanel),
                                                                new PropertyMetadata(100.0));
            PullDistanceProperty = DependencyProperty.Register("PullDistance", typeof(double),
                                                               typeof(PullDownToRefreshPanel),
                                                               new PropertyMetadata(0.0));
            PullFractionProperty = DependencyProperty.Register("PullFraction", typeof(double),
                                                               typeof(PullDownToRefreshPanel),
                                                               new PropertyMetadata(0.0));
            PullingDownTemplateProperty = DependencyProperty.Register("PullingDownTemplate", typeof(DataTemplate),
                                                                      typeof(PullDownToRefreshPanel), null);
            ReadyToReleaseTemplateProperty = DependencyProperty.Register("ReadyToReleaseTemplate", typeof(DataTemplate),
                                                                         typeof(PullDownToRefreshPanel), null);
            RefreshingTemplateProperty = DependencyProperty.Register("RefreshingTemplate", typeof(DataTemplate),
                                                                     typeof(PullDownToRefreshPanel), null);
        }

        public PullDownToRefreshPanel()
        {
            DefaultStyleKey = typeof(PullDownToRefreshPanel);
            LayoutUpdated += PullDownToRefreshPanelLayoutUpdated;
        }

        // Properties
        public bool IsRefreshing
        {
            get { return (bool)GetValue(IsRefreshingProperty); }
            set { SetValue(IsRefreshingProperty, value); }
        }

        public double PullDistance
        {
            get { return (double)base.GetValue(PullDistanceProperty); }
            private set { base.SetValue(PullDistanceProperty, value); }
        }

        public double PullFraction
        {
            get { return (double)base.GetValue(PullFractionProperty); }
            private set { base.SetValue(PullFractionProperty, value); }
        }

        public DataTemplate PullingDownTemplate
        {
            get { return (DataTemplate)base.GetValue(PullingDownTemplateProperty); }
            set { base.SetValue(PullingDownTemplateProperty, value); }
        }

        public double PullThreshold
        {
            get { return (double)base.GetValue(PullThresholdProperty); }
            set { base.SetValue(PullThresholdProperty, value); }
        }

        public DataTemplate ReadyToReleaseTemplate
        {
            get { return (DataTemplate)base.GetValue(ReadyToReleaseTemplateProperty); }
            set { base.SetValue(ReadyToReleaseTemplateProperty, value); }
        }

        public DataTemplate RefreshingTemplate
        {
            get { return (DataTemplate)base.GetValue(RefreshingTemplateProperty); }
            set { base.SetValue(RefreshingTemplateProperty, value); }
        }

        private event EventHandler RefreshRequested;

        private static T FindVisualElement<T>(DependencyObject container) where T : DependencyObject
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(container);
            while (queue.Count > 0)
            {
                DependencyObject obj2 = queue.Dequeue();
                var local = obj2 as T;
                if ((local != null) && (local != container))
                {
                    return local;
                }
                int childrenCount = VisualTreeHelper.GetChildrenCount(obj2);
                for (int i = 0; i < childrenCount; i++)
                {
                    queue.Enqueue(VisualTreeHelper.GetChild(obj2, i));
                }
            }
            return default(T);
        }

        protected void OnIsRefreshingChanged(DependencyPropertyChangedEventArgs e)
        {
            string str = e.NewValue != null && ((bool)e.NewValue) ? RefreshingVisualState : InactiveVisualState;
            VisualStateManager.GoToState(this, str, false);
        }

        private void PullDownToRefreshPanelLayoutUpdated(object sender, EventArgs e)
        {
            if (_targetScrollViewer == null)
            {
                _targetScrollViewer = FindVisualElement<ScrollViewer>(VisualTreeHelper.GetParent(this));
                if (_targetScrollViewer != null)
                {
                    _targetScrollViewer.MouseMove += TargetScrollViewerMouseMove;
                    _targetScrollViewer.MouseLeftButtonUp += TargetScrollViewerMouseLeftButtonUp;
                    if (_targetScrollViewer.ManipulationMode != 0)
                    {
                        throw new InvalidOperationException(
                            "PullDownToRefreshPanel requires the ScrollViewer to have ManipulationMode=Control. (ListBoxes may require re-templating.");
                    }
                }
            }
        }

        private void TargetScrollViewerMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var element = (UIElement)_targetScrollViewer.Content;
            var transform = element.RenderTransform as CompositeTransform;
            if ((transform != null) && !IsRefreshing)
            {
                VisualStateManager.GoToState(this, InactiveVisualState, false);
                PullDistance = 0.0;
                PullFraction = 0.0;
                if (transform.TranslateY >= PullThreshold)
                {
                    EventHandler refreshRequested = RefreshRequested;
                    if (refreshRequested != null)
                    {
                        refreshRequested(this, EventArgs.Empty);
                    }
                }
            }
        }

        private void TargetScrollViewerMouseMove(object sender, MouseEventArgs e)
        {
            var element = (UIElement)_targetScrollViewer.Content;
            var transform = element.RenderTransform as CompositeTransform;
            if ((transform != null) && !IsRefreshing)
            {
                string str;
                if (transform.TranslateY > PullThreshold)
                {
                    PullDistance = transform.TranslateY;
                    PullFraction = 1.0;
                    str = "ReadyToRelease";
                }
                else if (transform.TranslateY > 0.0)
                {
                    PullDistance = transform.TranslateY;
                    double pullThreshold = PullThreshold;
                    PullFraction = (pullThreshold == 0.0) ? 1.0 : Math.Min(1.0, (transform.TranslateY / pullThreshold));
                    str = "PullingDown";
                }
                else
                {
                    PullDistance = 0.0;
                    PullFraction = 0.0;
                    str = "Inactive";
                }
                VisualStateManager.GoToState(this, str, false);
            }
        }
    }
}