using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace RoomMateExpress.Core.ViewModels
{
    public class BusyViewModel : MvxViewModel<string>
    {
        private string _label;

        public override void Prepare(string parameter)
        {
            Label = parameter;
        }

        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }
    }
}
