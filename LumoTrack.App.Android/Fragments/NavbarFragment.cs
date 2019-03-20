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

namespace LumoTrack.App.Android.Fragments
{
    public class NavbarFragment : Fragment
    {
        ImageView _logo;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.NavBarFragment, container);
            ImageView imageView = view.FindViewById<ImageView>(Resource.Id.mapcenterIcon);
            imageView.SetImageResource(Resource.Mipmap.TruckPin); 
            //_logo = view.FindViewById<ImageView>(Resource.Id.addcomment);

            return view;
        }

        public void SetIcon(bool visible)
        {
            if (visible)
                _logo.Visibility = ViewStates.Visible;
            else
                _logo.Visibility = ViewStates.Gone;
        }
    }
}