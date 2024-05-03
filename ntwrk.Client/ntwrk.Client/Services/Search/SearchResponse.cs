using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ntwrk.Client.Services.Search
{
    public class SearchResponse : BaseResponse
    {
        public IEnumerable<User> Users { get; set; }
    }
}
