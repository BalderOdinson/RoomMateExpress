using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Services;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class PostViewModel : MvxViewModel<BasePostViewModel>
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private readonly IPostService _postService;
        private BasePostViewModel _post;

        #endregion

        public PostViewModel(IMvxNavigationService navigationService, IPostService postService)
        {
            _navigationService = navigationService;
            _postService = postService;
        }

        #region Overrided methods

        public override void Prepare(BasePostViewModel parameter)
        {
            Post = parameter;
        }

        #endregion

        #region Properties

        public BasePostViewModel Post
        {
            get => _post;
            set => SetProperty(ref _post, value);
        }

        public string Neighborhoods => string.Join(", ", Post.Neighborhoods.Select(n => n.Name));

        public List<ITransformation> Transformations => new List<ITransformation>
        {
            new CircleTransformation()
        };
        #endregion

        #region Commands

        public IMvxAsyncCommand LikeCommand => new MvxAsyncCommand(Like);

        public IMvxAsyncCommand OpenCommentsCommand => new MvxAsyncCommand(OpenComments);

        #endregion

        #region Methods

        private async Task Like()
        {
            await ApiRequestHelper.HandleApiResult(() => _postService.LikeOrDislike(_post.Id),
                model => Mapper.Map(model, Post));
        }

        private async Task OpenComments()
        {
            await _navigationService.Navigate<PostCommentsViewModel,BasePostViewModel>(Post);
        }

        #endregion
    }
}
