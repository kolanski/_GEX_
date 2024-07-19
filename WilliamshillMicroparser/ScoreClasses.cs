using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WilliamshillMicroparser
{
    public class Updatedate
    {
        public string _doc { get; set; }
        public string time { get; set; }
        public string date { get; set; }
        public string tz { get; set; }
        public int tzoffset { get; set; }
        public int uts { get; set; }
    }

    public class Status
    {
        public string _doc { get; set; }
        public int _id { get; set; }
        public string name { get; set; }
    }

    public class Dt
    {
        public string _doc { get; set; }
        public string time { get; set; }
        public string date { get; set; }
        public string tz { get; set; }
        public int tzoffset { get; set; }
        public int uts { get; set; }
    }

    public class Roundname
    {
        public string _doc { get; set; }
        public int _id { get; set; }
        public object displaynumber { get; set; }
        public string name { get; set; }
        public string shortname { get; set; }
        public object cuproundnumber { get; set; }
        public int statisticssortorder { get; set; }
    }

    public class Coverage
    {
        public int lineup { get; set; }
        public int formations { get; set; }
        public int livetable { get; set; }
        public int injuries { get; set; }
        public bool ballspotting { get; set; }
        public bool cornersonly { get; set; }
        public bool multicast { get; set; }
        public int scoutmatch { get; set; }
        public int scoutcoveragestatus { get; set; }
        public bool scoutconnected { get; set; }
        public bool liveodds { get; set; }
        public bool deepercoverage { get; set; }
        public bool tacticallineup { get; set; }
        public bool basiclineup { get; set; }
        public bool hasstats { get; set; }
        public bool inlivescore { get; set; }
        public int? advantage { get; set; }
        public int? tiebreak { get; set; }
        public int penaltyshootout { get; set; }
        public bool scouttest { get; set; }
    }

    public class Result
    {
        public int home { get; set; }
        public int away { get; set; }
        public string winner { get; set; }
    }

    public class P1
    {
        public int home { get; set; }
        public int away { get; set; }
    }

    public class P2
    {
        public int home { get; set; }
        public int away { get; set; }
    }

    public class P3
    {
        public int home { get; set; }
        public int away { get; set; }
    }

    public class Periods
    {
        public P1 p1 { get; set; }
        public P2 p2 { get; set; }
        //public P3 p3 { get; set; }
    }

    public class Suspensions
    {
        public List<string> away { get; set; }
    }

    public class Suspensionsgiven
    {
        public List<string> away { get; set; }
    }

    public class Timeinfo
    {
        public object injurytime { get; set; }
        public object ended { get; set; }
        public string started { get; set; }
        public string played { get; set; }
        public string remaining { get; set; }
        public bool running { get; set; }
        public Suspensions suspensions { get; set; }
        public Suspensionsgiven suspensionsgiven { get; set; }
        public List<object> suspensionplayers { get; set; }
    }

    public class Cc
    {
        public string _doc { get; set; }
        public int _id { get; set; }
        public string a2 { get; set; }
        public string name { get; set; }
        public string a3 { get; set; }
        public string ioc { get; set; }
        public int continentid { get; set; }
        public string continent { get; set; }
        public int? population { get; set; }
    }

    public class Cc2
    {
        public string _doc { get; set; }
        public int _id { get; set; }
        public string a2 { get; set; }
        public string name { get; set; }
        public string a3 { get; set; }
        public string ioc { get; set; }
        public int continentid { get; set; }
        public string continent { get; set; }
        public int? population { get; set; }
    }

    public class Child
    {
        public string _doc { get; set; }
        public int _id { get; set; }
        public int uid { get; set; }
        public bool @virtual { get; set; }
        public string name { get; set; }
        public string mediumname { get; set; }
        public string abbr { get; set; }
        public bool iscountry { get; set; }
        public Cc2 cc { get; set; }
    }

    public class Home
    {
        public string _doc { get; set; }
        public int _id { get; set; }
        public int uid { get; set; }
        public bool @virtual { get; set; }
        public string name { get; set; }
        public string mediumname { get; set; }
        public string abbr { get; set; }
        public bool iscountry { get; set; }
        public Cc cc { get; set; }
        public List<Child> children { get; set; }
    }

    public class Cc3
    {
        public string _doc { get; set; }
        public int _id { get; set; }
        public string a2 { get; set; }
        public string name { get; set; }
        public string a3 { get; set; }
        public string ioc { get; set; }
        public int continentid { get; set; }
        public string continent { get; set; }
        public int? population { get; set; }
    }

    public class Cc4
    {
        public string _doc { get; set; }
        public int _id { get; set; }
        public string a2 { get; set; }
        public string name { get; set; }
        public string a3 { get; set; }
        public string ioc { get; set; }
        public int continentid { get; set; }
        public string continent { get; set; }
        public int? population { get; set; }
    }

    public class Child2
    {
        public string _doc { get; set; }
        public int _id { get; set; }
        public int uid { get; set; }
        public bool @virtual { get; set; }
        public string name { get; set; }
        public string mediumname { get; set; }
        public string abbr { get; set; }
        public bool iscountry { get; set; }
        public Cc4 cc { get; set; }
    }

    public class Away
    {
        public string _doc { get; set; }
        public int _id { get; set; }
        public int uid { get; set; }
        public bool @virtual { get; set; }
        public string name { get; set; }
        public string mediumname { get; set; }
        public string abbr { get; set; }
        public bool iscountry { get; set; }
        public Cc3 cc { get; set; }
        public List<Child2> children { get; set; }
    }

    public class Teams
    {
        public Home home { get; set; }
        public Away away { get; set; }
    }

    public class Status2
    {
        public string _doc { get; set; }
        public int _id { get; set; }
        public string name { get; set; }
    }

    public class Gamescore
    {
        public int home { get; set; }
        public int away { get; set; }
        public string service { get; set; }
    }

    public class Court
    {
        public string _doc { get; set; }
        public int _id { get; set; }
        public string name { get; set; }
    }

    public class P12
    {
        public int home { get; set; }
        public int away { get; set; }
    }

    public class Tiebreaks
    {
        public P12 p1 { get; set; }
    }

    public class Match
    {
        public string _doc { get; set; }
        public int _id { get; set; }
        public int _sid { get; set; }
        public int _seasonid { get; set; }
        public int _rcid { get; set; }
        public int _tid { get; set; }
        public int _utid { get; set; }
        public Dt _dt { get; set; }
        public Roundname roundname { get; set; }
        public int week { get; set; }
        public Coverage coverage { get; set; }
        public Result result { get; set; }
        public Periods periods { get; set; }
        public int updated_uts { get; set; }
        public string p { get; set; }
        public int ptime { get; set; }
        public Timeinfo timeinfo { get; set; }
        public Teams teams { get; set; }
        public Status2 status { get; set; }
        public bool removed { get; set; }
        public bool facts { get; set; }
        public int stadiumid { get; set; }
        public bool localderby { get; set; }
        public int? weather { get; set; }
        public object pitchcondition { get; set; }
        public object temperature { get; set; }
        public object wind { get; set; }
        public int windadvantage { get; set; }
        public string matchstatus { get; set; }
        public double hf { get; set; }
        public int? periodlength { get; set; }
        public int? numberofperiods { get; set; }
        public Gamescore gamescore { get; set; }
        public object penaltyshootout { get; set; }
        public Court court { get; set; }
        public Tiebreaks tiebreaks { get; set; }
    }

    public class Periodscore
    {
        public int home { get; set; }
        public int away { get; set; }
    }

    public class Result2
    {
        public int home { get; set; }
        public int away { get; set; }
        public object winner { get; set; }
    }

    public class MatchStatus
    {
        public string _doc { get; set; }
        public int _id { get; set; }
        public string name { get; set; }
    }

    public class SetScore
    {
        public string away { get; set; }
        public string home { get; set; }
    }

    public class GameScore2
    {
        public string away { get; set; }
        public string home { get; set; }
    }

    public class GamePoints
    {
        public string away { get; set; }
        public string home { get; set; }
    }

    public class Scorer
    {
        public string name { get; set; }
    }

    public class Datum
    {
        public string _doc { get; set; }
        public int _id { get; set; }
        public string _scoutid { get; set; }
        public int _sid { get; set; }
        public int _rcid { get; set; }
        public int _tid { get; set; }
        public bool _dc { get; set; }
        public string _typeid { get; set; }
        public int uts { get; set; }
        public int updated_uts { get; set; }
        public string type { get; set; }
        public int matchid { get; set; }
        public int disabled { get; set; }
        public int time { get; set; }
        public int seconds { get; set; }
        public string name { get; set; }
        public int? injurytime { get; set; }
        public Updatedate updatedate { get; set; }
        public int period { get; set; }
        public Status status { get; set; }
        public Match match { get; set; }
        public string team { get; set; }
        public Periodscore periodscore { get; set; }
        public Result2 result { get; set; }
        public MatchStatus matchStatus { get; set; }
        public int? periodnumber { get; set; }
        public string pointtype { get; set; }
        public string pointflag { get; set; }
        public string pointflagtranslation { get; set; }
        public string faulttype { get; set; }
        public SetScore set_score { get; set; }
        public GameScore2 game_score { get; set; }
        public GamePoints game_points { get; set; }
        public string service { get; set; }
        public string positionid { get; set; }
        public object goaltypeid { get; set; }
        public Scorer scorer { get; set; }
        public bool? header { get; set; }
        public bool? owngoal { get; set; }
        public bool? penalty { get; set; }
    }

    public class Doc
    {
        public string @event { get; set; }
        public int _dob { get; set; }
        public int _maxage { get; set; }
        public bool _synced { get; set; }
        public List<Datum> data { get; set; }
    }

    public class ScoreClass
    {
        public string queryUrl { get; set; }
        public List<Doc> doc { get; set; }
    }
}
