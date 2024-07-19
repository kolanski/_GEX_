using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace ProjectGamb
{
    class BookmakerPattern
    {
        public TabControl ParentTab;
        public List<string> GamesLinks;
        public List<TabPage> BookmakerTabs;
        public List<Form2> BookmakerWebBrowsers;
        public TennisGames BookmakerTennisGames;
        public Gecko.GeckoWebBrowser ParentBrowser;
        
        public void SetUpTabPage(TabControl TabPage)
        {
            ParentTab = TabPage;
            BookmakerTennisGames = new TennisGames();
        }

        public  void Navigate(string url)
        {
            this.ParentBrowser.Navigate(url);
          //  Thread th= new Thread(()=>this.ParentBrowser.Navigate(url));
          //  th.Start();
          //  await Task.Delay(1000);
          //  ParentBrowser.Navigate(url);
            //Task.Run(()=> this.ParentBrowser.Navigate(url));
            
        }

        public void SetUpBrowser(Gecko.GeckoWebBrowser Browser)
        {
            ParentBrowser = Browser;
        }

        public void CreateTab()
        {

            if (BookmakerTabs == null)
                BookmakerTabs = new List<TabPage>();
            if (BookmakerWebBrowsers == null)
                BookmakerWebBrowsers = new List<Form2>();

            TabPage NewPage = new TabPage();
            Form2 NewWebForm2 = new Form2();
            NewWebForm2.TopLevel = false;
            NewWebForm2.Dock = DockStyle.Fill;
            NewWebForm2.Location = new System.Drawing.Point(3, 3);
            NewWebForm2.Show();
            ParentTab.TabPages.Add(NewPage);
            ParentTab.SelectedTab = NewPage;
            NewPage.Controls.Add(NewWebForm2);
            BookmakerWebBrowsers.Add(NewWebForm2);
            BookmakerTabs.Add(NewPage);
        }

        public void CreateTab(string Url)
        {
            try
            { 
            if (BookmakerTabs == null)
                BookmakerTabs = new List<TabPage>();
            if (BookmakerWebBrowsers == null)
                BookmakerWebBrowsers = new List<Form2>();

            TabPage NewPage = new TabPage();
            Form2 NewWebForm2 = new Form2();
            NewWebForm2.TopLevel = false;
            NewWebForm2.Dock = DockStyle.Fill;
            NewWebForm2.Location = new System.Drawing.Point(3, 3);
            NewWebForm2.Show();
            ParentTab.TabPages.Add(NewPage);
            ParentTab.SelectedTab = NewPage;
            NewPage.Controls.Add(NewWebForm2);
            BookmakerWebBrowsers.Add(NewWebForm2);
            BookmakerTabs.Add(NewPage);
            NewWebForm2.geckoWebBrowser1.Navigate(Url);
            }
            catch(Exception ex)
            {
                Console.WriteLine("CreateTab:"+ex.Message);
            }
        }

        public void CreateTabSafe(string Url)
        {
            bool found=false;
            try
            {
                if (BookmakerTabs == null)
                    BookmakerTabs = new List<TabPage>();
                if (BookmakerWebBrowsers == null)
                    BookmakerWebBrowsers = new List<Form2>();
                for(int i=0;i<BookmakerWebBrowsers.Count;i++)
                {
                    if(BookmakerWebBrowsers[i].geckoWebBrowser1.Url.ToString()=="about:blank")
                    {
                        found = true;
                        BookmakerWebBrowsers[i].geckoWebBrowser1.Navigate(Url);
                    }
                    break;
                }
                if (!found)
                {
                    TabPage NewPage = new TabPage();
                    Form2 NewWebForm2 = new Form2();
                    NewWebForm2.TopLevel = false;
                    NewWebForm2.Dock = DockStyle.Fill;
                    NewWebForm2.Location = new System.Drawing.Point(3, 3);
                    NewWebForm2.Show();
                    ParentTab.TabPages.Add(NewPage);
                    ParentTab.SelectedTab = NewPage;
                    NewPage.Controls.Add(NewWebForm2);
                    BookmakerWebBrowsers.Add(NewWebForm2);
                    BookmakerTabs.Add(NewPage);
                    NewWebForm2.geckoWebBrowser1.Navigate(Url);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateTab:" + ex.Message);
            }
        }

        public void CloseAllTabs()
        {
            for (int i = BookmakerTabs.Count - 1; i >= 0;i-- )
            {
                try
                {
                    TabPage tab = BookmakerTabs[i];
                    if (BookmakerWebBrowsers != null)
                        BookmakerWebBrowsers.RemoveAt(i);
                    ParentTab.TabPages.Remove(tab);

                    BookmakerTabs.Remove(tab);

                    //BookmakerWebBrowsers[i].geckoWebBrowser1.Dispose();
                }
                catch
                {

                }
            }
        }

        public void CloseTab(string Url)
        {
            for (int i = 0; i < BookmakerTabs.Count; i++)
            {
                try
                {
                    if (BookmakerWebBrowsers[i].geckoWebBrowser1.Url.ToString() == Url)
                    {
                        TabPage tab = BookmakerTabs[i];
                        if (BookmakerWebBrowsers != null)
                            BookmakerWebBrowsers.RemoveAt(i);
                        ParentTab.TabPages.Remove(tab);

                        BookmakerTabs.Remove(tab);
                        //BookmakerWebBrowsers[i].geckoWebBrowser1.Dispose();

                        break;
                    }
                }
                catch
                {

                }
            }
        }

        public void CloseTabSafe(string Url)
        {
            for (int i = 0; i < BookmakerTabs.Count; i++)
            {
                try
                {
                    if (BookmakerWebBrowsers[i].geckoWebBrowser1.Url.ToString() == Url)
                    {
                        BookmakerWebBrowsers[i].geckoWebBrowser1.Navigate("about:blank");
                        break;
                    }
                }
                catch
                {

                }
            }
        }

        public void CloseTab(int index)
        {
            TabPage tab = BookmakerTabs[index];
            ParentTab.TabPages.Remove(tab);
            BookmakerTabs.Remove(tab);
            //BookmakerWebBrowsers[index].geckoWebBrowser1.Dispose();
            BookmakerWebBrowsers.RemoveAt(index);
        }

        public void CloseTabSafe(int index)
        {
            BookmakerWebBrowsers[index].geckoWebBrowser1.Navigate("about:blank");
        }
    }
}
