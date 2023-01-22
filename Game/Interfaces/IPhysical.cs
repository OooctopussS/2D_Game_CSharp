using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Interfaces
{
    /*interface IPhysical
    {

    }*/

    interface IPhysicalActive //: IPhysical
    {
        bool CollideY();
        bool CollideX();

    }

    interface IPhysicalPassive //: IPhysical
    {

    }
}
