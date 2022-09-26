using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;

namespace UnoTest
{
    public sealed partial class SpaceView : Page
    {
        private PersonPicture _draggedObject;
        private Canvas _canvas;
        private PointerPoint _ellipsePosition;

        public SpaceView()
        {
            this.InitializeComponent();

            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var _canvas = (Canvas)this.FindName("TestCanvas");
            _canvas.PointerMoved += OnCanvasPointerMoved;
            _canvas.PointerReleased += OnCanvasPointerReleased;
            var top = 0;
            var column = 1;
            for (int i = 1; i < 120; i++)
            {
                var avatar = CreateAvatar(i);
                Canvas.SetLeft(avatar, column * 100);
                Canvas.SetTop(avatar, top);
                _canvas.Children.Add(avatar);
                column++;

                if (i % 10 == 0)
                {
                    column = 1;
                    top += 100;
                }

            }
        }

        private PersonPicture CreateAvatar(int avatarValue)
        {
            var picture = new PersonPicture();
            picture.Height = 50;
            picture.Width = 50;
            var bitmap = new BitmapImage();
            bitmap.UriSource = new System.Uri($"ms-appx:///Assets/p-{avatarValue}.jpg");
            picture.ProfilePicture = bitmap;
            picture.AllowDrop = true;
            picture.PointerPressed += OnEllipsePointerPressed;
            return picture;
        }

        private void OnEllipsePointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (sender is PersonPicture ellipse)
            {
                _ellipsePosition = e.GetCurrentPoint(ellipse);
                _draggedObject = ellipse;
            }
        }

        private void OnCanvasPointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _draggedObject = null;
            e.Handled = true;
        }

        private void OnCanvasPointerMoved(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_draggedObject == null)
                return;

            var canvasPoint = e.GetCurrentPoint(_canvas);
            var ellipsePoint = e.GetCurrentPoint(_draggedObject);

            Canvas.SetLeft(_draggedObject, canvasPoint.Position.X - _ellipsePosition.Position.X);
            Canvas.SetTop(_draggedObject, canvasPoint.RawPosition.Y);

            e.Handled = true;
        }
    }
}