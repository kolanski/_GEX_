using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using MicroparserFramework;
using System.Web.Script.Serialization;

namespace WilliamshillMicroparser
{
    public partial class Form1 : Form
    {

        List<RootObject> jsons = new List<RootObject>();
        williamsSimple ss;

        public Form1()
        {
            InitializeComponent();
            ss= new williamsSimple(richTextBox1);
            ServicePointManager.DefaultConnectionLimit = 1000;
            ss.richTextBox1 = richTextBox1;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            ss.loadmatches();
        }
        private void button2_Click(object sender, EventArgs e)
        {
           ss.loadgames();
        }
        public class Sport
        {
            public string sport_id { get; set; }
            public string sport_name { get; set; }
            public string prematch_disporder { get; set; }
            public string inplay_disporder { get; set; }
        }

        public class Type
        {
            public string type_id { get; set; }
            public string type_name { get; set; }
            public string disporder { get; set; }
        }

        public class ComClock
        {
            public string state { get; set; }
            public string period { get; set; }
            public string time { get; set; }
        }

        public class ComScore
        {
            public string score_1 { get; set; }
            public string score_2 { get; set; }
            public string comp1_score { get; set; }
            public string comp2_score { get; set; }
        }

        public class Collection
        {
            public string collection_id { get; set; }
            public string expanded { get; set; }
            public string name { get; set; }
            public string num_disp_mkt { get; set; }
            public string primary { get; set; }
        }

        public class Market
        {
            public string ev_mkt_id { get; set; }
            public string ev_oc_grp_id { get; set; }
            public string mkt_sort { get; set; }
            public string status { get; set; }
            public string mkt_name { get; set; }
            public string master_mkt_name { get; set; }
            public string grp_id { get; set; }
            public string displayed { get; set; }
            public string preloaded { get; set; }
            public string disporder { get; set; }
            public string expanded { get; set; }
            public string bir_index { get; set; }
            public string ew_avail { get; set; }
            public string ew_places { get; set; }
            public string ew_fac_num { get; set; }
            public string ew_fac_den { get; set; }
            public string hcap_value { get; set; }
            public string blurb { get; set; }
            public string col_id { get; set; }
            public string col_expanded { get; set; }
            public string col_disporder { get; set; }
            public string col_name { get; set; }
            public string bet_in_run { get; set; }
            public string last_msg_id { get; set; }
            public string template_name { get; set; }
            public string template_grp_id { get; set; }
            public string template_grp_col { get; set; }
            public string template_num_col { get; set; }
            public string template_col_header { get; set; }
            public string disp_sort { get; set; }
            public string tooltip_visible { get; set; }
            public string hovertext { get; set; }
        }

        public class Selection
        {
            public string ev_oc_id { get; set; }
            public string ev_mkt_id { get; set; }
            public string lp_num { get; set; }
            public string lp_den { get; set; }
            public string ilp_avail { get; set; }
            public string mkt_bir_index { get; set; }
            public string mkt_ew_avail { get; set; }
            public string mkt_ew_places { get; set; }
            public string mkt_ew_fac_num { get; set; }
            public string mkt_ew_fac_den { get; set; }
            public string mkt_hcap_value { get; set; }
            public string fb_result { get; set; }
            public string status { get; set; }
            public string result { get; set; }
            public string hcap_spread { get; set; }
            public string cs_home { get; set; }
            public string cs_away { get; set; }
            public string displayed { get; set; }
            public string raw_desc { get; set; }
            public string disporder { get; set; }
            public string name { get; set; }
        }

        public class RootObject
        {
            public Sport sport { get; set; }
            public Type type { get; set; }
            public string disporder { get; set; }
            public string @event { get; set; }
            public string event_link { get; set; }
            public string status { get; set; }
            public string raw_primary { get; set; }
            public string is_us_format { get; set; }
            public string start_time { get; set; }
            public string offset { get; set; }
            public string secs_to_start_time { get; set; }
            public string suspend_at { get; set; }
            public string has_video { get; set; }
            public string has_stats { get; set; }
            public string is_off { get; set; }
            public string is_running { get; set; }
            public string mkt_display_count { get; set; }
            public string name { get; set; }
            public string lang { get; set; }
            public string last_msg_id { get; set; }
            public string com_last_msg_id { get; set; }
            public string disp_perd_code { get; set; }
            public ComClock com_clock { get; set; }
            public ComScore com_score { get; set; }
            public List<Collection> collections { get; set; }
            public List<Market> markets { get; set; }
            public List<object> cast_markets { get; set; }
            public List<Selection> selections { get; set; }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {

            }
            else
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            //ss.loadscores();
            Phantomjs ph = new Phantomjs();
            richTextBox1.Text= ph.Grab("http://google.com");
            //richTextBox1.AppendText("done");
            //richTextBox1.Text = ph.Title();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
