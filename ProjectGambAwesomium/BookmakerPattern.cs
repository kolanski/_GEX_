using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace ProjectGambAwesomium
{
    class BookmakerPattern
    {
        public TabControl ParentTab;
        public List<string> GamesLinks;
        public List<TabPage> BookmakerTabs;
        public List<WebForm> BookmakerWebBrowsers;
        public TennisGames BookmakerTennisGames;
        public WebForm ParentBrowser;

        public void SetUpTabPage(TabControl TabPage)
        {
            ParentTab = TabPage;
            BookmakerTennisGames = new TennisGames();
        }

        public void Navigate(string url)
        {
            this.ParentBrowser.Load(url);
            //  Thread th= new Thread(()=>this.ParentBrowser.Navigate(url));
            //  th.Start();
            //  await Task.Delay(1000);
            //  ParentBrowser.Navigate(url);
            //Task.Run(()=> this.ParentBrowser.Navigate(url));

        }

        public void SetUpBrowser(WebForm Browser)
        {
            ParentBrowser = Browser;
        }

        public void CreateTab()
        {

            if (BookmakerTabs == null)
                BookmakerTabs = new List<TabPage>();
            if (BookmakerWebBrowsers == null)
                BookmakerWebBrowsers = new List<WebForm>();

            TabPage NewPage = new TabPage();
            WebForm NewWebWebForm = new WebForm();
            NewWebWebForm.TopLevel = false;
            NewWebWebForm.Dock = DockStyle.Fill;
            NewWebWebForm.Location = new System.Drawing.Point(3, 3);
            NewWebWebForm.Show();
            ParentTab.TabPages.Add(NewPage);
            ParentTab.SelectedTab = NewPage;
            NewPage.Controls.Add(NewWebWebForm);
            BookmakerWebBrowsers.Add(NewWebWebForm);
            BookmakerTabs.Add(NewPage);
        }

        public void CreateTab(string Url)
        {
            try
            {
                if (BookmakerTabs == null)
                    BookmakerTabs = new List<TabPage>();
                if (BookmakerWebBrowsers == null)
                    BookmakerWebBrowsers = new List<WebForm>();

                TabPage NewPage = new TabPage();
                WebForm NewWebWebForm = new WebForm(Url);
                NewWebWebForm.TopLevel = false;
                NewWebWebForm.Dock = DockStyle.Fill;
                NewWebWebForm.Location = new System.Drawing.Point(3, 3);
                NewWebWebForm.Show();
                ParentTab.TabPages.Add(NewPage);
                ParentTab.SelectedTab = NewPage;
                NewPage.Controls.Add(NewWebWebForm);
                BookmakerWebBrowsers.Add(NewWebWebForm);
                BookmakerTabs.Add(NewPage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CreateTab:" + ex.Message);
            }
        }
        public void CreateTab(string Url,string data)
        {
            try
            {
                if (BookmakerTabs == null)
                    BookmakerTabs = new List<TabPage>();
                if (BookmakerWebBrowsers == null)
                    BookmakerWebBrowsers = new List<WebForm>();

                TabPage NewPage = new TabPage();
                WebForm NewWebWebForm = new WebForm(Url,data);
                NewWebWebForm.TopLevel = false;
                NewWebWebForm.Dock = DockStyle.Fill;
                NewWebWebForm.Location = new System.Drawing.Point(3, 3);
                NewWebWebForm.Show();
                ParentTab.TabPages.Add(NewPage);
                ParentTab.SelectedTab = NewPage;
                NewPage.Controls.Add(NewWebWebForm);
                BookmakerWebBrowsers.Add(NewWebWebForm);
                BookmakerTabs.Add(NewPage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CreateTab:" + ex.Message);
            }
        }

        public async Task CreateTabDelay(string Url, int delay)
        {
            try
            {
                if (BookmakerTabs == null)
                    BookmakerTabs = new List<TabPage>();
                if (BookmakerWebBrowsers == null)
                    BookmakerWebBrowsers = new List<WebForm>();

                TabPage NewPage = new TabPage();
                WebForm NewWebWebForm = new WebForm();
                NewWebWebForm.TopLevel = false;
                NewWebWebForm.Dock = DockStyle.Fill;
                NewWebWebForm.Location = new System.Drawing.Point(3, 3);
                NewWebWebForm.Show();
                ParentTab.TabPages.Add(NewPage);
                ParentTab.SelectedTab = NewPage;
                NewPage.Controls.Add(NewWebWebForm);
                BookmakerWebBrowsers.Add(NewWebWebForm);
                BookmakerTabs.Add(NewPage);
                await Task.Delay(delay);
                NewWebWebForm.Load(Url);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CreateTab:" + ex.Message);
            }
        }
        public void CreateTabSafe(string Url)
        {
            bool found = false;
            try
            {
                if (BookmakerTabs == null)
                    BookmakerTabs = new List<TabPage>();
                if (BookmakerWebBrowsers == null)
                    BookmakerWebBrowsers = new List<WebForm>();
                for (int i = 0; i < BookmakerWebBrowsers.Count; i++)
                {
                    if (BookmakerWebBrowsers[i].Address.ToString() == "about:blank")
                    {
                        found = true;
                        BookmakerWebBrowsers[i].Load(Url);
                        break;
                    }

                }
                if (!found)
                {
                    TabPage NewPage = new TabPage();
                    WebForm NewWebWebForm = new WebForm();
                    NewWebWebForm.TopLevel = false;
                    NewWebWebForm.Dock = DockStyle.Fill;
                    NewWebWebForm.Location = new System.Drawing.Point(3, 3);
                    NewWebWebForm.Show();
                    ParentTab.TabPages.Add(NewPage);
                    ParentTab.SelectedTab = NewPage;
                    NewPage.Controls.Add(NewWebWebForm);
                    BookmakerWebBrowsers.Add(NewWebWebForm);
                    BookmakerTabs.Add(NewPage);
                    NewWebWebForm.Load(Url);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CreateTab:" + ex.Message);
            }
        }

        public async Task CloseAllTabs()
        {
            for (int i = BookmakerTabs.Count - 1; i >= 0; i--)
            {

                CloseTab(i);
                await Task.Delay(1000);



            }
        }

        public async Task CloseTab(string Url)
        {
            for (int i = 0; i < BookmakerTabs.Count; i++)
            {
                try
                {
                    if (BookmakerWebBrowsers[i].Address.ToString() == Url)
                    {
                        await CloseTab(i);

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
                    if (BookmakerWebBrowsers[i].Address.ToString() == Url)
                    {
                        BookmakerWebBrowsers[i].Load("about:blank");
                        break;
                    }
                }
                catch
                {

                }
            }
        }

        public async Task CloseTab(int index)
        {
            try
            {
                //Debug.WriteLine("Bookdeleted1:" + BookmakerWebBrowsers[index].IsAccessible.ToString());
                BookmakerWebBrowsers[index].Close();
              //  Debug.WriteLine("Bookdeleted2:" + BookmakerWebBrowsers[index].IsAccessible.ToString());
                BookmakerWebBrowsers.RemoveAt(index);
                
                TabPage tab = BookmakerTabs[index];
                ParentTab.TabPages.Remove(tab);
                BookmakerTabs.Remove(tab);
               
                
                await Task.Delay(250);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public void CloseTabSafe(int index)
        {
            BookmakerWebBrowsers[index].Load("about:blank");
        }
    }
}
