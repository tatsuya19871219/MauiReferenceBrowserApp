using ReferenceBrowserApp.Data;
using ReferenceBrowserApp.Models;

namespace ReferenceBrowserApp;

public partial class MainPage : ContentPage
{
	SearchItemDatabase _database; 

	public MainPage(SearchItemDatabase database)
	{
		InitializeComponent();

		_database = database;
		//BindingContext = this;

		//SwipeGestureRecognizer swipeGesture = new SwipeGestureRecognizer();

		//_ = Initialize();

	}

	
	async Task Initialize()
	{
		
	}

	

    private void Button_Clicked(object sender, EventArgs e)
    {
		MoveToSubPage();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

		myContentView.WidthRequest = width;
		myContentView.HeightRequest = height * 0.9;
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

    private void myWebView_Navigated(object sender, WebNavigatedEventArgs e)
    {
		if (e.Result == WebNavigationResult.Success)
		{
			// 
			SearchItem item = new SearchItem();
			item.URL= e.Url;

			_ = _database.SaveItemAsync(item);

		}
    }

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
	}

	protected override async void OnNavigatedFrom(NavigatedFromEventArgs args)
	{
		base.OnNavigatedFrom(args);
	}

    protected override async void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);
    }
}

