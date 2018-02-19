using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class MessageGroupItemViewModel : MvxViewModel
    {
        private BaseUserViewModel _user;

        public BaseUserViewModel User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public MessageGroupItemViewModel(NewMessageItemViewModel newMessageItemViewModel)
        {
            User = newMessageItemViewModel.User;
        }

        public List<ITransformation> Transformations => new List<ITransformation>
        {
            new CircleTransformation()
        };

        public override bool Equals(object obj)
        {
            return obj?.GetType() == GetType() && ((MessageGroupItemViewModel)obj).User.Id.Equals(User.Id);
        }

        public override int GetHashCode()
        {
            return User.Id.GetHashCode();
        }
    }
}
