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

            if (result == null) result = new List<User.User>();

            return result;
        }
        public async Task<TblUserFriend?> AddFriend(int userId, int friendId)
        {
            if (_ntwrkContext.TblUserFriends.Any(f =>
                (f.UserId == userId && f.FriendId == friendId)))
            {
                return null;
            }
            var entity = new TblUserFriend() { UserId = userId, FriendId = friendId };
            entity = new TblUserFriend() { UserId = friendId, FriendId = userId }; // temporary solution(to do: friend request page)
            _ntwrkContext.TblUserFriends.Add(entity);
            await _ntwrkContext.SaveChangesAsync();
            return entity;
        }
    }
}
