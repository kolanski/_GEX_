using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PinnacleMicroparser.PinnacleApi
{
    public class ClientBalance
    {
        [JsonProperty(PropertyName = "availableBalance")]
        public decimal AvailableBalance;

        [JsonProperty(PropertyName = "outstandingTransactions")]
        public decimal OutstandingTransactions;

        [JsonProperty(PropertyName = "givenCredit")]
        public decimal GivenCredit;

        [JsonProperty(PropertyName = "currency")]
        public string Currency;
    }
    public class Sport
    {

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("hasOfferings")]
        public bool hasOfferings { get; set; }

        [JsonProperty("leagueSpecialsCount")]
        public int leagueSpecialsCount { get; set; }

        [JsonProperty("eventSpecialsCount")]
        public int eventSpecialsCount { get; set; }

        [JsonProperty("eventCount")]
        public int eventCount { get; set; }
    }

    public class Sports
    {

        [JsonProperty("sports")]
        public IList<Sport> sports { get; set; }
    }
    public class PinnacleApi
    {
        private HttpClient _httpClient;

        private string _clientId;
        private string _password;

        public string CurrencyCode { get; private set; }

        private const int MinimumFeedRefreshWithLast = 5;   // minimum time in seconds between calls when supplying the last timestamp parameter
        private const int MinimumFeedRefresh = 60;          // minimum time in seconds between calls without last timestamp parameter

        private DateTime? _lastFeedRequest;

        private const string BaseAddress = "https://api.pinnaclesports.com/";

        public PinnacleApi(string clientId, string password, string currencyCode)
        {
            _clientId = clientId;
            _password = password;
            CurrencyCode = currencyCode;

            _httpClient = new HttpClient { BaseAddress = new Uri(BaseAddress) };

            // put auth header into httpclient
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(string.Format("{0}:{1}", _clientId, _password))));
        }
        protected async Task<T> GetJsonAsync<T>(string requestType, params object[] values)
        {
            var response = await _httpClient.GetAsync(string.Format(requestType, values)).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();         // throw if web request failed

            var json = await response.Content.ReadAsStringAsync();
            if (json.Contains("blocked"))
                throw new Exception("Blocked location by pinnacle");
            // deserialise json async
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(json));
        }
        public Task<ClientBalance> GetClientBalance()
        {
            const string uri = "v1/client/balance";
            return GetJsonAsync<ClientBalance>(uri);
        }
        public Task<Sports> GetSports()
        {
            const string uri = "v2/sports";
            return GetJsonAsync<Sports>(uri);
        }
        public Task<Odds> GetOddsForTennis()
        {
            const string uri = "v1/odds?sportid=33&islive=1&oddsFormat=DECIMAL";
            return GetJsonAsync<Odds>(uri);
        }
        public Task<PinnacleMicroparser.PinnacleApi.Fixtures.Fixtures> GetFixturesForTennis()
        {
            const string uri = "v1/fixtures?sportid=33&islive=1";
            return GetJsonAsync<PinnacleMicroparser.PinnacleApi.Fixtures.Fixtures>(uri);
        }
    }
}
