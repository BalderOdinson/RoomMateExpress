using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using RoomMateExpress.Core.ViewModels.BaseViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class NeighboorhoodItemViewModel : MvxViewModel
    {
        #region Private fields

        private BaseNeighborhoodViewModel _neighborhood;
        private BasePostViewModel _post;

        #endregion

        public NeighboorhoodItemViewModel(BaseNeighborhoodViewModel neighborhood, BasePostViewModel post)
        {
            Neighborhood = neighborhood;
            Post = post;
        }

        #region Properties

        public BasePostViewModel Post
        {
            get => _post;
            set => SetProperty(ref _post, value);
        }

        public BaseNeighborhoodViewModel Neighborhood
        {
            get => _neighborhood;
            set => SetProperty(ref _neighborhood, value);
        }

        public bool NeighborhoodChecked
        {
            get => Post.Neighborhoods.Contains(Neighborhood);
            set
            {
                if (value) Post.Neighborhoods.Add(Neighborhood);
                else Post.Neighborhoods.Remove(Neighborhood);
                RaisePropertyChanged(nameof(NeighborhoodChecked));
            }
        }

        #endregion
    }
}
