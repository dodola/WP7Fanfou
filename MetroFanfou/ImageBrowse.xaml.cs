using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using FanFou.SDK.Objects;
using MetroFanfou.common;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MetroFanfou
{
    public partial class ImageBrowse : PhoneApplicationPage
    {
        protected Photo Img;
        private bool _isDragging;
        private bool _isPinching;
        private Point _ptPinchPositionStart;

        public ImageBrowse()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            InitData();
            base.OnNavigatedTo(e);
        }

        private void InitData()
        {
            object imgObj;
            PhoneApplicationService.Current.State.TryGetValue(Const.IMGOBJ, out imgObj);
            if (imgObj != null)
            {
                Img = (Photo)imgObj;
                image.Source = new BitmapImage(new Uri(Img.LargeUrl));
            }
        }


        private void OnGestureListenerDragStarted(object sender, DragStartedGestureEventArgs args)
        {
            _isDragging = args.OriginalSource == image;
        }

        private void OnGestureListenerDragDelta(object sender, DragDeltaGestureEventArgs args)
        {
            if (_isDragging)
            {
                translateTransform.X += args.HorizontalChange;
                translateTransform.Y += args.VerticalChange;
            }
        }

        private void OnGestureListenerDragCompleted(object sender, DragCompletedGestureEventArgs args)
        {
            if (_isDragging)
            {
                TransferTransforms();
                _isDragging = false;
            }
        }

        private void OnGestureListenerPinchStarted(object sender, PinchStartedGestureEventArgs args)
        {
            _isPinching = args.OriginalSource == image;

            if (_isPinching)
            {
                // Set transform centers
                Point ptPinchCenter = args.GetPosition(image);
                ptPinchCenter = previousTransform.Transform(ptPinchCenter);

                scaleTransform.CenterX = ptPinchCenter.X;
                scaleTransform.CenterY = ptPinchCenter.Y;

                rotateTransform.CenterX = ptPinchCenter.X;
                rotateTransform.CenterY = ptPinchCenter.Y;

                _ptPinchPositionStart = args.GetPosition(this);
            }
        }

        private void OnGestureListenerPinchDelta(object sender, PinchGestureEventArgs args)
        {
            if (_isPinching)
            {
                // Set scaling
                scaleTransform.ScaleX = args.DistanceRatio;
                scaleTransform.ScaleY = args.DistanceRatio;
                rotateTransform.Angle = args.TotalAngleDelta;

                // Set translation
                Point ptPinchPosition = args.GetPosition(this);
                translateTransform.X = ptPinchPosition.X - _ptPinchPositionStart.X;
                translateTransform.Y = ptPinchPosition.Y - _ptPinchPositionStart.Y;
            }
        }

        private void OnGestureListenerPinchCompleted(object sender, PinchGestureEventArgs args)
        {
            if (_isPinching)
            {
                TransferTransforms();
                _isPinching = false;
            }
        }

        private void TransferTransforms()
        {
            previousTransform.Matrix = Multiply(previousTransform.Matrix, currentTransform.Value);

            // Set current transforms to default values
            scaleTransform.ScaleX = scaleTransform.ScaleY = 1;
            scaleTransform.CenterX = scaleTransform.CenterY = 0;

            rotateTransform.Angle = 0;
            rotateTransform.CenterX = rotateTransform.CenterY = 0;

            translateTransform.X = translateTransform.Y = 0;
        }

        private static Matrix Multiply(Matrix a, Matrix b)
        {
            return new Matrix(a.M11 * b.M11 + a.M12 * b.M21,
                              a.M11 * b.M12 + a.M12 * b.M22,
                              a.M21 * b.M11 + a.M22 * b.M21,
                              a.M21 * b.M12 + a.M22 * b.M22,
                              a.OffsetX * b.M11 + a.OffsetY * b.M21 + b.OffsetX,
                              a.OffsetX * b.M12 + a.OffsetY * b.M22 + b.OffsetY);
        }
    }
}