using Newtonsoft.Json;
using RawBot.Exceptions;
using RawBot.State.Model.Auth;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RawBot.Authentication
{
    public class Authenticator
    {
        private readonly HttpClient _http = new();
        private readonly Random _random = new();
        private readonly string _url;

        public Authenticator(string url, string userAgent)
        {
            _url = url;
            _http.DefaultRequestHeaders.Add("artixmode", "launcher");
            _http.DefaultRequestHeaders.Add("User-Agent", userAgent);
        }

        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            var formData = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "user", username },
                { "pass", password },
                { "option", "1" }
            });
            var response = await _http.PostAsync(_url + "?ran=" + _random.NextDouble(), formData);
            if (!response.IsSuccessStatusCode)
            {
                throw new AuthenticationException("Failed to authenticate user.");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LoginResponse>(content);
        }
    }
}
