using ReferenceBrowserApp.Data;
using ReferenceBrowserApp.Models;
using System.Collections.ObjectModel;

namespace ReferenceBrowserApp;

public partial class SubPage : ContentPage
{

	SearchItemDatabase _database;

	public ObservableCollection<SearchItem> Items { get; set; } = new();

	public SubPage(SearchItemDatabase database)
	{
		InitializeComponent();

		BindingContext= this;

		_database = database;
		//ProcessDatabase(database);
		
	}

	//async void ProcessDatabase()
	//{
 //       var items = await _database.GetItemsAsync();

	//	//Items = new ObservableCollection<SearchItem>(items); // NG

	//	MainThread.BeginInvokeOnMainThread(() =>
	//	{
	//		Items.Clear();
	//		foreach (var item in items)
	//			Items.Add(item);
	//	});
 //   }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

		//ProcessDatabase();
    }
}