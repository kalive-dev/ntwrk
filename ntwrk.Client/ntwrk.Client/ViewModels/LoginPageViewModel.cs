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
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LoginPageViewModel()
        {
            Username = "";
            Password = "";
            IsProcessing = false;

            LoginCommand = new Command(() =>
            {
                if (isProcessing) return;

                if (Username.Trim() == "" || Password.Trim() == "") return;
                isProcessing = true;
                Login().GetAwaiter().OnCompleted(() =>
                {
                    isProcessing = false;
                });
            });
        }

        async Task Login()
        {
            try {

            var request = new AuthenticateRequest
            {
                LoginId = Username,
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
                else {
                    await AppShell.Current.DisplayAlert("NTWRK",
                        response.StatusMessage, "OK");

                }
            } catch (Exception ex) {
                await AppShell.Current.DisplayAlert("NTWRK",
                        ex.Message, "OK");
            }
        }
       
        private string _username;
        private string _password;
        private bool isProcessing;

        public string Username
        {
            get { return _username; }
            set { _username  = value; OnPropertyChanged(nameof(Username));}
        }

        public string Password
        {
            get { return _password; }
            set { Password = value; OnPropertyChanged(); }
        }
        public bool IsProcessing
        {
            get { return isProcessing; }
            set { isProcessing = value; OnPropertyChanged();}
        }

        public ICommand LoginCommand { get; set; }
    }
}
