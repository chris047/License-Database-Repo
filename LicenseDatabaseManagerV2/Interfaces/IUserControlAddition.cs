using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseDatabaseManagerV2.Interfaces
{
    public interface IUserControlAddition //Allows communication between usercontrol and addition usercontrol popup.
    {
        void ChangeAdditionSearchVisibility(int type);
        /* 
            0: General addition File
            1: Owner addition File
            2: License addition File
            3: Client addition File
        */
    }
}
