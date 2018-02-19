using System;
using Android.Content;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using Android.Gms.Common;
using Android.OS;
using Android.Util;
using Android.Widget;
using Firebase.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.Plugins.Visibility;
using MvvmCross.Droid.Views;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platform.Converters;
using RoomMateExpress.Droid.Converters;
using RoomMateExpress.Droid.Services;
using RoomMateExpress.Core.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;

namespace RoomMateExpress.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
            IsPlayServicesAvailable(applicationContext);
        }

        protected override IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = base.ValueConverterAssemblies.ToList();
                toReturn.Add(typeof(MvxVisibilityValueConverter).Assembly);
                toReturn.Add(typeof(MvxInvertedVisibilityValueConverter).Assembly);
                return toReturn;
            }
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MvxAppCompatViewPresenter(AndroidViewAssemblies);
        }

        protected override void InitializeFirstChance()
        {
            CreatableTypes().EndingWith("Service").AsInterfaces().RegisterAsLazySingleton();
            base.InitializeFirstChance();
        }

        public bool IsPlayServicesAvailable(Context applicationContext)
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(applicationContext);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Toast.MakeText(applicationContext, GoogleApiAvailability.Instance.GetErrorString(resultCode), ToastLength.Long).Show();
                {
                    Toast.MakeText(applicationContext, "This device is not supported", ToastLength.Long).Show();
                }
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
