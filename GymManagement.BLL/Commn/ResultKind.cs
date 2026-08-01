using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Commn
{
    public enum ResultKind
    {
        Ok,
        NotFound,
        Conflict,
        ValidationFalied,
        Forbidden 

    }
}
