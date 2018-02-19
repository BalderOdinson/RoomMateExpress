using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Models;
using RoomMateExpress.Core.Services;

namespace RoomMateExpress.Core.ViewModels.BaseViewModels
{
    public class BaseAdminViewModel : BaseViewModel
    {
        private string _firstName;
        private string _lastName;

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public BaseAdminViewModel()
        {
            
        }

        public BaseAdminViewModel(Admin admin) : base(admin.Id)
        {
            Mapper.Map(admin, this);
        }

    }
}
