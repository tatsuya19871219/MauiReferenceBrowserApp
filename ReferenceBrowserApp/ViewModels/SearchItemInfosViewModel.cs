using ReferenceBrowserApp.Data;
using ReferenceBrowserApp.Models;
using System.Collections.ObjectModel;

namespace ReferenceBrowserApp.ViewModels;

public class SearchItemInfosViewModel 
{

	SearchItemDatabase _database;

	public ObservableCollection<SearchItemInfo> SearchItemInfos { get; private set; } = new();

	public SearchItemInfosViewModel()
	{

	}

	public void BindDatabase(SearchItemDatabase database)
	{
		_database = database;

		//database.Updated += DatabaseUpdated;    
    }

	async public void Refresh()
	{
		//var items = await _database.GetItemsAsync();

		//UpdateViewModel(items);
	}

	void DatabaseUpdated(List<SearchItem> items)
	{
		UpdateViewModel(items);   
	}

	void UpdateViewModel(List<SearchItem> items) 
	{
        SearchItemInfos.Clear();

        foreach (var item in items)
        {
            SearchItemInfos.Add(new SearchItemInfo(item));
        }
    }
}