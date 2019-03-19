using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace LumoTrack.App.Android.Helpers
{
    public class InternetConnectionHelper
    {
        public bool IsInternetAvailable()
        {
            string CheckUrl = "http://google.com";

            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);

                iNetRequest.Timeout = 1000;

                WebResponse iNetResponse = iNetRequest.GetResponse();

                Console.WriteLine("...connection established..." + iNetRequest.ToString());
                iNetResponse.Close();

                return true;

            }
            catch (WebException ex)
            {

                Console.WriteLine(".....no connection..." + ex.ToString());

                return false;
            }
        }

    }
}