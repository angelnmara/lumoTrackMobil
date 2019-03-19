using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace LumoTrack.App.iOS.Helpers
{
    public class InternetConnectionHelper
    {


        public bool isInternetAviable()
        {
            string CheckUrl = "http://google.com";

            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);

                iNetRequest.Timeout = 1000;

                WebResponse iNetResponse = iNetRequest.GetResponse();

                 Console.WriteLine ("...connection established..." + iNetRequest.ToString ());
                iNetResponse.Close();

                return true;

            }
            catch (WebException ex)
            {

                 Console.WriteLine (".....no connection..." + ex.ToString ());

                return false;
            }
        }

    }
}
