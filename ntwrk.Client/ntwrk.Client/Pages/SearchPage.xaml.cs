namespace ntwrk.Client.Pages;

public partial class SearchPage : ContentPage
{
    public SearchPage(SearchPageViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }
//    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
//    {
//        (this.BindingContext as SearchPageViewModel).Initialize();
//    }
}