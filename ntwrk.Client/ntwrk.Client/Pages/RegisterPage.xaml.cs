namespace ntwrk.Client.Pages;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterPageViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }
    private void OnLoginLabelTapped(object sender, EventArgs e)
    {
        // Handle login navigation here (e.g., push a login page)
        Shell.Current.Navigation.PopAsync();
    }
}