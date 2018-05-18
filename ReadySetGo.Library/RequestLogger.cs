using System;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ReadySetGo.Library.DataContracts;

namespace ReadySetGo.Library
{
    public class RequestLogger : IRequestLogger
    {
        private readonly IOptions<SetlistConfig> _config;

        public RequestLogger(IOptions<SetlistConfig> config)
        {
            _config = config;
        }
        
        public void LogSetlist(PlaylistResult playlistResult)
        {
            var client = new AmazonDynamoDBClient(_config.Value.AccessKey, _config.Value.AccessKeySecret, RegionEndpoint.USEast1);
            var table = Table.LoadTable(client, "ReadySetGoSetlist");
            var jsonText = JsonConvert.SerializeObject(playlistResult);
            var item = Document.FromJson(jsonText);

            var result = table.PutItemAsync(item).Result;
        }

        public void LogSpotify(SpotifyUser user, PlaylistResult playlistResult, TokenResponse token)
        {
            var combinedLog = new CombinedLog { User = user, Playlist = playlistResult, Token = token };

            var client = new AmazonDynamoDBClient(_config.Value.AccessKey, _config.Value.AccessKeySecret, RegionEndpoint.USEast1);
            var table = Table.LoadTable(client, "ReadySetGoSpotify");
            var jsonText = JsonConvert.SerializeObject(combinedLog);
            var item = Document.FromJson(jsonText);

            var result = table.PutItemAsync(item).Result;
        }
    }

    public class CombinedLog
    {
        public string UserId
        {
            get { return User.Id; }
        }

        public DateTime RequestDateTime
        {
            get { return DateTime.Now; }
        }

        public SpotifyUser User { get; set; }
        public PlaylistResult Playlist { get; set; }
        public TokenResponse Token { get; set; }
    }
}
