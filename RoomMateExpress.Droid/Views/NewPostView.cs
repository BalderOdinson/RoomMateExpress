using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using RoomMateExpress.Core.ViewModels;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace RoomMateExpress.Droid.Views
{

    [Activity(
        Theme = "@style/RoomMateExpressTheme")]
    public class NewPostView : MvxAppCompatActivity<NewPostViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.new_post_view);

            var pictureRecyclerView = FindViewById<MvxRecyclerView>(Resource.Id.pictureRecyclerView);
            if(pictureRecyclerView != null)
            {
                pictureRecyclerView.HasFixedSize = true;
                var layoutManager1 = new GridLayoutManager(this, 3);
                pictureRecyclerView.SetLayoutManager(layoutManager1);
                pictureRecyclerView.HasFixedSize = true;                
            }

            var neighborhoodRecycler = FindViewById<MvxRecyclerView>(Resource.Id.neighborhoodRecyclerView);
            if(neighborhoodRecycler != null)
            {
                var layoutManager = new LinearLayoutManager(this);
                neighborhoodRecycler.SetLayoutManager(layoutManager);
            }
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            if (toolbar == null) return;
            toolbar.InflateMenu(Resource.Menu.new_post_menu);
            toolbar.MenuItemClick += async (sender, args) =>
            {
                if (args.Item.ItemId == Resource.Id.makePost)
                {
                    await ViewModel.CreatePostCommand.ExecuteAsync();
                }
            };
            toolbar.NavigationClick += (sender, args) =>
            {
                OnBackPressed();
            };

        }
    }
}