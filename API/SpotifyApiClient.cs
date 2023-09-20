using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System.Collections.Generic;
using System.Threading;


namespace Ya_.SpoticApi
{
    internal class SpotifyApiClient
    {
        public async Task<SpotifyWebAPI> Authenticate()
        {
            var clientId = "499d07bdc822451e976aa8e5daa7ae4c";
            var clientSecret = "598673c29c77445aaf648a2f68473306";

            var auth = new ClientCredentialsAuth()
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };

            var response = await auth.DoAuthAsync();

            if (response is ClientCredentialsTokenResponse tokenResponse)
            {
                var spotify = new SpotifyWebAPI()
                {
                    AccessToken = tokenResponse.AccessToken,
                    TokenType = tokenResponse.TokenType,
                    UseAuth = true
                };

                return spotify;
            }
            else
            {
                throw new Exception("Ошибка аутентификации в Spotify API");
            }
        }
        public List<string> GetSongs()
        {
            var spotify = Authenticate().Result;

            // Получение песен из Spotify API
            var songs = new List<string>();

            var searchRequest = new SearchRequest()
            {
                Q = "your_query", // Ваш запрос для поиска песен
                Type = SearchRequest.Types.Track
            };

            var searchResult = spotify.SearchItems(searchRequest);

            foreach (var track in searchResult.Tracks.Items)
            {
                songs.Add(track.Name);
            }

            return songs;
        }
    }
}
