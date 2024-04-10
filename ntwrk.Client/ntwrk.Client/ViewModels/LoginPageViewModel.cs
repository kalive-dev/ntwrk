using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ntwrk.Client.ViewModels
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ServiceProvider _serviceProvider;
        public LoginPageViewModel()
        {
            UserName = "wanda";
            Password = "Abc12345";
            IsProcessing = false;
             
            LoginCommand = new Command(() =>
            {
                if (IsProcessing) return;

                if (UserName.Trim() == "" || Password.Trim() == "") return;

                IsProcessing = true;
                Login().GetAwaiter().OnCompleted(() =>
                {
                    IsProcessing = false;
                });
            });
        }

        async Task Login()
        {
            try
            {

                var request = new AuthenticateRequest
                {
                    LoginId = UserName,
                    Password = Password
                };
                var response = await ServiceProvider.GetInstance().Authenticate(request);
                if (response.StatusCode == 200)
                {
                    await AppShell.Current.DisplayAlert("NTWRK",
                        "Login sucessful! \n" +
                        $"Username: {response.Username} \n" +
                        $"Token: {response.Token}", "OK");
                }
                else
                {
                    await AppShell.Current.DisplayAlert("NTWRK",
                        $"{response.StatusCode}",
                        response.StatusMessage, "OK");

                }
            }
            catch (Exception ex)
            {
                await AppShell.Current.DisplayAlert("NTWRK",
                        ex.Message, "OK");
            }
        }

        private string userName;
        private string password;
        private bool isProcessing;

        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }

        public bool IsProcessing
        {
            get { return isProcessing; }
            set { isProcessing = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; set; }
    }
}