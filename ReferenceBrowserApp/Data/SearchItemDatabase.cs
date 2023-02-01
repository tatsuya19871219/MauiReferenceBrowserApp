using ReferenceBrowserApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceBrowserApp.Data;

public class SearchItemDatabase
{
    SQLiteAsyncConnection Database;

    public List<SearchItem> SearchItems { get; private set; } = new();

    // for store search index
    Dictionary<int, int> _searchIndexByID = new(); // mapping ID to List index
    Dictionary<string, int> _searchIndexByUrl = new(); // mapping Url to List index

    public SearchItemDatabase()
    {
    }

    async Task Init()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        var result = await Database.CreateTableAsync<SearchItem>();

        // initialize SearchItems List
        if (result == CreateTableResult.Migrated)
        {
            var items = await Database.Table<SearchItem>().ToListAsync();

            int index = 0;
            foreach(var item in items)
            {
                SearchItems.Add(item);

                _searchIndexByID[item.ID] = index;
                _searchIndexByUrl[item.URL] = index++;
            }
        }

    }

    //
    //public async Task<List<SearchItem>> GetItemsAsync()
    //{
    //    await Init();

    //    return await Database.Table<SearchItem>().ToListAsync();
    //}

    public async Task<bool> HasItemByURLAsync(string url)
    {
        var item = await GetItemByUrlAsync(url);

        if(item is null) return false;
        return true;

    }

    public bool HasItemByURL(string url)
    {
        return _searchIndexByUrl.TryGetValue(url, out _);
    }

    public async Task<SearchItem> GetItemByUrlAsync(string url)
    {
        await Init();

        return await Database.Table<SearchItem>().Where(x => x.URL == url).FirstOrDefaultAsync();
    }

    //
    //public async void SaveItemAsync(SearchItem item)
    //{
    //    await Init();

    //    if (item.ID != 0)
    //    {
    //        SearchItems[_searchIndexByID[item.ID]] = item;

    //        await Database.UpdateAsync(item);
    //    }
    //    else
    //    {
    //        SearchItems.Add(item);
    //        await Database.InsertAsync(item);
            
    //        //// check new item ID 
    //        //int id = (await GetItemByUrlAsync(item.URL)).ID;

    //        // item.ID is updated in InsertAsync call.

    //        _searchIndexByID[item.ID] = _searchIndexByID.Count;
    //    }
    //}


    //
    public async void AddNewItemAsync(string url)
    {
        await Init();

        SearchItem item = new SearchItem();
        item.URL = url;

        SearchItems.Add(item);
        await Database.InsertAsync(item);

        _searchIndexByID[item.ID] = _searchIndexByID.Count;
        _searchIndexByUrl[item.URL] = _searchIndexByUrl.Count;
    }

    //
    public async void UpdateItemAsync(string url)
    {
        await Init();

        var item = SearchItems[_searchIndexByUrl[url]];
        item.COUNT_MINOR++;

        await Database.UpdateAsync(item);
    }

    //
    public async void ClearDatabaseAsync()
    {
        await Init();

        await Database.DeleteAllAsync<SearchItem>();

        SearchItems.Clear();
    }
}
