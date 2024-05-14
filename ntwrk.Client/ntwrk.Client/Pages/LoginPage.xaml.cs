namespace ntwrk.Client.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }
    private void OnRegisterLabelTapped(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"RegisterPage");
    }

}