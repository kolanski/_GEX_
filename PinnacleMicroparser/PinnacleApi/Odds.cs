using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinnacleMicroparser.PinnacleApi
{
    public class Spread
    {

        [JsonProperty("hdp")]
        public double hdp { get; set; }

        [JsonProperty("home")]
        public float home { get; set; }

        [JsonProperty("away")]
        public float away { get; set; }

        [JsonProperty("altLineId")]
        public float? altLineId { get; set; }
    }

    public class Moneyline
    {

        [JsonProperty("home")]
        public float home { get; set; }

        [JsonProperty("away")]
        public float away { get; set; }

        [JsonProperty("draw")]
        public float draw { get; set; }
    }

    public class Total
    {

        [JsonProperty("pofloats")]
        public double pofloats { get; set; }

        [JsonProperty("over")]
        public float over { get; set; }

        [JsonProperty("under")]
        public float under { get; set; }

        [JsonProperty("altLineId")]
        public float? altLineId { get; set; }
    }

    public class Period
    {

        [JsonProperty("lineId")]
        public float lineId { get; set; }

        [JsonProperty("number")]
        public float number { get; set; }

        [JsonProperty("cutoff")]
        public DateTime cutoff { get; set; }

        [JsonProperty("maxSpread")]
        public float maxSpread { get; set; }

        [JsonProperty("maxMoneyline")]
        public float maxMoneyline { get; set; }

        [JsonProperty("maxTotal")]
        public float maxTotal { get; set; }

        [JsonProperty("spreads")]
        public IList<Spread> spreads { get; set; }

        [JsonProperty("moneyline")]
        public Moneyline moneyline { get; set; }

        [JsonProperty("totals")]
        public IList<Total> totals { get; set; }
    }

    public class Event
    {

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("awayScore")]
        public float awayScore { get; set; }

        [JsonProperty("homeScore")]
        public float homeScore { get; set; }

        [JsonProperty("awayRedCards")]
        public float awayRedCards { get; set; }

        [JsonProperty("homeRedCards")]
        public float homeRedCards { get; set; }

        [JsonProperty("periods")]
        public IList<Period> periods { get; set; }
    }

    public class League
    {

        [JsonProperty("id")]
        public float id { get; set; }

        [JsonProperty("events")]
        public IList<Event> events { get; set; }
    }

    public class Odds
    {

        [JsonProperty("sportId")]
        public float sportId { get; set; }

        [JsonProperty("last")]
        public float last { get; set; }

        [JsonProperty("leagues")]
        public IList<League> leagues { get; set; }
    }
}
