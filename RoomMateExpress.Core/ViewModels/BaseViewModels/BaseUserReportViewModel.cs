using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using RoomMateExpress.Core.Models;

namespace RoomMateExpress.Core.ViewModels.BaseViewModels
{
    public class BaseUserReportViewModel : BaseViewModel
    {
        private string _text;
        private BaseUserViewModel _userReporting;
        private BaseUserViewModel _userReported;
        private BaseAdminViewModel _admin;
        private DateTimeOffset _dateReporting;
        private DateTimeOffset? _dateProcessed;
        private string _adminDecision;

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public BaseUserViewModel UserReporting
        {
            get => _userReporting;
            set => SetProperty(ref _userReporting, value);
        }

        public BaseUserViewModel UserReported
        {
            get => _userReported;
            set => SetProperty(ref _userReported, value);
        }

        public BaseAdminViewModel Admin
        {
            get => _admin;
            set => SetProperty(ref _admin, value);
        }

        public DateTimeOffset DateReporting
        {
            get => _dateReporting;
            set => SetProperty(ref _dateReporting, value);
        }

        public DateTimeOffset? DateProcessed
        {
            get => _dateProcessed;
            set
            {
                SetProperty(ref _dateProcessed, value);
                RaisePropertyChanged(nameof(IsProcessed));
            }
        }

        public string AdminDecision
        {
            get => _adminDecision;
            set => SetProperty(ref _adminDecision, value);
        }

        public bool IsProcessed => DateProcessed.HasValue;

        public BaseUserReportViewModel()
        {

        }

        public BaseUserReportViewModel(UserReport userReport)
        {
            Mapper.Map(userReport, this);
        }
    }
}
