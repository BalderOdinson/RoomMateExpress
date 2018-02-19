using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Core.ViewModels;

namespace RoomMateExpress.Core.ViewModels.BaseViewModels
{
    public class BaseViewModel : MvxViewModel
    {
        public Guid Id { get; set; }

        public BaseViewModel()
        {
            Id = Guid.NewGuid();
        }

        public BaseViewModel(Guid id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            return obj?.GetType() == GetType() && ((BaseViewModel) obj).Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
