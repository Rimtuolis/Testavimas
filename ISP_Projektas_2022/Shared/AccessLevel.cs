using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISP_Projektas_2022.Shared
{
    public enum AccessLevelType
    {
        NONE = 0,
        CLIENT = 1,
        WORKER = 2,
        SUPPLIER = 3,

        ADMIN = 4
    }
}