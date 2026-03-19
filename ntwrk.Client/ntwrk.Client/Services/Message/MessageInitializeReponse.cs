namespace ntwrk.Client.Services.Message
{
    public class MessageInitializeReponse : BaseResponse
    {
        public User FriendInfo { get; set; }
        public IEnumerable<Models.Message> Messages { get; set; }
    }
}
