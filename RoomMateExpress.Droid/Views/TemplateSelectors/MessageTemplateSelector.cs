using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using RoomMateExpress.Core.Helpers;
using RoomMateExpress.Core.Settings;
using RoomMateExpress.Core.ViewModels;

namespace RoomMateExpress.Droid.Views.TemplateSelectors
{
    public class MessageTemplateSelector : MvxTemplateSelector<MessageItemViewModel>
    {
        public override int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        protected override int SelectItemViewType(MessageItemViewModel forItemObject)
        {
            return forItemObject.Message.UserSender.Equals(ApplicationData.CurrentUserViewModel)
                ? Resource.Layout.message_item_right
                : Resource.Layout.message_item_left;
        }
    }
}