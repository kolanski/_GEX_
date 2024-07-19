using System;
namespace ProjectGamb
{
    partial class FormSearch
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SearchParseAll = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.AutoButton = new System.Windows.Forms.Button();
            this.AutoIsOn = new System.Windows.Forms.CheckBox();
            this.ParseBooks = new System.Windows.Forms.Button();
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.richTextBox5 = new System.Windows.Forms.RichTextBox();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.BlockCheck = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.richTextBox7 = new System.Windows.Forms.RichTextBox();
            this.richTextBox6 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SearchParseAll
            // 
            this.SearchParseAll.Location = new System.Drawing.Point(12, 12);
            this.SearchParseAll.Name = "SearchParseAll";
            this.SearchParseAll.Size = new System.Drawing.Size(75, 23);
            this.SearchParseAll.TabIndex = 0;
            this.SearchParseAll.Text = "ParseAll";
            this.SearchParseAll.UseVisualStyleBackColor = true;
            this.SearchParseAll.Click += new System.EventHandler(this.SearchParseAll_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(3, 9);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(268, 395);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(270, 9);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(268, 395);
            this.richTextBox2.TabIndex = 2;
            this.richTextBox2.Text = "";
            // 
            // richTextBox3
            // 
            this.richTextBox3.Location = new System.Drawing.Point(544, 9);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(268, 398);
            this.richTextBox3.TabIndex = 3;
            this.richTextBox3.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(94, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AutoButton
            // 
            this.AutoButton.Location = new System.Drawing.Point(176, 12);
            this.AutoButton.Name = "AutoButton";
            this.AutoButton.Size = new System.Drawing.Size(75, 23);
            this.AutoButton.TabIndex = 5;
            this.AutoButton.Text = "Auto";
            this.AutoButton.UseVisualStyleBackColor = true;
            this.AutoButton.Click += new System.EventHandler(this.AutoButton_Click);
            // 
            // AutoIsOn
            // 
            this.AutoIsOn.AutoSize = true;
            this.AutoIsOn.Location = new System.Drawing.Point(338, 16);
            this.AutoIsOn.Name = "AutoIsOn";
            this.AutoIsOn.Size = new System.Drawing.Size(70, 17);
            this.AutoIsOn.TabIndex = 6;
            this.AutoIsOn.Text = "AutoIsOn";
            this.AutoIsOn.UseVisualStyleBackColor = true;
            // 
            // ParseBooks
            // 
            this.ParseBooks.Location = new System.Drawing.Point(257, 12);
            this.ParseBooks.Name = "ParseBooks";
            this.ParseBooks.Size = new System.Drawing.Size(75, 23);
            this.ParseBooks.TabIndex = 7;
            this.ParseBooks.Text = "ParseTest";
            this.ParseBooks.UseVisualStyleBackColor = true;
            this.ParseBooks.Click += new System.EventHandler(this.ParseBooks_Click);
            // 
            // richTextBox4
            // 
            this.richTextBox4.Location = new System.Drawing.Point(13, 489);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.Size = new System.Drawing.Size(816, 297);
            this.richTextBox4.TabIndex = 8;
            this.richTextBox4.Text = "";
            // 
            // richTextBox5
            // 
            this.richTextBox5.Location = new System.Drawing.Point(818, 9);
            this.richTextBox5.Name = "richTextBox5";
            this.richTextBox5.Size = new System.Drawing.Size(268, 398);
            this.richTextBox5.TabIndex = 9;
            this.richTextBox5.Text = "";
            // 
            // BlockCheck
            // 
            this.BlockCheck.AutoSize = true;
            this.BlockCheck.Location = new System.Drawing.Point(415, 16);
            this.BlockCheck.Name = "BlockCheck";
            this.BlockCheck.Size = new System.Drawing.Size(71, 17);
            this.BlockCheck.TabIndex = 10;
            this.BlockCheck.Text = "BlockBox";
            this.BlockCheck.UseVisualStyleBackColor = true;
            this.BlockCheck.CheckedChanged += new System.EventHandler(this.BlockCheck_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.richTextBox7);
            this.panel1.Controls.Add(this.richTextBox6);
            this.panel1.Controls.Add(this.richTextBox1);
            this.panel1.Controls.Add(this.richTextBox2);
            this.panel1.Controls.Add(this.richTextBox3);
            this.panel1.Controls.Add(this.richTextBox5);
            this.panel1.Location = new System.Drawing.Point(13, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1091, 410);
            this.panel1.TabIndex = 11;
            // 
            // richTextBox7
            // 
            this.richTextBox7.Location = new System.Drawing.Point(1366, 9);
            this.richTextBox7.Name = "richTextBox7";
            this.richTextBox7.Size = new System.Drawing.Size(268, 398);
            this.richTextBox7.TabIndex = 11;
            this.richTextBox7.Text = "";
            // 
            // richTextBox6
            // 
            this.richTextBox6.Location = new System.Drawing.Point(1092, 9);
            this.richTextBox6.Name = "richTextBox6";
            this.richTextBox6.Size = new System.Drawing.Size(268, 398);
            this.richTextBox6.TabIndex = 10;
            this.richTextBox6.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(835, 492);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "label1";
            // 
            // FormSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1110, 798);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.BlockCheck);
            this.Controls.Add(this.richTextBox4);
            this.Controls.Add(this.ParseBooks);
            this.Controls.Add(this.AutoIsOn);
            this.Controls.Add(this.AutoButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SearchParseAll);
            this.Name = "FormSearch";
            this.Text = "FormSearch";
            this.Load += new System.EventHandler(this.FormSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SearchParseAll;
        public System.Windows.Forms.RichTextBox richTextBox1;
        public System.Windows.Forms.RichTextBox richTextBox2;
        public System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button AutoButton;
        private System.Windows.Forms.CheckBox AutoIsOn;
        private System.Windows.Forms.Button ParseBooks;
        public System.Windows.Forms.RichTextBox richTextBox4;
        public System.Windows.Forms.RichTextBox richTextBox5;
        private System.Windows.Forms.BindingSource bindingSource1;
        public System.Windows.Forms.CheckBox BlockCheck;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.RichTextBox richTextBox6;
        public System.Windows.Forms.RichTextBox richTextBox7;
        private System.Windows.Forms.Label label1;
    }
}