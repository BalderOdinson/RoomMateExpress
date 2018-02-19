using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomMateExpressWebApi.Models.AccountViewModels
{
    public class ConfirmationViewModel
    {
        public ConfirmationViewModel(string viewTitle, string title, string msg)
        {
            Title = title;
            Message = msg;
            ViewTitle = viewTitle;
        }

        public string ViewTitle { get; }
        public string Title { get; }
        public string Message { get; }
    }
}
