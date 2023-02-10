using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using ReferenceBrowserApp.Data;
using ReferenceBrowserApp.Models;
using ReferenceBrowserApp.ViewModels;

namespace ReferenceBrowserApp;

public partial class MainPage : ContentPage
{

	//List<ReferenceSite> _sites = new();

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

    public MainPage(SearchItemDatabase database, WebViewModel vm)
	{
		InitializeComponent();

		//Application.Current.UserAppTheme = AppTheme.Dark;
		
		_database = database;

		BindingContext = this;

		myWebView.BindingContext = vm;

		//_sites.Add(new ReferenceSite("maui", "https://learn.microsoft.com/en-us/dotnet/maui/?view=net-maui-7.0"));

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

	async private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
	{
		//Page page = (Page)Activator.CreateInstance(typeof(SubPage));
		//SubPage page = (SubPage)Activator.CreateInstance(typeof(SubPage));
		//await Navigation.PushAsync(page);
	}

	async void MoveToSubPage() 
		=> await Shell.Current.GoToAsync(nameof(SubPage)); 

    

    private void ClickGestureRecognizer_Clicked(object sender, EventArgs e)
    {

    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }
}

