using ReferenceBrowserApp.Data;
using ReferenceBrowserApp.Models;
using System.Collections.ObjectModel;

namespace ReferenceBrowserApp.ViewModels;

public class SearchItemInfosViewModel 
{

	ReferenceSearchItemDatabase _database;

	public ObservableCollection<SearchItemInfo> SearchItemInfos { get; private set; } = new();

	public SearchItemInfosViewModel()
	{

	}

	public void BindDatabase(ReferenceSearchItemDatabase database)
	{
		_database = database;
    }

	public void Refresh()
	{
		SearchItemInfos.Clear();

		var list = _database.GetSearchItemInfos();
		
		foreach (var info in list)
		{
			SearchItemInfos.Add(info);
		}
		
	}

}