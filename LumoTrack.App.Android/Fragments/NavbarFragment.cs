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
using LumoTrack.App.Android.Helpers;

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

            /*  Realizar cambios dinamicos  */

            ImageView imageViewSigueTuCamion = view.FindViewById<ImageView>(Resource.Id.mapcenterIcon);
            //int idresource = Resources.GetIdentifier(Constants.SigueTuCamion.ToLower(), Constants.Mipmap, Application.Context.PackageName);
            imageViewSigueTuCamion.SetImageResource(Resources.GetIdentifier(Constants.SigueTuCamion.ToLower(), Constants.Mipmap, Application.Context.PackageName));

            /*Por municipio*/
            ImageView imageViewMunicipio = view.FindViewById<ImageView>(Resource.Id.imgMunicipio);
            imageViewMunicipio.SetImageResource(Resources.GetIdentifier(Constants.MunicipioImg.ToLower(), Constants.Mipmap, Application.Context.PackageName));

            /*  Realizar cambios dinamicos  */

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