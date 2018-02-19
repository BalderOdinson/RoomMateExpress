using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;
using RoomMateExpress.Core.ViewModels;

namespace RoomMateExpress.Droid.Views.Fragments
{
    [MvxDialogFragmentPresentation]
    [Register(nameof(ReportUserFragment))]
    public class ReportUserFragment : MvxDialogFragment<ReportUserViewModel>
    {
        public ReportUserFragment()
        {
            RetainInstance = true;
        }

        protected ReportUserFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            RetainInstance = true;
        }

        public override void OnStart()
        {
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            base.OnStart();
        }

        public override void OnDestroyView()
        {
            if (Dialog != null && RetainInstance)
            {
                Dialog.SetDismissMessage(null);
            }
            base.OnDestroyView();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.report_user_fragment, null);

            return view;
        }
    }
}