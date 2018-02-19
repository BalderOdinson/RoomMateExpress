using System;
using System.Collections.Generic;
using System.Text;

namespace RoomMateExpress.Core.Settings
{
    public class SearchHistory
    {
        public Guid Id { get; set; }
        public UserData User { get; set; }
        public string SearchText { get; set; }

        public SearchHistory()
        {
            Id = Guid.NewGuid();
        }
    }
}
