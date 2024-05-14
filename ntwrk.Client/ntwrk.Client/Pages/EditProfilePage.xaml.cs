namespace ntwrk.Client.Pages;

public partial class EditProfilePage : ContentPage
{
    public EditProfilePage(EditProfilePageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (this.BindingContext as EditProfilePageViewModel).Initialize();
    }
}