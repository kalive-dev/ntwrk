namespace ntwrk.Api.Functions.UserFriend
{
    public class UserFriendFunction : IUserFriendFunction
    {
        ntwrkContext _ntwrkContext;
        IUserFunction _userFunction;
        public UserFriendFunction(ntwrkContext ntwrkContext, IUserFunction userFunction)
        {
            _ntwrkContext = ntwrkContext;
            _userFunction = userFunction;
        }
        public async Task<IEnumerable<User.User>> GetListUserFriend(int userId)
        {
            var entities = await _ntwrkContext.TblUserFriends
                .Where(x => x.UserId == userId)
                .ToListAsync();

            var result = entities.Select(x => _userFunction.GetUserById(x.FriendId));

            if (result ==null) result  = new List<User.User>();

            return result;
        }
    }
}
