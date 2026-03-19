using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ntwrk.Client.Services.Friend
{
    public class FriendResponse : BaseResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
    }
}
