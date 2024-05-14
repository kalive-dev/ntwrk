namespace ntwrk.Client.Services.ListChat
{
    public class ListChatInitializeResponse : BaseResponse
    {
        public User User { get; set; }
        public IEnumerable<User> UserFriends { get; set; }
        public IEnumerable<LastestMessage> LastestMessages { get; set; }
    }
}
