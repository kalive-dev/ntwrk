namespace ntwrk.Client;

public partial class AppShell : Shell
{
    public AppShell(LoginPage loginPage)
    {
        InitializeComponent();

        Routing.RegisterRoute("ListChatPage", typeof(ListChatPage));
        Routing.RegisterRoute("ChatPage", typeof(ChatPage));
        Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
        Routing.RegisterRoute("SearchPage", typeof(SearchPage));
        Routing.RegisterRoute("EditProfilePage", typeof(EditProfilePage));
        this.CurrentItem = loginPage;
    }

    //public AppShell(ChatPage chatPage)
    //{
    //    InitializeComponent();

    //    Routing.RegisterRoute("ListChatPage", typeof(ListChatPage));

    //    this.CurrentItem = chatPage;
    //}
}
