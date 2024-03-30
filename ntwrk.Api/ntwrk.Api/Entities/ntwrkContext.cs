using Microsoft.EntityFrameworkCore;

namespace ntwrk.Api.Entities
{
    public class ntwrkContext:DbContext
    {
        public ntwrkContext(DbContextOptions<ntwrkContext> options) :base (options)
        { }

        public virtual DbSet<TblUser> TblUsers { get; set; } = null!;
        public virtual DbSet<TblUserFriend> TblUserFriends { get; set; } = null!;
        public virtual DbSet<TblMessage> TblMessages { get; set; } = null!;
    }
}
