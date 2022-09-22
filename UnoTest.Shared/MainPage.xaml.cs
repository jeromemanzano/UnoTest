using Microsoft.UI.Xaml.Controls;

namespace UnoTest
{
    public sealed partial class MainPage : Page
    {
        private SplitView _splitView;
        private SpaceView _spaceView;
        private ChatView _chatView;

        public MainPage()
        {
            this.InitializeComponent();
            _splitView = (SplitView)this.FindName("SplitView");

            this.Loaded += OnMainPageLoaded;
        }

        private void OnMainPageLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            _spaceView = new SpaceView();
            _chatView = new ChatView();
            _splitView.Content = _chatView;
        }

        private void OnSpaceButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            _splitView.Content = _spaceView;
        }

        private void OnChatButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            _splitView.Content = _chatView;
        }
    }
}
