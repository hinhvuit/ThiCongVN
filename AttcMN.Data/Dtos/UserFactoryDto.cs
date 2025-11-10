using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.Data.Dtos
{
    public class UserFactoryDto : BaseDto
    {
        public long UserId { get; set; }
        public List<long> UserIds { get; set; }
        public int FacID { get; set; }
    }
}
