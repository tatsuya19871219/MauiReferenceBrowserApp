using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using ReferenceBrowserApp.Data;
using ReferenceBrowserApp.Models;

namespace ReferenceBrowserApp;

public partial class MainPage : ContentPage
{

	public string CurrentLocation { get; private set; }
	
	SearchItemDatabase _database;

	Uri _baseUrl = new Uri("https://learn.microsoft.com/en-us/dotnet/maui/?view=net-maui-7.0");

	string _baseLocalPath;

    public MainPage(SearchItemDatabase database)
	{
		InitializeComponent();

		
		_database = database;

		BindingContext = this;

		CurrentLocation = _baseUrl.OriginalString;

		_baseLocalPath = _baseUrl.LocalPath;

		//SwipeGestureRecognizer swipeGesture = new SwipeGestureRecognizer();


		// Restore app state
		PrimarySwitch.IsToggled = Preferences.Default.Get<bool>("Primary", false);


        myURL.SetBinding(Label.TextProperty, new Binding(nameof(CurrentLocation)));

		myWebView.Source = CurrentLocation;

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

    private void myWebView_Navigating(object sender, WebNavigatingEventArgs e)
    {

    }

    async private void myWebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
		if (e.Result == WebNavigationResult.Success)
		{

			// if url doesn't indicate the dotnet directory or deeper, don't save
			var url = new Uri(e.Url);

			CurrentLocation = e.Url;

			if (e.Url.Equals(_baseUrl))
			{
				return;
			}
			else if(!url.LocalPath.Contains(_baseLocalPath))
			{
				await Task.Delay(1000);

				// return to base
				myWebView.Source = _baseUrl.OriginalString;

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



    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
		// Store app state
		Preferences.Default.Set("Primary", e.Value);
    }

    private async void Button_DeleteDatabase(object sender, EventArgs e)
    {

		bool answer = await DisplayAlert("WARNING", "Are you sure to DELETE the current local database?", "YES", "NO");

		if (answer) _database.ClearDatabaseAsync();
    }

	
}

