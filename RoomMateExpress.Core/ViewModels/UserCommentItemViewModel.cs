using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Navigation;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using RoomMateExpress.Core.Settings;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class UserCommentItemViewModel : MvxViewModel
    {
        #region Private fields

        private BaseProfileCommentViewModel _comment;
        private readonly IMvxNavigationService _navigationService;

        #endregion

        public UserCommentItemViewModel(IMvxNavigationService navigationService, BaseProfileCommentViewModel comment)
        {
            _navigationService = navigationService;
            Comment = comment;
        }

        #region Properties

        public BaseProfileCommentViewModel Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        public List<ITransformation> Transformations => new List<ITransformation>
        {
            new CircleTransformation()
        };

        #endregion

        #region Commands

        public IMvxAsyncCommand OpenProfileCommand => new MvxAsyncCommand(OpenProfile);

        #endregion

        #region Methods

        private async Task OpenProfile()
        {
            if (Comment.UserCommentator.Equals(ApplicationData.CurrentUserViewModel))
                return;
            await _navigationService.Navigate<UserProfileViewModel, BaseUserViewModel>(Comment.UserCommentator);
        }

        #endregion
    }
}
