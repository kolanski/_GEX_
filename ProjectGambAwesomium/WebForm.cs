using System;
using System.IO;
using Awesomium.Core;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using Awesomium.Windows.Forms;
using System.Runtime.InteropServices;
using Awesomium.Core.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectGambAwesomium
{
    public partial class WebForm : Form
    {
        public WebView webView;
        private ImageSurface surface;
        private WebSession session;
        private BindingSource bindingSource;
        public string Data;
        public int RetryCount = 0;
        public WebForm()
        {
            WebSession session = InitializeCoreAndSession();
            InitializeComponent();
            InitializeView(WebCore.CreateWebView(this.ClientSize.Width, this.ClientSize.Height, session));
        }
        public void Load(string url)
        {
            webView.Source=new Uri(url);
        }
        public string Address
        {
            get { return webView.Source.ToString(); }
        }
        public Awesomium.Core.JSValue EvaluateScriptAsync(string script)
        {
            JSValue result;

            if (webView.InvokeRequired)
            {
                result = (JSValue)webView.Invoke(
                      (Func<String, JSValue>)webView.ExecuteJavascriptWithResult,
                      script);
                Application.DoEvents();
            }
            else
            { 
                result = webView.ExecuteJavascriptWithResult(script);
                Application.DoEvents();
            }

            /*if (webView.IsDocumentReady)
                return webView.ExecuteJavascriptWithResult(script);
            else
            {
                RetryCount++;
                if (RetryCount > 10)
                {
                    RetryCount = 0;
                    webView.Source = webView.Source;
                }
                return JSValue.Undefined;
            }*/
            return result;
        }
        private  static void Executeme(WebView view, string js)
        {
            // JSValue supports implicit casting to and from the supported
            // types so we could actually assign directly to string.
            // In this sample however, we need to check for undefined.
           WebCore.QueueWork(view,new Action(()=>{ view.ExecuteJavascript(js);}));
        }
        public void ExecuteScriptAsync(string script)
        {
            if (webView.IsDocumentReady)
            {
                try
                {
                    webView.Invoke((MethodInvoker)delegate
                    {
                        webView.ExecuteJavascriptWithResult(script);
                        Application.DoEvents();
                    });
                }
                catch(Exception e)
                {
                    webView.Invoke((MethodInvoker)delegate
                        {
                            webView.ExecuteJavascriptWithResult(script);
                            Application.DoEvents();
                        });
                    //Console.WriteLine(e.Message);
                }
            }
            else
            {
                RetryCount++;
                if (RetryCount > 10)
                {
                    RetryCount = 0;
                    webView.Invoke((MethodInvoker)delegate
                        {
                    webView.Source = webView.Source;
                        });
                }
            }
        }

        public WebForm(Uri targetURL)
        {
            // Initialize the core and get a WebSession.
            WebSession session = InitializeCoreAndSession();

            // Notice that 'Control.DoubleBuffered' has been set to true
            // in the designer, to prevent flickering.

            InitializeComponent();

            // Initialize a new view.
            InitializeView(WebCore.CreateWebView(this.ClientSize.Width, this.ClientSize.Height, session), false, targetURL);
        }

        public WebForm(String targetURL)
        {
            // Initialize the core and get a WebSession.
            WebSession session = InitializeCoreAndSession();

            // Notice that 'Control.DoubleBuffered' has been set to true
            // in the designer, to prevent flickering.

            InitializeComponent();

            // Initialize a new view.
            if (targetURL != "undefined")
            {
                InitializeView(WebCore.CreateWebView(this.ClientSize.Width, this.ClientSize.Height, session), false, new Uri(targetURL));
            }
            else
            {

            }
        }
        public WebForm(String targetURL,string data)
        {
            // Initialize the core and get a WebSession.
            WebSession session = InitializeCoreAndSession();

            // Notice that 'Control.DoubleBuffered' has been set to true
            // in the designer, to prevent flickering.

            InitializeComponent();
            Data = data;
            // Initialize a new view.
            InitializeView(WebCore.CreateWebView(this.ClientSize.Width, this.ClientSize.Height, session), false, new Uri(targetURL));
        }
        internal WebForm(WebView view, int width, int height)
        {
            this.Width = width;
            this.Height = height;

            InitializeComponent();

            // Initialize the view.
            InitializeView(view, true);

            // We should immediately call a resize,
            // after wrapping child views.
            if (view != null)
                view.Resize(width, height);
        }
        private WebSession InitializeCoreAndSession()
        {
            if (!WebCore.IsInitialized)
                WebCore.Initialize(new WebConfig()
                {
                    AssetProtocol = "https",
                    LogLevel = LogLevel.Normal
                    
                });
            //this.webSessionProvider1.DataPath = Application.StartupPath + "\\SessionDataPath";
            // Build a data path string. In this case, a Cache folder under our executing directory.
            // - If the folder does not exist, it will be created.
            // - The path should always point to a writeable location.
            string dataPath = String.Format("{0}{1}Cache", Path.GetDirectoryName(Application.ExecutablePath), Path.DirectorySeparatorChar);

            // Check if a session synchronizing to this data path, is already created;
            // if not, create a new one.
            try
            {
                session = WebCore.Sessions[dataPath] ??
                    WebCore.CreateWebSession(dataPath, new WebPreferences() { });//??WebCore.CreateWebSession(dataPath, WebPreferences.Default);
            }
            catch
            {

            }
            // session.AddDataSource(DataSource.CATCH_ALL, new MyDataSource());

            // The core must be initialized by now. Print the core version.
            Debug.Print(WebCore.Version.ToString());

            // Return the session.
            return session;
        }
        private void InitializeView(WebView view, bool isChild = false, Uri targetURL = null)
        {
            if (view == null)
                return;

            // We demonstrate the use of a resource interceptor.
            // if (WebCore.ResourceInterceptor == null)
            //     WebCore.ResourceInterceptor = this;

            // Create an image surface to render the
            // WebView's pixel buffer.
            surface = new ImageSurface();
            surface.Updated += OnSurfaceUpdated;

            webView = view;

            // Assign our surface.
            webView.Surface = surface;
            // Assign a context menu.
            // webControlContextMenu.View = webView;

            // Handle some important events.
            webView.CursorChanged += OnCursorChanged;
            webView.AddressChanged += OnAddressChanged;
            webView.ShowCreatedWebView += OnShowNewView;
            webView.ShowContextMenu += OnShowContextMenu;
            webView.PrintRequest += OnPrintRequest;
            webView.PrintComplete += OnPrintComplete;
            webView.PrintFailed += OnPrintFailed;
            webView.Crashed += OnCrashed;
            webView.ShowJavascriptDialog += OnJavascriptDialog;
            webView.WindowClose += OnWindowClose;
            webView.DocumentReady += OnDocumentReady;
            // We demonstrate binding to properties.
            bindingSource = new BindingSource() { DataSource = webView };
            this.DataBindings.Add(new Binding("Text", bindingSource, "Title", true));

            if (!isChild)
                // Tip: /ncr = No Country Redirect ;-)
                webView.Source = targetURL ?? new Uri("http://www.google.com/ncr");

            // Give focus to the view.
            webView.FocusView();
        }
        int cnt = 0;
        private void OnDocumentReady(object sender, DocumentReadyEventArgs e)
        {
            //if (cnt == 3)
            //    webView.Source = webView.Source;
            //cnt++;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if ((surface != null) && (surface.Image != null))
                e.Graphics.DrawImageUnscaled(surface.Image, 0, 0);
            else
                base.OnPaint(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            this.Opacity = 1.0D;

            if ((webView == null) || !webView.IsLive)
                return;

            webView.FocusView();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            if ((webView == null) || !webView.IsLive)
                return;

            // Let popup windows be semi-transparent,
            // when they are not active.
            if (webView.ParentView != null)
                this.Opacity = 0.8D;

            webView.UnfocusView();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Destroy the WebView.
            if (webView != null)
            {
                webView.Dispose();
                webView = null;
            }

            // Destroy our customized Context Menu.
            /* if (webControlContextMenu != null)
             {
                 webControlContextMenu.Dispose();
                 webControlContextMenu = null;
             }*/

            // The surface that is currently assigned to the view,
            // does not need to be disposed. It will be disposed 
            // internally.

            base.OnFormClosed(e);

            // For WebCore.Shutdown, see OnApplicationExit in Program.cs.
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if ((webView == null) || !webView.IsLive)
                return;

            // Never resize the view to a width or height equal to 0;
            // instead, you can pause internal rendering.
            webView.IsRendering = (this.ClientSize.Width > 0) && (this.ClientSize.Height > 0);

            if (webView.IsRendering)
                // Request a resize.
                webView.Resize(this.ClientSize.Width, this.ClientSize.Height);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if ((webView == null) || !webView.IsLive)
                return;

            webView.InjectKeyboardEvent(e.GetKeyboardEvent());
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if ((webView == null) || !webView.IsLive)
                return;

            webView.InjectKeyboardEvent(e.GetKeyboardEvent(WebKeyboardEventType.KeyDown));
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if ((webView == null) || !webView.IsLive)
                return;

            webView.InjectKeyboardEvent(e.GetKeyboardEvent(WebKeyboardEventType.KeyUp));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if ((webView == null) || !webView.IsLive)
                return;

            webView.InjectMouseDown(e.Button.GetMouseButton());
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if ((webView == null) || !webView.IsLive)
                return;

            webView.InjectMouseUp(e.Button.GetMouseButton());
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if ((webView == null) || !webView.IsLive)
                return;

            webView.InjectMouseMove(e.X, e.Y);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if ((webView == null) || !webView.IsLive)
                return;

            webView.InjectMouseWheel(e.Delta, 0);
        }



        private void OnAddressChanged(object sender, UrlEventArgs e)
        {
            // Reflect the current URL to the window text.
            // Normally, after the page loads, we will get a title.
            // But a page may as well not specify a title.
            this.Text = e.Url.AbsoluteUri;
        }

        private void OnCursorChanged(object sender, CursorChangedEventArgs e)
        {
            // Update the cursor.
            this.Cursor = Awesomium.Windows.Forms.Utilities.GetCursor(e.CursorType);
        }

        private void OnSurfaceUpdated(object sender, SurfaceUpdatedEventArgs e)
        {
            // When the surface is updated, invalidate the 'dirty' region.
            // This will force the form to repaint that region.
            Invalidate(e.DirtyRegion.ToRectangle(), false);
        }

        private void OnShowContextMenu(object sender, ContextMenuEventArgs e)
        {
            // A context menu is requested, typically as a result of the user
            // right-clicking in the view. Open our extended WebControlContextMenu.
            //webControlContextMenu.Show( this );
        }

        private void OnShowNewView(object sender, ShowCreatedWebViewEventArgs e)
        {
            if ((webView == null) || !webView.IsLive)
                return;

            if (e.IsPopup)
            {
                // Create a WebView wrapping the view created by Awesomium.
                WebView view = new WebView(e.NewViewInstance);
                // ShowCreatedWebViewEventArgs.InitialPos indicates screen coordinates.
                Rectangle screenRect = e.Specs.InitialPosition.ToRectangle();
                // Create a new WebForm to render the new view and size it.
                WebForm childForm = new WebForm(view, screenRect.Width, screenRect.Height)
                {
                    ShowInTaskbar = false,
                    FormBorderStyle = FormBorderStyle.FixedToolWindow,
                    ClientSize = screenRect.Size != Size.Empty ? screenRect.Size : new Size(640, 480)
                };

                // Show the form.
                childForm.Show(this);

                if (screenRect.Location != Point.Empty)
                    // Move it to the specified coordinates.
                    childForm.DesktopLocation = screenRect.Location;
            }
            else if (e.IsWindowOpen || e.IsPost)
            {
                // Create a WebView wrapping the view created by Awesomium.
                WebView view = new WebView(e.NewViewInstance);
                // Create a new WebForm to render the new view and size it.
                WebForm childForm = new WebForm(view, 640, 480);
                // Show the form.
                childForm.Show(this);
            }
            else
            {
                // Let the new view be destroyed. It is important to set Cancel to true 
                // if you are not wrapping the new view, to avoid keeping it alive along
                // with a reference to its parent.
                e.Cancel = true;

                // Load the url to the existing view.
                webView.Source = e.TargetURL;
            }
        }

        private void OnCrashed(object sender, CrashedEventArgs e)
        {
            Debug.Print("Crashed! Status: " + e.Status);
        }

        // Called in response to JavaScript: 'window.close'.
        private void OnWindowClose(object sender, WindowCloseEventArgs e)
        {
            // If this is a child form, respect the request and close it.
            if ((webView != null) && (webView.ParentView != null))
                this.Close();
        }

        // This is called when the page asks to be printed, usually as result of
        // a window.print().
        private void OnPrintRequest(object sender, PrintRequestEventArgs e)
        {
            if (!webView.IsLive)
                return;

            // You can actually call PrintToFile anytime after the ProcessCreated
            // event is fired (or the DocumentReady or LoadingFrameComplete in 
            // subsequent navigations), but you usually call it in response to
            // a print request. You should possibly display a dialog to the user
            // such as a FolderBrowserDialog, to allow them select the output directory
            // and verify printing.
            int requestId = webView.PrintToFile(@".\Prints", PrintConfig.Default);

            Debug.Print(String.Format("Print request {0} is being printed to {1}.", requestId, @".\Prints"));
        }

        private void OnPrintComplete(object sender, PrintCompleteEventArgs e)
        {
            Debug.Print(String.Format("Print request {0} completed. The following files were created:", e.RequestId));

            foreach (string file in e.Files)
                Debug.Print(String.Format("\t {0}", file));
        }

        private void OnPrintFailed(object sender, PrintOperationEventArgs e)
        {
            Debug.Print(String.Format("Printing request {0} failed! Make sure the provided outputDirectory is writable.", e.RequestId));
        }

        private void OnJavascriptDialog(object sender, JavascriptDialogEventArgs e)
        {
            if (!e.DialogFlags.HasFlag(JSDialogFlags.HasPromptField) &&
                !e.DialogFlags.HasFlag(JSDialogFlags.HasCancelButton))
            {
                // It's a 'window.alert'
                MessageBox.Show(this, e.Message);
                e.Handled = true;
            }
        }

        private void webControlContextMenu_Opening(object sender, ContextMenuOpeningEventArgs e)
        {
            // Update the visibility of our menu items based on the
            // latest context data.
            // openSeparator.Visible =
            //    openMenuItem.Visible = !e.Info.IsEditable && ( webView.Source != null );
        }

        private void webControlContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if ((webView == null) || !webView.IsLive)
                return;

            // We only process the menu item added by us. The WebControlContextMenu
            // will handle the predefined items.
            if ((string)e.ClickedItem.Tag != "open")
                return;

            WebForm webForm = new WebForm(webView.Source);
            webForm.Show(this);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            webView.Source = new Uri(textBox1.Text);
        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            textBox1.Text = webView.Source.AbsoluteUri.ToString();
        }
    }
}
