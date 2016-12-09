using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace WikipediaViewer2
{
    enum Language
    {
        PL,
        ENG
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Language lang = WikipediaViewer2.Language.PL;
        string prefix = @"https://pl.";
        Language Lang
        {
            get { return lang; }
            set
            {
                lang = value;
                prefix = value == WikipediaViewer2.Language.PL ? @"https://pl." : @"https://en.";
            }
        }

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.
        }

        private void tbSearch_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
                GetArticles();
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            GetArticles();
        }

        private void GetArticles()
        {
            try
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    string msg = Lang == WikipediaViewer2.Language.PL ? "Brak połączenia z internetem!" : "No internet connection!";
                    ShowMessage(msg);
                    return;
                }

                if (String.IsNullOrEmpty(tbSearch.Text))
                {
                    string msg = Lang == WikipediaViewer2.Language.PL ? "Pole tekstowe nie może być puste!" : "Textbox can't be empty!";
                    ShowMessage(msg);
                    return;
                }

                string url = prefix + @"wikipedia.org/w/api.php?action=query&format=json&list=search&srsearch=" + tbSearch.Text + "&srprop=snippet";
                var json = GetWebString(url);
                RootObject jsonPages = JsonConvert.DeserializeObject<RootObject>(json);
                int numberOfArticles = jsonPages.query.search.Count;

                SetMainGrid(numberOfArticles);
                ClearArticleGrid();
                if (numberOfArticles > 0)
                    ShowArticles(jsonPages);
                else
                {
                    string msg = Lang == WikipediaViewer2.Language.PL ? "Brak wyników wyszukiwania!" : "No search results!";
                    ShowMessage(msg);
                }
            }
            catch(Exception ex)
            {
                string msg = Lang == WikipediaViewer2.Language.PL ? "Wystąpił nieznany błąd!" : ex.Message;
                ShowMessage(msg);
            }
        }
        private void SetMainGrid(int numberOfArticles)
        {
            mainGrid.RowDefinitions[2].Height = new GridLength(3 * numberOfArticles, GridUnitType.Star);
            mainGrid.Height = 200 + numberOfArticles * 200;
        }
        private void ClearArticleGrid()
        {
            articlesGrid.Children.Clear();
            articlesGrid.RowDefinitions.Clear();
        }
        private void ShowArticles(RootObject jsonPages)
        {
            for (int i = 0; i < jsonPages.query.search.Count; i++)
            {
                articlesGrid.RowDefinitions.Add(new RowDefinition());
                articlesGrid.RowDefinitions[i].Height = new GridLength(1, GridUnitType.Star);

                StackPanel sp = new StackPanel();

                TextBlock tbTitle = new TextBlock();
                tbTitle.TextWrapping = TextWrapping.Wrap;
                tbTitle.Text = jsonPages.query.search[i].title;
                Style style = Resources["Title"] as Style;
                tbTitle.Style = style;
                sp.Children.Add(tbTitle);

                TextBlock tbDesc = new TextBlock();
                tbDesc.TextWrapping = TextWrapping.Wrap;
                string txt = StripHTML(jsonPages.query.search[i].snippet);
                Regex rgx = new Regex(@"\s{2,}");
                tbDesc.Text = rgx.Replace(txt, " ");
                style = Resources["Desc"] as Style;
                tbDesc.Style = style;
                sp.Children.Add(tbDesc);

                Button button = new Button();
                button.Click += new RoutedEventHandler(Article_Click);
                button.Style = Resources["Article"] as Style;
                button.Content = sp;

                Grid.SetRow(button, i);
                articlesGrid.Children.Add(button);
            }
        }
        private string GetWebString(string uri)
        {
            using (var client = new HttpClient())
            {
                var stringTask = client.GetStringAsync(uri).Result;
                return stringTask;
            }
        }
        private string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>|&quot;", String.Empty);
        }
        private async void launchUri(string uriToLaunch)
        {
            Uri uri = new Uri(uriToLaunch);
            bool success = await Launcher.LaunchUriAsync(uri);
        }

        private void Article_Click(object sender, RoutedEventArgs e)
        {
            string uriToLaunch = prefix + @"wikipedia.org/wiki/" + (((sender as Button).Content as StackPanel).Children[0] as TextBlock).Text;
            launchUri(uriToLaunch);
        }
        private void RandomArticle_Click(object sender, RoutedEventArgs e)
        {
            launchUri(prefix + @"wikipedia.org/wiki/Special:Random");
        }
        private void ChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.IsEnabled = false;
            Image img = new Image();
            img.Height = 50;
            img.Margin = new Thickness(0, 3, 0, 0);

            Image img2 = new Image();
            img2.Height = 50;
            img2.Margin = new Thickness(0, 3, 0, 0);

            if (btn.Name == "btnPL")
            {
                Lang = WikipediaViewer2.Language.PL;
                btnEng.IsEnabled = true;
                img.Source = new BitmapImage(new Uri("ms-appx:///Images/ENG.png"));
                btnEng.Content = img;

                img2.Source = new BitmapImage(new Uri("ms-appx:///Images/PL-OFF.png"));
                btnPL.Content = img2;
            }
            else
            {
                btnPL.IsEnabled = true;
                Lang = WikipediaViewer2.Language.ENG;
                img.Source = new BitmapImage(new Uri("ms-appx:///Images/PL.png"));
                btnPL.Content = img;

                img2.Source = new BitmapImage(new Uri("ms-appx:///Images/ENG-OFF.png"));
                btnEng.Content = img2;
            }

            if (String.IsNullOrEmpty(tbSearch.Text))
            {
                SetMainGrid(0);
                ClearArticleGrid();
            }
            else
                GetArticles();
            
        }

        private async void ShowMessage(string msg)
        {
            MessageDialog msgBox = new MessageDialog(msg);
            await msgBox.ShowAsync();
        }
    }
}
