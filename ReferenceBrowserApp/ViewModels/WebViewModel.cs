using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceBrowserApp.ViewModels;

public partial class WebViewModel : ObservableObject
{

    [ObservableProperty]
    string currentLocation;


    public WebViewModel() 
    { 
    }


    // Callback functions for WebView
    //async private void myWebView_Navigated(object sender, WebNavigatedEventArgs e)
    //{
    //    if (e.Result == WebNavigationResult.Success)
    //    {

    //        // if url doesn't indicate the dotnet directory or deeper, don't save

    //        CurrentLocation = e.Url;

    //        // analyse url
    //        var url = new Uri(e.Url);

    //        // query check including redirection on mobile 

    //        if (e.Url.Equals(_baseUrl.OriginalString))
    //        {
    //            return;
    //        }
    //        else if (!url.LocalPath.Contains(_baseLocalPath))
    //        {
    //            await Task.Delay(1000);

    //            // return to base
    //            myWebView.Source = CurrentLocation = _baseUrl.OriginalString;

    //            return;
    //        }

    //        //HttpStyleUriParser parser = new HttpStyleUriParser(); how to use this?


    //        //         if(!await _database.HasItemByURLAsync(e.Url))
    //        //{
    //        //	_database.AddNewItemAsync(e.Url);
    //        //}
    //        //else
    //        //{
    //        //	_database.UpdateItemAsync(e.Url);
    //        //}

    //    }
    //}

    //private void myWebView_Navigating(object sender, WebNavigatingEventArgs e)
    //{
    //    //e.Cancel= true;
    //}

}
