//using Foundation;
namespace ntwrk.Client.ViewModels
{
    public class ChatPageViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query == null || query.Count == 0) return;

            FromUserId = int.Parse(HttpUtility.UrlDecode(query["fromUserId"].ToString()));
            ToUserId = int.Parse(HttpUtility.UrlDecode(query["toUserId"].ToString()));

        }

        private ServiceProvider _serviceProvider;
        private ChatHub _chatHub;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ChatPageViewModel(ServiceProvider serviceProvider, ChatHub chatHub)
        {
            Messages = new ObservableCollection<Message>();
            _serviceProvider = serviceProvider;
            _chatHub = chatHub;
            _chatHub.AddReceivedMessageHandler(OnReceiveMessage);
            _chatHub.Connect();

            GoBackCommand = new Command(async () =>
            {
                await Shell.Current.Navigation.PopAsync();
            });
            SendMessageCommand = new Command(async () =>
            {
                try
                {
                    if (Message.Trim() != "")
                    {
                        await _chatHub.SendMessageToUser(FromUserId, ToUserId, Message);

                        Messages.Add(new Models.Message
                        {
                            Content = Message,
                            FromUserId = FromUserId,
                            ToUserId = ToUserId,
                            SendDateTime = DateTime.Now
                        });

                        Message = "";
                    }
                }
                catch (Exception ex)
                {
                    await AppShell.Current.DisplayAlert("NTWRK", ex.Message, "OK");
                }
            });
        }

        async Task GetMessages()
        {
            var request = new MessageInitializeRequest
            {
                FromUserId = FromUserId,
                ToUserId = ToUserId,
            };

            var response = await _serviceProvider.CallWebApi<MessageInitializeRequest, MessageInitializeReponse>
                ("/Message/Initialize", HttpMethod.Post, request);

            if (response.StatusCode == 200)
            {
                FriendInfo = response.FriendInfo;
                Messages = new ObservableCollection<Message>(response.Messages);
            }
            else
            {
                await AppShell.Current.DisplayAlert("NTWRK", response.StatusMessage, "OK");
            }
        }
        public void Initialize()
        {
            Task.Run(async () =>
            {
                IsRefreshing = true;
                await GetMessages();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false;
            });
        }

        private void OnReceiveMessage(int fromUserId, string message)
        {
            Messages.Add(new Models.Message
            {
                Content = message,
                FromUserId = ToUserId,
                ToUserId = FromUserId,
                SendDateTime = DateTime.Now
            });
        }
        //private void OnReading() {
        //    if(Messages.Last().IsRead == true)
        //    {
        //        _isRead = "Read";
        //    } else
        //    {
        //        _isRead = "";
        //    }
        //}
        //private void ScrollToEndOfMessages()
        //{
        //    if (_collectionView != null && Messages.Count > 0)
        //    {
        //        Device.BeginInvokeOnMainThread(() =>
        //        {
        //            _collectionView.ScrollTo(Messages.Last(), ScrollToPosition.End, true);
        //        });
        //    }
        //}


        private int fromUserId;
        private int toUserId;
        private User friendInfo;
        private ObservableCollection<Message> messages;
        private bool isRefreshing;
        private string message;
        //private string _isRead = "";
        //public string isRead
        //{
        //    get { return _isRead; }
        //    set { _isRead = value; OnPropertyChanged(); }
        //}
        public int FromUserId
        {
            get { return fromUserId; }
            set { fromUserId = value; OnPropertyChanged(); }
        }

        public int ToUserId
        {
            get { return toUserId; }
            set { toUserId = value; OnPropertyChanged(); }
        }

        public User FriendInfo
        {
            get { return friendInfo; }
            set { friendInfo = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set { messages = value; OnPropertyChanged(); }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(); }
        }

        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(); }
        }

        public ICommand SendMessageCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
    }
}
