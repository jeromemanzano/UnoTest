using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;

namespace UnoTest
{
    public sealed partial class ChatView : Page
    {
        private ListView _listView;
        private PaginatedCollection<Chat> _chats;
        public ChatView()
        {
            this.InitializeComponent();
            this.Loaded += OnChatViewLoaded;
            _listView = (ListView)this.FindName("ListView");
        }

        private void OnChatViewLoaded(object sender, RoutedEventArgs e)
        {
            _chats = new PaginatedCollection<Chat>(async (start, size) => await FetchItems(100), 10);
            _listView.ItemsSource = _chats;
        }

        private async Task<Chat[]> FetchItems(int size)
        {
            //simulates some work to get new items
            await Task.Delay(200);
            var random = new Random();
            var items = new List<Chat>();
            for (int i = 0; i < size; i++)
            {
                _chats.Insert(0, new Chat()
                {
                    ChatType = (ChatType)(random.Next() % 3)
                });
            }
            return items.ToArray();
        }

        private void InsertChatItems()
        {
            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                _chats.Insert(0, new Chat()
                {
                    ChatType = (ChatType)(random.Next() % 3)
                });
            }
        }
    }

    public class PaginatedCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        public delegate Task<T[]> Fetch(int start, int count);

        private readonly Fetch _fetch;
        private int _start, _pageSize;

        public PaginatedCollection(Fetch fetch, int pageSize)
        {
            _fetch = fetch;
            _start = 0;
            _pageSize = pageSize;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run<LoadMoreItemsResult>(async () =>
            {
                var items = await _fetch(_start, _pageSize);
                await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    foreach (var item in items)
                    {
                        Add(item);
                        if (Count > _pageSize)
                        {
                            //hack to give the UI time for layout udpates
                            await Task.Delay(20);
                        }
                    }
                });

                _start += items.Length;

                return new LoadMoreItemsResult() { Count = (uint)items.Length };
            }).AsAsyncOperation();
        }

        public bool HasMoreItems => true;
    }



    public enum ChatType
    {
        Text,
        Image,
        Lottie
    }

    public class Chat
    {
        public ChatType ChatType { get; set; }
    }

    public class TestTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Image { get; set; }
        public DataTemplate RichText { get; set; }
        public DataTemplate Lottie { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is Chat chat)
            {
                switch (chat.ChatType)
                {
                    case ChatType.Image:
                        return Image;
                    case ChatType.Lottie:
                        return Lottie;
                    default:
                        return RichText;
                }
            }

            throw new NotImplementedException();
        }
    }

    public static class ExtendedVisualTreeHelper
    {
        public static T GetFirstDescendant<T>(DependencyObject reference) => GetDescendants(reference)
            .OfType<T>()
            .FirstOrDefault();

        public static T GetFirstDescendant<T>(DependencyObject reference, Func<T, bool> predicate) => GetDescendants(reference)
            .OfType<T>()
            .FirstOrDefault(predicate);

        public static IEnumerable<DependencyObject> GetDescendants(DependencyObject reference)
        {
            foreach (var child in GetChildren(reference))
            {
                yield return child;
                foreach (var grandchild in GetDescendants(child))
                {
                    yield return grandchild;
                }
            }
        }

        public static IEnumerable<DependencyObject> GetChildren(DependencyObject reference)
        {
            return Enumerable
                .Range(0, VisualTreeHelper.GetChildrenCount(reference))
                .Select(x => VisualTreeHelper.GetChild(reference, x));
        }
    }

    public static class ListViewExtensions
    {
        public static DependencyProperty AddLazyLoadingSupportProperty { get; } = DependencyProperty.RegisterAttached(
            "AddLazyLoadingSupport",
            typeof(bool),
            typeof(ListViewExtensions),
            new PropertyMetadata(default(bool), (d, e) => OnAddLazyLoadingSupportChanged(d as ListView, e)));

        public static bool GetAddLazyLoadingSupport(ListView obj) => (bool)obj.GetValue(AddLazyLoadingSupportProperty);
        public static void SetAddLazyLoadingSupport(ListView obj, bool value) => obj.SetValue(AddLazyLoadingSupportProperty, value);

        private static DependencyProperty IsIncrementallyLoadingProperty { get; } = DependencyProperty.RegisterAttached(
            "IsIncrementallyLoading",
            typeof(bool),
            typeof(ListViewExtensions),
            new PropertyMetadata(default(bool)));

        private static bool GetIsIncrementallyLoading(ListView obj) => (bool)obj.GetValue(IsIncrementallyLoadingProperty);
        private static void SetIsIncrementallyLoading(ListView obj, bool value) => obj.SetValue(IsIncrementallyLoadingProperty, value);

        private static void OnAddLazyLoadingSupportChanged(ListView control, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                if (control.IsLoaded)
                {
                    InstallIncrementalLoadingWorkaround(control, null!);
                }
                else
                {
                    control.Loaded += InstallIncrementalLoadingWorkaround;
                }
            }
        }

        private static void InstallIncrementalLoadingWorkaround(object sender, RoutedEventArgs _)
        {
            var lv = (ListView)sender;
            var sv = ExtendedVisualTreeHelper.GetFirstDescendant<ScrollViewer>(lv);
            var loadingThreshold = 0.5;

            sv.Loaded += OnListViewLoaded;
            sv.ViewChanged += async (s, e) =>
            {

                if (lv.ItemsSource is not ISupportIncrementalLoading source) return;
                if (lv.Items.Count > 0 && !source.HasMoreItems) return;
                if (GetIsIncrementallyLoading(lv)) return;

                if (((sv.ExtentHeight - sv.VerticalOffset) / sv.ViewportHeight) - 1.0 <= loadingThreshold)
                {
                    try
                    {
                        SetIsIncrementallyLoading(lv, true);
                        //await source.LoadMoreItemsAsync(1);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("failed to load more items: " + ex);
                    }
                    finally
                    {
                        SetIsIncrementallyLoading(lv, false);
                    }
                }
            };
        }

        private static void OnListViewLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                var test = scrollViewer.ActualHeight;
                var something = scrollViewer.ActualOffset;
                var sosdfa = scrollViewer.ActualSize;
            }
        }
    }
}
