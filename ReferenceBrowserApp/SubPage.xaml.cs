//using Org.Apache.Http.Client;
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

    SearchItemDatabase _database;

    NetworkInfoService _networkInfoService = new();

    DatabaseSyncServerService? _server = null;
    DatabaseSyncClientService? _client = null;


    public SubPage(SearchItemDatabase database, SearchItemInfosViewModel vm)
	{
		InitializeComponent();

        // Get IP Information such as local IP address
        _networkInfoService.Invoke();

        // Restore app state
        PrimarySwitch.IsToggled = Preferences.Default.Get<bool>("Primary", false);

        BindingContext = vm;

        _database = database;

        vm.BindDatabase(database);

        vm.Refresh();

        UpdateServerIPEntry();

        //SwipeGestureRecognizer swipeGestureRecognizer = new SwipeGestureRecognizer();
        //swipeGestureRecognizer.Swiped += SwipeGestureRecognizer_Swiped;
        
        //swipeGestureRecognizer.Direction = SwipeDirection.Up;

        //SearchItemScrollView.GestureRecognizers.Add(swipeGestureRecognizer);
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

    private void SwitchPrimary_Toggled(object sender, ToggledEventArgs e)
    {
        // Store app state
        Preferences.Default.Set("Primary", e.Value);

        if (e.Value)
        {
            _client?.ShutdownClient();

            // if primary is true, prepare server
            _server = new DatabaseSyncServerService(_networkInfoService);
        }
        else
        {
            _server?.ShutdownServer();
        }

        UpdateServerIPEntry();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        //itemListView.HeightRequest = height*0.8;
    }

    private void ButtonSync_Clicked(object sender, EventArgs e)
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
            // Alart
            return;
        }

        _client = new DatabaseSyncClientService(targetIPAdress);

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

    private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
    {
        //(sender as ScrollView)?.
    }
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