namespace ntwrk.Client.ViewModels
{
    public class EditProfilePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private ServiceProvider _serviceProvider;
        public EditProfilePageViewModel(ServiceProvider serviceProvider)
        {
            UserInfo = new User();
            _serviceProvider = serviceProvider;
            GoBackCommand = new Command(async () =>
            {
                await Shell.Current.Navigation.PopAsync();
            });
            MessagingCenter.Subscribe<User>(this, "UserInfoMessage", (user) =>
            {
                UserInfo = user;
            });
        }
        public bool IsProcessing { get; set; }
        private User _userInfo;
        public User UserInfo
        {
            get { return _userInfo; }
            set { _userInfo = value; OnPropertyChanged(); }
        }
        //public async Task GetUserById()
        //{
        //    try
        //    {
        //        var response = await _serviceProvider.CallWebApi<int, SearchByIdResponse>("/Search/GetUserById", HttpMethod.Post, UserInfo.Id);
        //        UserInfo = response.User;
        //    }
        //    catch (Exception ex)
        //    {
        //        await AppShell.Current.DisplayAlert("NTWRK", ex.Message, "OK");
        //    }
        //}
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(); }
        }
        //public void Initialize()
        //{
        //    Task.Run(async () =>
        //    {
        //        IsRefreshing = true;
        //        await GetUserById();
        //    }).GetAwaiter().OnCompleted(() =>
        //    {
        //        IsRefreshing = false;
        //    });
        //}
        //public void ApplyQueryAttributes(IDictionary<string, object> query)
        //{
        //    if (query == null || query.Count == 0) return;

        //    UserInfo.Id = int.Parse(HttpUtility.UrlDecode(query["userId"].ToString()));
        //}
        public ICommand GoBackCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }
    }
}
