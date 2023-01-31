using CommunityToolkit.Maui.Views;
using ReferenceBrowserApp.Data;
using ReferenceBrowserApp.Models;

namespace ReferenceBrowserApp;

public partial class MainPage : ContentPage
{
	SearchItemDatabase _database;

	Uri _baseUrl = new Uri("https://learn.microsoft.com/en-us/dotnet/maui/");

	string _baseLocalPath;

    public MainPage(SearchItemDatabase database)
	{
		InitializeComponent();

		_database = database;

		myWebView.Source = _baseUrl;

		_baseLocalPath = _baseUrl.LocalPath;

		//BindingContext = this;

		//SwipeGestureRecognizer swipeGesture = new SwipeGestureRecognizer();

		//_ = Initialize();

		// Restore app state
		PrimarySwitch.IsToggled = Preferences.Default.Get<bool>("Primary", false);

		// for test
		//_database.ClearDatabaseAsync();
	}

	
	//async Task Initialize()
	//{
		
	//}

	

    private void Button_Clicked(object sender, EventArgs e)
    {
		MoveToSubPage();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

		myContentView.WidthRequest = width;
		myContentView.HeightRequest = height - Header.Height - Footer.Height;

		CurrentLocation.WidthRequest = width * 0.6;
    }

    async private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
    {
        //Page page = (Page)Activator.CreateInstance(typeof(SubPage));
        SubPage page = (SubPage)Activator.CreateInstance(typeof(SubPage));
        await Navigation.PushAsync(page);
    }

	async void MoveToSubPage()
	{
        SubPage page = (SubPage)Activator.CreateInstance(typeof(SubPage));

		page.SetCurrentDatabase(_database);

        await Navigation.PushAsync(page);
    }

    private void myWebView_Navigating(object sender, WebNavigatingEventArgs e)
    {

    }

    async private void myWebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
		if (e.Result == WebNavigationResult.Success)
		{

			// if url doesn't indicate the dotnet directory or deeper, don't save
			var url = new Uri(e.Url);

			CurrentLocation.Text = e.Url;

			if(!url.LocalPath.Contains(_baseLocalPath))
			{
				await Task.Delay(1000);

				// return to base
				myWebView.Source = _baseUrl.OriginalString;

				return;
			}

			//HttpStyleUriParser parser = new HttpStyleUriParser(); how to use this?
			

            if(!await _database.HasItemByURLAsync(e.Url))
			{
				SearchItem item = new SearchItem();
				item.URL= e.Url;

				_ = _database.SaveItemAsync(item);

			}
			else
			{
				var item = await _database.GetItemByUrlAsync(e.Url);

				// add count
				if(PrimarySwitch.IsToggled)
				{
					item.COUNT_MAJOR++;
				}
				else
				{
					item.COUNT_MINOR++;
				}

				_ = _database.SaveItemAsync(item);
			}

		}
    }

	//protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	//{
	//	base.OnNavigatedTo(args);
	//}

	//protected override async void OnNavigatedFrom(NavigatedFromEventArgs args)
	//{
	//	base.OnNavigatedFrom(args);
	//}

 //   protected override async void OnNavigatingFrom(NavigatingFromEventArgs args)
 //   {
 //       base.OnNavigatingFrom(args);
 //   }

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
		// Store app state
		Preferences.Default.Set("Primary", e.Value);
    }

    private async void Button_DeleteDatabase(object sender, EventArgs e)
    {
		//var popup = new Popup
		//{
		//	Content = new VerticalStackLayout
		//	{
		//		Children =
		//		{
		//			new Label
		//			{
		//				Text = "Delete"
		//			}
		//		}
		//	}
		//};

		//this.ShowPopup(popup);

		bool answer = await DisplayAlert("WARNING", "Are you sure to DELETE the current local database?", "YES", "NO");

		if (answer) _database.ClearDatabaseAsync();
    }

	
}

