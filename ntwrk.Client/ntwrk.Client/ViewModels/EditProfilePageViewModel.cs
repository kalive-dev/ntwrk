using Microsoft.Maui.Storage;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace ntwrk.Client.ViewModels
{
    public class EditProfilePageViewModel : INotifyPropertyChanged, IQueryAttributable
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
            GoBackCommand = new Command(async () =>
            {
                await Shell.Current.Navigation.PopAsync();
            });
            PickImageCommand = new Command(async () =>
            {
                await PickImage();
            });
            _serviceProvider = serviceProvider;
        }
        public bool IsProcessing { get; set; }
        private User _userInfo;
        public User UserInfo
        {
            get { return _userInfo; }
            set { _userInfo = value; OnPropertyChanged(); }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(); }
        }
        public void Initialize()
        {
            Task.Run(async () =>
            {
                IsRefreshing = true;
                await GetUser();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false;
            });
        }
        async Task PickImage()
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images
            });
            if (result == null)
                return;
            var stream = await result.OpenReadAsync();
            var filePath = Path.Combine(FileSystem.AppDataDirectory, "Images", "picked_image.jpg");

            using (var fileStream = File.OpenWrite(filePath))
            {
                await stream.CopyToAsync(fileStream);
            }

            // Set the image path to the UserInfo
            UserInfo.AvatarSourceName = filePath;

            // Notify property changed for UserInfo
            OnPropertyChanged(nameof(UserInfo));
        }


        async Task GetUser()
        {
            var response = await _serviceProvider.CallWebApi<int, ListChatInitializeResponse>
                ("/ListChat/Initialize", HttpMethod.Post, UserInfo.Id);

            if (response.StatusCode == 200)
            {
                UserInfo = response.User;
            }
            else
            {
                await AppShell.Current.DisplayAlert("NTWRK", response.StatusMessage, "OK");
            }
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query == null || query.Count == 0) return;
            UserInfo.Id = int.Parse(HttpUtility.UrlDecode(query["userId"].ToString()));
        }
        public ICommand GoBackCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }
        public ICommand PickImageCommand { get; set; }
    }
}
