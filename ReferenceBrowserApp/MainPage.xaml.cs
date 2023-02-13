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

		Application.Current.UserAppTheme = AppTheme.Dark;
		
		BindingContext = vm;

		vm.BindWebView(myWebView);

        vm.SetReferenceSite(new ReferenceSite("MAUI", "https://learn.microsoft.com/en-us/dotnet/maui/?view=net-maui-7.0"));

        HomeButton.Clicked += vm.GoHome;
        PreviousButton.Clicked += vm.GoPrevious;
        NextButton.Clicked += vm.GoNext;

        checkButton.IsEnabled = true;

        myWebView.Navigating += vm.NavigatingCallback;
		myWebView.Navigated += vm.NavigatedCallback;

		//SubPage.GoToAction += vm.GoTo;
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

		myContentView.WidthRequest = width;

#if WINDOWS
		myContentView.HeightRequest = height - Header.Height - Footer.Height;
#elif ANDROID
		myContentView.HeightRequest = height - Header.Height;
#endif

	}

}

