using Newtonsoft.Json;
using System;

namespace RawBot.State.Model.Auth
{
    public class UserInfo
    {
        [JsonProperty("bSuccess")]
        public bool Success { get; set; }

        [JsonProperty("iAccess")]
        public int Access { get; set; }

        [JsonProperty("dCreated")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("dUpgExp")]
        public DateTime UpgradeExpiredDate { get; set; }

        [JsonProperty("iAge")]
        public int Age { get; set; }

        [JsonProperty("intHours")]
        public int Hours { get; set; }

        [JsonProperty("iUpg")]
        public int Upgrade { get; set; }

        [JsonProperty("iUpgDays")]
        public int UpgradeDays { get; set; }

        [JsonProperty("sMsg")]
        public string StatusMessage { get; set; }

        [JsonProperty("sToken")]
        public string Token { get; set; }

        [JsonProperty("strCountryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("strEmail")]
        public string Email { get; set; }

        [JsonProperty("unm")]
        public string Username { get; set; }

        [JsonProperty("userid")]
        public int Id { get; set; }
    }
}
