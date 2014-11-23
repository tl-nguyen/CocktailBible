using System;
using System.Collections.Generic;
using System.Text;
using Windows.Networking.Connectivity;

namespace CocktailBible.Utils
{
    public class StatusManager
    {
        public static bool CheckInternetConnection()
        {
            try
            {
                ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

                if (InternetConnectionProfile == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
