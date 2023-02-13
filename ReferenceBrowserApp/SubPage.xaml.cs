using Microsoft.Maui.Controls.Platform;
using ReferenceBrowserApp.CustomViews;
using ReferenceBrowserApp.Data;
using ReferenceBrowserApp.Models;
using ReferenceBrowserApp.Services;
using ReferenceBrowserApp.ViewModels;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net;

namespace ReferenceBrowserApp;

public partial class SubPage : ContentPage
{

    readonly SearchItemInfosViewModel _vm;

    readonly ReferenceSearchItemDatabase _database;

    readonly NetworkInfoService _networkInfoService = new();

    DatabaseSyncServerService? _server = null;
    DatabaseSyncClientService? _client = null;


    //public static Action<SearchUri> GoToAction;


    public SubPage(ReferenceSearchItemDatabase database, SearchItemInfosViewModel vm)
	{

        _database = database; // _database is used in initialize component (Switch)

		InitializeComponent();

        InitializeOnPlatform();

        // Get IP Information such as local IP address
        _networkInfoService.Invoke();

        // Restore app state
        PrimarySwitch.IsToggled = Preferences.Default.Get<bool>("Primary", false);

        BindingContext = vm;

        vm.BindDatabase(database);

        //vm.Refresh();

        _vm = vm;


        SearchItemInfoView.GotoPageAction += (uri) =>
        {
            MoveToMainPage();
            //GoToAction?.Invoke(uri);
            WebViewModel.Instance.GoTo(uri);
        };

        UpdateServerIPEntry();

    }

    partial void InitializeOnPlatform();


    // SubPage -> MainPage
    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);
    }

    // MainPage -> SubPage
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        _vm.Refresh();

        base.OnNavigatedTo(args);
    }

    private async void Button_DeleteDatabase(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("WARNING", "Are you sure to DELETE the current local database?", "YES", "NO");

        if (answer)
        {
            await _database.ClearDatabaseAsync();

            _vm.Refresh();

            SearchItemList.Clear();
            
        }
    }

    private void SwitchPrimary_Toggled(object sender, ToggledEventArgs e)
    {
        // Store app state
        Preferences.Default.Set("Primary", e.Value);

        if (e.Value)
        {
            _client?.ShutdownClient();

            // if primary is true, prepare server
            _server = new DatabaseSyncServerService(_database, _networkInfoService);
            _server.Invoke();
        }
        else
        {
            _server?.ShutdownServer();
        }

        UpdateServerIPEntry();
    }

    private async void ButtonSync_Clicked(object sender, EventArgs e)
    {
        // Sync database with the primary app (server)

        // prepare client
        string ip = ServerIP.Text;
        IPAddress targetIPAdress;

        try
        {
            targetIPAdress = IPAddress.Parse(ip);
        }
        catch (Exception ex)
        {
            // fails to parse text
            DisplayAlert("Notification", "Fails to parse IP address from text", "OK");

            return;
        }

        _client = new DatabaseSyncClientService(_database, targetIPAdress);

        await _client.Invoke();

        //SearchItemList.Clear();

        await DisplayAlert("Notification", "Search items are successfully synchronized.", "OK");

        //MoveToMainPage();

        // Reload page
        await Navigation.PopAsync();
        await Shell.Current.GoToAsync(nameof(SubPage));

    }

    private void UpdateServerIPEntry()
    {
        if (PrimarySwitch.IsToggled)
        {
            ServerIP.Text = _networkInfoService?.MyIPAddress.ToString();
            ServerIP.IsEnabled = false;
            ServerIP.TextColor = Colors.Red;
        }
        else
        {
            ServerIP.Text = _networkInfoService?.GatewayIP.ToString();
            ServerIP.IsEnabled = true;
        }
    }


    void BackButton_Clicked(object sender, EventArgs e)
    {
        MoveToMainPage();
    }

    async void MoveToMainPage()
        => await Navigation.PopAsync();

}

public class InvertedBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !((bool)value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !((bool)value);
    }
}