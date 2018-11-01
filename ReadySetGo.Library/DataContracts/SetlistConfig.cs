using System;

namespace ReadySetGo.Library.DataContracts
{
    public class SetlistConfig
    {
        public string ClientId 
        {
            get
            {
                return Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID");
            }
        }

        public string ClientSecret
        {
            get
            {
                return Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_SECRET");
            }
        }
    }
}