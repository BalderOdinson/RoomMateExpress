using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.Models;

namespace RoomMateExpress.Core.ViewModels.BaseViewModels
{
    public class BaseNeighborhoodViewModel : BaseViewModel
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public BaseNeighborhoodViewModel()
        {
            
        }

        public BaseNeighborhoodViewModel(Neighborhood neighborhood) : base(neighborhood.Id)
        {
            Mapper.Map(neighborhood, this);
        }
    }
}
