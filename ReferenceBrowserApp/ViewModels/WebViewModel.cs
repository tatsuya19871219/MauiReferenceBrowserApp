using CommunityToolkit.Mvvm.ComponentModel;
using ReferenceBrowserApp.Data;
using ReferenceBrowserApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.WebRequestMethods;

namespace ReferenceBrowserApp.ViewModels;

public partial class WebViewModel : ObservableObject
{
    public static WebViewModel Instance;

    [ObservableProperty]
    string currentLocation;

    [ObservableProperty]
    string currentStatus;

    ReferenceSite _reference;

    ReferenceSearchItemDatabase _database;

    WebView _webView;

    SearchHistory _searchHistory;


    public WebViewModel(ReferenceSearchItemDatabase database) 
    {
        _database = database;

        Instance = this;

        CurrentStatus = "WebViewModel is Initialized.";
    }

    public void BindWebView(WebView webView)
    {
        _webView = webView;

        //_webView.Source = _reference.ToString();
    }

    public void SetReferenceSite(ReferenceSite reference)
    {
        _reference = reference;
        _searchHistory = new(reference);

        _database.SetReferenceSite(reference);

        //_webView.Source = reference.ToString();
        CurrentLocation = reference.ToString();
        //_webView.Source = CurrentLocation;
        //_webView.Source = "https://learn.microsoft.com/dotnet/maui";

        GoTo(reference);

        CurrentStatus = $"You're in {_reference.Name} reference.";

    }


    // Callback functions for WebView
    public void NavigatingCallback(object sender, WebNavigatingEventArgs e)
    {
        var uri = new SearchUri(e.Url);

        // return if navigation event is not new page
        if (e.NavigationEvent != WebNavigationEvent.NewPage) return;

        if (_searchHistory.TryPush(uri))
        {
            _database.TryPushSearchUri(uri);

        }
        else
        {
            // Cancelation of navigation works wiredly in android
            //e.Cancel = true; 

        }
    }


    public void NavigatedCallback(object sender, WebNavigatedEventArgs e)
    {
        if (e.Result == WebNavigationResult.Success)
        {
            CurrentLocation = e.Url;

            if(_reference.Contains(new SearchUri(e.Url)))
            {
                CurrentStatus = $"You're in {_reference.Name} reference.";
            }
            else
            {
                CurrentStatus = $"You're not in {_reference.Name} reference.";
            }
        }
    }

    

    public void GoHome(object sender, EventArgs e)
    {
        _searchHistory.TryPush(_reference);
        CurrentLocation = _reference.ToString();
        GoTo(_reference);
    }

    public void GoNext(object sender, EventArgs e)
    {
        //_searchHistory
        if (_webView.CanGoForward)
        {
            _searchHistory.GoForward();
            _webView.GoForward();
        }
    }

    public void GoPrevious(object sender, EventArgs e)
    {
        if (_webView.CanGoBack)
        {
            _searchHistory.GoBack();
            _webView.GoBack();
        }
    }

    public void GoTo(SearchUri uri)
    {
        _webView.Source = uri.ToString();
    }
}
