using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttcMN.Common.Enums
{
    public enum StatusAct
    {
        [Description("Dong Y")]
        dong_y,

        [Description("Tra Lai")]
        tra_lai
    }
    public enum StatusSign
    {
        [Description("Cho Ky")]
        cho_ky,

        [Description("Chua Ky")]
        chua_ky,
        [Description("Da Ky")]
        da_ky
    }
}
