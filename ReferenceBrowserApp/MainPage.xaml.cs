using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using ReferenceBrowserApp.Data;
using ReferenceBrowserApp.Models;

namespace ReferenceBrowserApp;

public partial class MainPage : ContentPage
{

	string _currentLocation;

	public string CurrentLocation {
		get => _currentLocation;
		private set
		{
			_currentLocation = value;
			myURL.Text = _currentLocation;
		} 
	}
	
	SearchItemDatabase _database;

	Uri _baseUrl = new Uri("https://learn.microsoft.com/en-us/dotnet/maui/?view=net-maui-7.0");

	string _baseLocalPath;

    public MainPage(SearchItemDatabase database)
	{
		InitializeComponent();

		Application.Current.UserAppTheme = AppTheme.Dark;
		
		_database = database;

		BindingContext = this;

		CurrentLocation = _baseUrl.OriginalString;

		_baseLocalPath = _baseUrl.LocalPath;

		//SwipeGestureRecognizer swipeGesture = new SwipeGestureRecognizer();

		myWebView.Source = CurrentLocation;

		// prepare to access database
		PrepareDatabaseAccess();
    }

	async void PrepareDatabaseAccess()
	{
		while (true)
		{
			if (_database.IsInitialized) break;
			await Task.Delay(100);
		}

		checkButton.IsEnabled = true;
	}


    private void Button_Clicked(object sender, EventArgs e)
    {
		MoveToSubPage();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

		myContentView.WidthRequest = width;
		myContentView.HeightRequest = height - Header.Height - Footer.Height;

    }

    //async private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
    //{
    //    //Page page = (Page)Activator.CreateInstance(typeof(SubPage));
    //    SubPage page = (SubPage)Activator.CreateInstance(typeof(SubPage));
    //    await Navigation.PushAsync(page);
    //}

	async void MoveToSubPage() 
		=> await Shell.Current.GoToAsync(nameof(SubPage)); 

    async private void myWebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
		if (e.Result == WebNavigationResult.Success)
		{

			// if url doesn't indicate the dotnet directory or deeper, don't save

			CurrentLocation = e.Url;

            // analyse url
            var url = new Uri(e.Url);

			// query check including redirection on mobile 

            if (e.Url.Equals(_baseUrl.OriginalString))
			{
				return;
			}
			else if(!url.LocalPath.Contains(_baseLocalPath))
			{
				await Task.Delay(1000);

				// return to base
				myWebView.Source = CurrentLocation = _baseUrl.OriginalString;

				return;
			}

			//HttpStyleUriParser parser = new HttpStyleUriParser(); how to use this?
			

            if(!await _database.HasItemByURLAsync(e.Url))
			{
				_database.AddNewItemAsync(e.Url);
			}
			else
			{
				_database.UpdateItemAsync(e.Url);
			}

		}
    }

    private void myWebView_Navigating(object sender, WebNavigatingEventArgs e)
    {
		//e.Cancel= true;
    }
}

