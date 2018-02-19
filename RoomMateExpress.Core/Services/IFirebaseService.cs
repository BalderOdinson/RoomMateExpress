using System;
using System.Collections.Generic;
using System.Text;

namespace RoomMateExpress.Core.Services
{
    public interface IFirebaseService
    {
        void InitializeToken();
        void Subsrcibe();
        void Unsubscribe();
    }
}
