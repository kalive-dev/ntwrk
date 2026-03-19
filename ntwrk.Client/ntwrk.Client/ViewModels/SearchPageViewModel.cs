namespace ntwrk.Client.ViewModels
{
    public class SearchPageViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private ServiceProvider _serviceProvider;
        public SearchPageViewModel(ServiceProvider serviceProvider)
        {
            UserInfo = new User();
            ReceivedUsers = new ObservableCollection<User>();
            _serviceProvider = serviceProvider;
            OpenChatPageCommand = new Command<int>(async (param) =>
            {
                await Shell.Current.GoToAsync($"ChatPage?fromUserId={UserInfo.Id}&toUserId={param}");
            });
            SearchCommand = new Command(async () =>
            {
                if (IsProcessing) return;
                if (_searchRequestData == "") return;
                IsProcessing = true;
                Search(SearchRequestData).GetAwaiter().OnCompleted(() =>
                {
                    IsProcessing = false;
                });
            });
            GoBackCommand = new Command(async () =>
            {
                await Shell.Current.Navigation.PopAsync();
            });
            AddFriendCommand = new Command<int>(async (param) =>
            {
                if (IsProcessing) return;
                IsProcessing = true;
                AddFriend(param).GetAwaiter().OnCompleted(() =>
                {
                    IsProcessing = false;
                });
            });
        }
        private async Task AddFriend(int friendId)
        {
            try
            {
                var request = new FriendRequest()
                {
                    userId = UserInfo.Id,
                    friendId = friendId
                };
                await _serviceProvider.CallWebApi<FriendRequest, FriendResponse>("/Friend/AddFriend", HttpMethod.Post, request);
                await AppShell.Current.Navigation.PopAsync();
                
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("NTWRK", ex.Message, "OK");
            }
        }
        private async Task Search(string searchRequestData)
        {
            try
            {
                // Replace with your actual registration logic
                var request = new SearchRequest
                {
                    SearchRequestData = searchRequestData
                };

                var response = await _serviceProvider.Search(request);
                if (response.StatusCode == 200)
                {
                    ReceivedUsers = (ObservableCollection<User>)response.Users;
                }
                else
                {
                    await AppShell.Current.DisplayAlert("NTWRK", response.StatusMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("NTWRK", ex.Message, "OK");
            }
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query == null || query.Count == 0) return;

            UserInfo.Id = int.Parse(HttpUtility.UrlDecode(query["userId"].ToString()));
        }
        public bool IsProcessing { get; set; }
        private string _searchRequestData;
        public string SearchRequestData
        {
            get { return _searchRequestData; }
            set { _searchRequestData = value; OnPropertyChanged(); }
        }
        private User _userInfo;
        public User UserInfo
        {
            get { return _userInfo; }
            set { _userInfo = value; OnPropertyChanged(); }
        }
        private ObservableCollection<User> _receivedUsers;
        public ObservableCollection<User> ReceivedUsers
        {
            get { return _receivedUsers; }
            set { _receivedUsers = value; OnPropertyChanged(); }
        }
        public ICommand SearchCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public ICommand OpenChatPageCommand { get; set; }
        public ICommand AddFriendCommand { get; set; }
    }
}