using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomMateExpressWebApi.EF.Models
{
    [Flags]
    public enum PetOptions
    {
        None = 0,
        Fish = 1,
        Dog = 2,
        Cat = 4,
        Bird = 8,
        SmallAnimal = 16,
        Reptile = 32,
        Other = 64

    }
}
