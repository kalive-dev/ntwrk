using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ntwrk.Client.Services.Message
{
    public class MessageInitializeRequest
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; } 
    }
}
