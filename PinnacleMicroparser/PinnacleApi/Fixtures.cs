using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinnacleMicroparser.PinnacleApi.Fixtures
{
    public class Event
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("starts")]
        public DateTime Starts { get; set; }

        [JsonProperty("home")]
        public string Home { get; set; }

        [JsonProperty("away")]
        public string Away { get; set; }

        [JsonProperty("rotNum")]
        public string RotNum { get; set; }

        [JsonProperty("liveStatus")]
        public int LiveStatus { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("parlayRestriction")]
        public int ParlayRestriction { get; set; }
    }

    public class League
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("events")]
        public IList<Event> Events { get; set; }
    }

    public class Fixtures
    {

        [JsonProperty("sportId")]
        public int SportId { get; set; }

        [JsonProperty("last")]
        public int Last { get; set; }

        [JsonProperty("league")]
        public IList<League> League { get; set; }
    }
}
