using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Navigation;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class NewMessageItemViewModel : MvxViewModel
    {
        #region Private fields

        private BaseUserViewModel _user;
        private bool _itemAdded;

        #endregion

        public NewMessageItemViewModel(BaseUserViewModel user, bool itemAdded)
        {
            _itemAdded = itemAdded;
            User = user;
        }

        #region Properties

        public BaseUserViewModel User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public bool ItemAdded
        {
            get => _itemAdded;
            set => SetProperty(ref _itemAdded, value);
        }

        public List<ITransformation> Transformations => new List<ITransformation>
        {
            new CircleTransformation()
        };

        #endregion
    }
}
