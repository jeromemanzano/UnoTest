using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private int _messageNumber = 1;
        public ChatView()
        {
            this.InitializeComponent();
            _listView = (ListView)this.FindName("ListView");
            _chats = new PaginatedCollection<Chat>(async (start, size) => await FetchItems(10000), 10);
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
                items.Add(new Chat()
                {
                    ChatType = (ChatType)(random.Next() % 3),
                    MessageNumber = _messageNumber
                });

                _messageNumber++;
            }
            return items.ToArray();
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
        public int MessageNumber { get; set; }
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

            return RichText;
        }
    }
}
