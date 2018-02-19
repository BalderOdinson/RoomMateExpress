using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Models;

namespace RoomMateExpress.Core.ViewModels.BaseViewModels
{
    public class BaseCityViewModel : BaseViewModel
    {
        private MvxObservableCollection<BaseNeighborhoodViewModel> _neighborhoods;
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public MvxObservableCollection<BaseNeighborhoodViewModel> Neighborhoods
        {
            get => _neighborhoods;
            set => SetProperty(ref _neighborhoods, value);
        }

        public BaseCityViewModel()
        {
            Neighborhoods = new MvxObservableCollection<BaseNeighborhoodViewModel>();
        }

        public BaseCityViewModel(City city) : base(city.Id)
        {
            Mapper.Map(city, this);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
