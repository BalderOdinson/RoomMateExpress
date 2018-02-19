using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using RoomMateExpress.Core.Models;

namespace RoomMateExpress.Core.ViewModels.BaseViewModels
{
    public class BasePostPictureViewModel : BaseViewModel
    {
        private string _pictureUrl;

        public string PictureUrl
        {
            get => _pictureUrl;
            set => SetProperty(ref _pictureUrl, value);
        }

        public BasePostPictureViewModel()
        {
            
        }

        public BasePostPictureViewModel(PostPicture postPicture) : base(postPicture.Id)
        {
            Mapper.Map(postPicture, this);
        }
    }
}
