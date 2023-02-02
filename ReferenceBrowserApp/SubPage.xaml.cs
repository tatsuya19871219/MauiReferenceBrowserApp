using ReferenceBrowserApp.Data;
using ReferenceBrowserApp.Models;
using ReferenceBrowserApp.ViewModels;
using System.Collections.ObjectModel;

namespace ReferenceBrowserApp;

public partial class SubPage : ContentPage
{

	SearchItemDatabase _database;


	public SubPage(SearchItemDatabase database, SearchItemInfosViewModel vm)
	{
		InitializeComponent();

        // Restore app state
        PrimarySwitch.IsToggled = Preferences.Default.Get<bool>("Primary", false);

        BindingContext = vm;

		_database = database;

		vm.BindDatabase(database);

        vm.Refresh();
	}

    async protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    //private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    //{

    //}

    private async void Button_DeleteDatabase(object sender, EventArgs e)
    {

        bool answer = await DisplayAlert("WARNING", "Are you sure to DELETE the current local database?", "YES", "NO");

        if (answer) _database.ClearDatabaseAsync();
    }

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
        // Store app state
        Preferences.Default.Set("Primary", e.Value);
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        itemListView.HeightRequest = height*0.8;
    }
}