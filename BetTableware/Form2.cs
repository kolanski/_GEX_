using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace BetTableware
{
    public partial class Form2 : Form
    {
        public int result=2;
        public bool isChecked;
        public bool isChecked1;
        public bool isChecked2;
        
        public Form2()
        {

            InitializeComponent();
            comboBox1.Text = comboBox1.Items[1].ToString();
            textBox1.Text = Program.getref().ToString();
            checkBox1.Checked = true;
            loadsettings();
            radioButton1.Click += new EventHandler(radioButton1_Click);
            radioButton2.Click += new EventHandler(radioButton2_Click);
            radioButton3.Click += new EventHandler(radioButton3_Click);
            
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            e.Cancel = true;
            this.Hide();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out result))
            {
                Program.setref(result);
            }
            else
            {
                MessageBox.Show("12!", "Ты человек даун?Здесь должна быть цифра!",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            }
        }
        public bool getText()
        {
            return checkBox6.Checked;
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }
        private void Form2_Shown(object sender, EventArgs e)
        {
            textBox1.Text = Program.getref().ToString();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Program.setplaysound(checkBox1.Checked);
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("sasddsa");
            //if (radioButton1.Checked)
            //    radioButton1.Checked = false;
            //else
            //{
            //    radioButton1.Checked = true;
            //}
        }
        private void radioButton2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("sasddsa");
            //if (radioButton2.Checked )
            //    radioButton2.Checked = false;
            //else
            //{
            //    radioButton2.Checked = true;
                
            //}
        }
        private void radioButton3_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("sasddsa");
            //if (radioButton3.Checked)
            //    radioButton3.Checked = false;
            //else
            //{
            //    radioButton3.Checked = true;
                
            //}
        }
        public bool isBetcity()
        {
            return radioButton1.Checked;
        }
        public bool isFonbet()
        {
            return radioButton2.Checked;
        }
        public bool isMarafon()
        {
            return radioButton3.Checked;
        }
        public bool isZenit()
        {
            return checkBox4.Checked;
        }
        public bool debug()
        {
            return checkBox2.Checked;
        }
        public bool hardtest()
        {
            return checkBox3.Checked;
        }
        public bool parsecurr()
        {
            return checkBox5.Checked;
        }
        public float minproc()
        {
            double lol = decimal.ToDouble(numericUpDown1.Value);
            float myFloat = (float)lol;
            return myFloat * 0.01f;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //isChecked = radioButton1.Checked;

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            //isChecked1 = radioButton2.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            //isChecked2 = radioButton3.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            Program.MainForm.navigatewb3();
            await Task.Delay(3000);
            Program.MainForm.refwb3();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.MainForm.navigatewb1();
        }
        public string banned()
        {
            return textBox2.Text;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Сохранее настроек будет в будующей версии.");
            var dir= Directory.GetCurrentDirectory();
            string path =Path.Combine(dir, "settings.txt");
            try
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                    FileStream fcreate = File.Open(path, FileMode.Create); StreamWriter x = new StreamWriter(fcreate);
                    x.WriteLine(isBetcity());
                    x.WriteLine(isFonbet());
                    x.WriteLine(isMarafon());
                    x.WriteLine(isZenit());
                    x.WriteLine(getSound());
                    x.WriteLine(numericUpDown1.Value);
                    x.WriteLine(debug());
                    x.WriteLine(textBox1.Text);
                    x.WriteLine(checkBox3.Checked);
                    x.WriteLine(checkBox6.Checked);
                    x.WriteLine(ActiveCheck.Checked);
                    x.WriteLine(textBoxBCSRC.Text);
                    x.WriteLine(textBoxFBSRC.Text);
                    x.WriteLine(textBoxMRSRC.Text);
                    x.WriteLine(textBoxOLSRC.Text);
                    x.WriteLine(textBoxZNSRC.Text);
                    x.Close();
                }
                else if (File.Exists(path))
                {
                    FileStream fcreate = File.Open(path, FileMode.Create); StreamWriter x = new StreamWriter(fcreate);
                    x.WriteLine(isBetcity());
                    x.WriteLine(isFonbet());
                    x.WriteLine(isMarafon());
                    x.WriteLine(isZenit());
                    x.WriteLine(getSound());
                    x.WriteLine(numericUpDown1.Value);
                    x.WriteLine(debug());
                    x.WriteLine(textBox1.Text);
                    x.WriteLine(checkBox3.Checked);
                    x.WriteLine(checkBox6.Checked);
                    x.WriteLine(ActiveCheck.Checked);
                    x.WriteLine(textBoxBCSRC.Text);
                    x.WriteLine(textBoxFBSRC.Text);
                    x.WriteLine(textBoxMRSRC.Text);
                    x.WriteLine(textBoxOLSRC.Text);
                    x.WriteLine(textBoxZNSRC.Text);
                    x.Close();
                    
                }
            }
            catch
            {
            }
        }
        private void loadsettings()
        {
            var dir = Directory.GetCurrentDirectory();
            string path = Path.Combine(dir, "settings.txt");
            try
            {
                FileInfo fi1 = new FileInfo(path);

                if (!fi1.Exists)
                {
                    //Create a file to write to.
                    using (StreamWriter xx = fi1.CreateText())
                    {
                       xx.WriteLine(isBetcity());
                       xx.WriteLine(isFonbet());
                       xx.WriteLine(isMarafon());
                       xx.WriteLine(isZenit());
                       xx.WriteLine(getSound());
                       xx.WriteLine(numericUpDown1.Value);
                       xx.WriteLine(debug());
                       xx.WriteLine(textBox1.Text);
                       xx.WriteLine(checkBox3.Checked);
                       xx.WriteLine(checkBox6.Checked);
                       xx.WriteLine(ActiveCheck.Checked);
                       xx.WriteLine(textBoxBCSRC.Text);
                       xx.WriteLine(textBoxFBSRC.Text);
                       xx.WriteLine(textBoxMRSRC.Text);
                       xx.WriteLine(textBoxOLSRC.Text);
                       xx.WriteLine(textBoxZNSRC.Text);
                        xx.Close();
                    }
                }
                FileStream fcreate = File.Open(path, FileMode.Open);
                StreamReader x = new StreamReader(fcreate);
                radioButton1.Checked = bool.Parse(x.ReadLine());
                radioButton2.Checked = bool.Parse(x.ReadLine());
                radioButton3.Checked = bool.Parse(x.ReadLine());
                checkBox4.Checked = bool.Parse(x.ReadLine());
                comboBox1.Text = x.ReadLine();
                numericUpDown1.Value = decimal.Parse(x.ReadLine());
                checkBox2.Checked = bool.Parse(x.ReadLine());
                textBox1.Text = x.ReadLine();
                checkBox3.Checked = bool.Parse(x.ReadLine());
                checkBox6.Checked = bool.Parse(x.ReadLine());
                ActiveCheck.Checked = bool.Parse(x.ReadLine());
                textBoxBCSRC.Text = x.ReadLine();
                textBoxFBSRC.Text = x.ReadLine();
                textBoxMRSRC.Text = x.ReadLine();
                textBoxOLSRC.Text = x.ReadLine();
                textBoxZNSRC.Text = x.ReadLine();
                try
                {
                    if (int.TryParse(textBox1.Text, out result))
                    {
                        Program.setref(result);
                    }
                    
                }
                catch
                {
                    //Program.setref(5);
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        public string getSound()
        {
           return comboBox1.GetItemText(comboBox1.GetItemText(comboBox1.SelectedItem).ToString());
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Program.MainForm.openmf();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        public bool getactivegaming()
        {
            return ActiveCheck.Checked;
        }
        private void ActiveCheck_CheckedChanged(object sender, EventArgs e)
        {

        }


    }
}
