using System;

namespace CL.Services.Contracts.Interfaces
{
    public interface IFriend
    {
        int Friend_Id { get; set; }

        String Userto { get; set; }

        String Userfrom { get; set; }
    }
}
