using EO.WebBrowser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Google_Search_TrackerUrl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _webView = new WebView();

            WebBrw.WebView = _webView;
        }

        private WebView _webView;

        private void BtnLoadAndSearch_Click(object sender, RoutedEventArgs e)
        {
            _webView.LoadUrl("https://www.google.com/search?q=nokia+3310");
        }

        private void BtnGetTrackerUrl_Click(object sender, RoutedEventArgs e)
        {
            GetTargetUrl();
        }

        private void GetTargetUrl()
        {
            var linksOrig = new List<string>();
            try
            {
                EO.WebBrowser.DOM.Window wb = _webView.GetDOMWindow();
                string htmlofPage = _webView.GetHtml();
                //File.WriteAllText(@"c:\temp\a1.htm", htmlofPage);
                if (wb.document != null)
                {
                    var elements = wb.document.getElementsByTagName("h3");
                    List<EO.WebBrowser.DOM.Element> d = new List<EO.WebBrowser.DOM.Element>();
                    if (elements != null)
                        d = elements.ToList();
                    //var d = wb.document.getElementsByTagName("h3").ToList();
                    if (d.Count > 0)
                    {
                        foreach (var h in d)
                        {
                            if (h.innerHTML == null)
                                continue;
                            if (h.innerHTML.Contains("<a "))
                            {
                                EO.WebBrowser.JSObject a = (EO.WebBrowser.JSObject)h["firstChild"];

                                if (a == null)
                                    continue;
                                if (a["href"] != null && a["href"] is string)
                                {

                                    var anchorHref = (string)a["href"];
                                    anchorHref = anchorHref.TrimEnd('/');

                                    string newUrl = anchorHref;
                                    if (newUrl.StartsWith("/url?"))
                                        newUrl = "http://www.google.com" +  newUrl;

                                    newUrl = Uri.UnescapeDataString(newUrl);

                                    linksOrig.Add(newUrl);

                                }
                            }
                        }

                    }
                }
                if(linksOrig.Count>0)
                    MessageBox.Show(linksOrig[linksOrig.Count - 1]);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
