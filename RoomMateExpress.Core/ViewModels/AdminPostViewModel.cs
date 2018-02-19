using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFImageLoading.Work;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;
using FFImageLoading.Transformations;

namespace RoomMateExpress.Core.ViewModels
{
    public class AdminPostViewModel : MvxViewModel<BasePostViewModel, bool>
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private BasePostViewModel _post;

        #endregion

        public AdminPostViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #region Overrided methods

        public override void Prepare(BasePostViewModel parameter)
        {
            Post = parameter;
        }

        #endregion

        #region Properties
        
        public string Neighborhoods => string.Join(", ", Post.Neighborhoods.Select(n => n.Name));

        public BasePostViewModel Post
        {
            get => _post;
            set => SetProperty(ref _post, value);
        }

        public List<ITransformation> Transformations => new List<ITransformation>
        {
            new CircleTransformation()
        };

        #endregion

        #region Commands

        public IMvxAsyncCommand OpenCommentsCommand => new MvxAsyncCommand(OpenComments);

        public IMvxAsyncCommand RemovePostDialogCommand => new MvxAsyncCommand(RemovePostDialog);

        #endregion

        #region Methods

        private async Task OpenComments()
        {
            await _navigationService.Navigate<AdminPostCommentsViewModel, BasePostViewModel>(Post);
        }

        private async Task RemovePostDialog()
        {
            if(await _navigationService.Navigate<RemovePostViewModel, BasePostViewModel, bool>(Post))
                await _navigationService.Close(this, true);
        }

        #endregion
    }
}
