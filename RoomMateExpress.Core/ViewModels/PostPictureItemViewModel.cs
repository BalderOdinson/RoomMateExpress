using System;
using System.Collections.Generic;
using System.Text;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class PostPictureItemViewModel : MvxViewModel
    {
        private BasePostPictureViewModel _postPicture;
        private bool _isChecked;
        
        public PostPictureItemViewModel(BasePostPictureViewModel postPicture)
        {
            PostPicture = postPicture;
        }

        public BasePostPictureViewModel PostPicture
        {
            get => _postPicture;
            set => SetProperty(ref _postPicture, value);
        }

        public bool IsChecked
        {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }

        public List<ITransformation> Transformations => new List<ITransformation>
        {
            new RoundedTransformation()
        };

        public IMvxCommand CheckCommand => new MvxCommand(() => IsChecked = !IsChecked);

    }
}
