using System;

namespace RoomMateExpress.Core.Helpers.Enums
{
    public class ResourceKeyAttribute : Attribute
    {
        private string _key;

        public string Key
        {
            get => _key;
            set => _key = value;
        }
    }
}
