using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Helpers.Enums;

namespace RoomMateExpress.Core.ViewModels
{
    public class DatePickerViewModel : MvxViewModel<DatePickerOptions, DateTime>
    {
        #region Private fields

        private readonly IMvxNavigationService _navigationService;
        private DateTime _minDate;
        private DateTime _maxDate;
        private DateTime _selectedDate;

        #endregion

        public DatePickerViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #region Overrided methods

        public override void Prepare(DatePickerOptions parameter)
        {
            MinDate = parameter.MinDate;
            MaxDate = parameter.MaxDate;
            SelectedDate = parameter.SelectedDate;
        }

        #endregion

        #region Properties

        public DateTime MinDate
        {
            get => _minDate;
            set => SetProperty(ref _minDate, value);
        }

        public DateTime MaxDate
        {
            get => _maxDate;
            set => SetProperty(ref _maxDate, value);
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        #endregion

        #region Commands

        public IMvxAsyncCommand AcceptCommand => new MvxAsyncCommand(Accept);

        #endregion

        #region Methods

        private async Task Accept()
        {
            await _navigationService.Close(this, SelectedDate);
        }

        #endregion

    }
}
