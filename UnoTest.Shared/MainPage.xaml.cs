using Microsoft.UI.Xaml.Controls;

namespace UnoTest
{
    public sealed partial class MainPage : Page
    {
        private NavigationView _navigationView;
        private SpaceView SpaceView { get; } = new SpaceView();
        private ChatView ChatView { get; } = new ChatView();

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += OnMainPageLoaded;
            _navigationView = (NavigationView)this.FindName("NavigationView");
            _navigationView.SelectionChanged += OnNavigationViewSelectionChanged;
        }

        private void OnMainPageLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            _navigationView.Content = SpaceView;

        }

        private void OnNavigationViewSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var clickedItem = (NavigationViewItem)args.SelectedItem;
            string itemTag = (string)clickedItem.Tag;

            switch (itemTag)
            {
                case "Chat":
                    _navigationView.Content = ChatView;
                    break;

                case "Space":
                    _navigationView.Content = SpaceView;
                    break;
            }
        }
    }
}
