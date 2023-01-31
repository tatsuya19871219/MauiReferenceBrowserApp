using ReferenceBrowserApp.Data;
using ReferenceBrowserApp.Models;
using System.Collections.ObjectModel;

namespace ReferenceBrowserApp;

public partial class SubPage : ContentPage
{
	public ObservableCollection<SearchItem> Items { get; set; } = new();

	public SubPage(SearchItemDatabase database)
	{
		InitializeComponent();

		BindingContext= this;

		ProcessDatabase(database);
		
	}

	async void ProcessDatabase(SearchItemDatabase database)
	{
        var items = await database.GetItemsAsync();

		//Items = new ObservableCollection<SearchItem>(items); // NG

		MainThread.BeginInvokeOnMainThread(() =>
		{
			Items.Clear();
			foreach (var item in items)
				Items.Add(item);
		});
    }

}