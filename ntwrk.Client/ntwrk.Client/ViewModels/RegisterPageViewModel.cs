namespace ntwrk.Client.ViewModels
{
    public class RegisterPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }
        private string _loginId;
        public string LoginId
        {
            get { return _loginId; }
            set { _loginId = value; OnPropertyChanged(); }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }
        private string _confirmPassword;
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { _confirmPassword = value; OnPropertyChanged(); }
        }
        private bool _isProcessing;
        public bool IsProcessing
        {
            get { return _isProcessing; }
            set { _isProcessing = value; OnPropertyChanged(); }
        }

        public ICommand RegisterCommand { get; set; }

        public RegisterPageViewModel(ServiceProvider serviceProvider)
        {
            // Initialize properties or commands here 
            // (e.g., RegisterCommand = new Command(async () => await Register()))
            IsProcessing = false;

            RegisterCommand = new Command(async () =>
            {
                if (IsProcessing) return;
                if (_confirmPassword != _password)
                {
                    await AppShell.Current.DisplayAlert("NTWRK", "Passwords don't matched", "OK");
                    return;
                }
                if (string.IsNullOrEmpty(LoginId) && string.IsNullOrEmpty(UserName) && Password?.Length >= 8) return;

                IsProcessing = true;
                Register().GetAwaiter().OnCompleted(() =>
                {
                    IsProcessing = false;
                });
            });
            this._serviceProvider = serviceProvider;
        }
        private ServiceProvider _serviceProvider;
        private async Task Register()
        {
            // Implement registration logic here
            // (e.g., call a service to register the user)
            IsProcessing = true;
            try
            {
                // Replace with your actual registration logic
                var request = new RegisterRequest
                {
                    LoginId = LoginId,
                    UserName = UserName,
                    Password = Password,
                };
                var response = await _serviceProvider.Register(request);

                if (response.StatusCode == 200)
                {
                    try
                    {
                        var authRequest = new AuthenticateRequest
                        {
                            LoginId = LoginId,
                            Password = Password,
                        };
                        var authResponse = await _serviceProvider.Authenticate(authRequest);
                        if (authResponse.StatusCode == 200)
                        {
                            await Shell.Current.GoToAsync($"ListChatPage?userId={response.Id}");
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
    }
}

