using CL.Services.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts
{
    public class Friend : IFriend
    {
        public int Friend_Id { get; set; }
        
        public String Userto { get; set; }

        public String Userfrom { get; set; }
    }
}
