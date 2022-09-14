using System;
using System.Diagnostics;
using System.IO;
using Snovio.Objects;
using System.Threading.Tasks;
using Snovio.Structs;
using Flurl.Http;

namespace Snovio;

public class Authentication
{
    public string _clientId { get; set; }
        public string _clientSecret { get; set; }
        public string _token;
        public AccessToken _accessToken = new AccessToken();
        private AccessTokenStruct accessToken;
        private string SnovioApiUrl = "https://api.snov.io/";
        private string PathToken = String.Join(Directory.GetCurrentDirectory(), "Token");

        public Authentication(string clientId = null, string clientSecret = null, string accessToken = null)
        {
            if (clientId == null & clientSecret == null & accessToken == null)
            {
                Debug.WriteLine("Por favor, proporcione un access_token o client_id y client_secret keys.");
            }
            else
            {
                if (clientId != null & clientSecret != null & accessToken == null)
                {
                    accessToken = GetAccessToken(clientId,clientSecret).GetAwaiter().GetResult().access_token;
                }
            }
            this._clientId = clientId;
            this._clientSecret = clientSecret;
            this._token = accessToken;

        }

        public async Task<AccessTokenStruct> GetAccessToken(string clientId, string clientSecret)
        {
            _accessToken.validationDirectory();
            if (_accessToken.validationToken())
            {
                return _accessToken._token;
            }
            else
            {
                string authEndpoint = "oauth/access_token";
                var parametros = new
                {
                    grant_type = "client_credentials",
                    client_id = clientId,
                    client_secret = clientSecret
                };

                var response = await $"{SnovioApiUrl}v1/{authEndpoint}"
                    .PostJsonAsync(parametros).ReceiveJson<AccessTokenStruct>();
                
                _accessToken.saveToken(response);
                return response;
            }
            
        }
}