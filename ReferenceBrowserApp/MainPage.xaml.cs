using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using ReferenceBrowserApp.Data;
using ReferenceBrowserApp.Models;
using ReferenceBrowserApp.ViewModels;

namespace ReferenceBrowserApp;

public partial class MainPage : ContentPage
{

    public MainPage(WebViewModel vm)
	{
		InitializeComponent();

		InitializeOnPlatform();

		//Application.Current.UserAppTheme = AppTheme.Dark;
		
		BindingContext = vm;

		vm.BindWebView(myWebView);

        vm.SetReferenceSite(new ReferenceSite("maui", "https://learn.microsoft.com/en-us/dotnet/maui/?view=net-maui-7.0"));
        //vm.SetReferenceSite(new ReferenceSite("maui", "https://learn.microsoft.com/en-us/dotnet/maui"));

        //myWebView.Source = "https://learn.microsoft.com/en-us/dotnet/maui/?view=net-maui-7.0";

        HomeButton.Clicked += vm.GoHome;
        PreviousButton.Clicked += vm.GoPrevious;
        NextButton.Clicked += vm.GoNext;

        checkButton.IsEnabled = true;

        myWebView.Navigating += vm.NavigatingCallback;
		myWebView.Navigated += vm.NavigatedCallback;

		SubPage.GoToAction += vm.GoTo;
    }

	partial void InitializeOnPlatform();


    private void CheckButton_Clicked(object sender, EventArgs e)
    {
		MoveToSubPage();
    }

	async void MoveToSubPage() 
		=> await Shell.Current.GoToAsync(nameof(SubPage)); 


    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

		//return;

		myContentView.WidthRequest = width;

		//myContentView.HeightRequest = height - Header.Height - Footer.Height;

#if WINDOWS
		myContentView.HeightRequest = height - Header.Height - Footer.Height;
#elif ANDROID

  //      this.MinimumHeightRequest = 0;
		//this.MinimumWidthRequest = 0;
		//this.HeightRequest = height;
		//this.WidthRequest = width;

		myContentView.HeightRequest = height - Header.Height;
		//myContentView.HeightRequest = 600;

		//Header.WidthRequest = Footer.WidthRequest = width;
#endif

	}

}

