namespace ntwrk.Api.Functions.UserFriend
{
    public interface IUserFriendFunction
    {
        Task<IEnumerable<User.User>> GetListUserFriend(int userId);
        Task<TblUserFriend?> AddFriend(int userId, int friendId);
    }
}
