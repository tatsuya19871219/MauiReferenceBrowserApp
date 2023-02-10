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
    // Item class for the database
    private class SearchItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string URL { get; set; }
        public int COUNT_MAJOR { get; set; }
        public int COUNT_MINOR { get; set; }
    }


    SQLiteAsyncConnection Database;

    List<SearchItem> _searchItems = new();

    // for store search index
    Dictionary<int, int> _searchIndexByID = new(); // mapping ID to List index
    Dictionary<string, int> _searchIndexByUrl = new(); // mapping Url to List index

    // for notifying database update
    //public Action<List<SearchItem>> Updated;

    //
    public bool IsInitialized = false;

    public SearchItemDatabase()
    {
        Init();
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
                _searchItems.Add(item);

                _searchIndexByID[item.ID] = index;
                _searchIndexByUrl[item.URL] = index++;
            }

            //Updated?.Invoke(_searchItems);
        }
        else
        {
            // just created
        }

        IsInitialized = true;
    }


    //public async Task<List<SearchItem>> GetItemsAsync()
    //{
    //    await Init();

    //    return _searchItems;
    //}

    //public async Task<bool> HasItemByURLAsync(string url)
    //{
    //    var item = await GetItemByUrlAsync(url);

    //    if(item is null) return false;
    //    return true;

    //}


    //public async Task<SearchItem> GetItemByUrlAsync(string url)
    //{
    //    await Init();

    //    return await Database.Table<SearchItem>().Where(x => x.URL == url).FirstOrDefaultAsync();
    //}


    //
    public async void AddNewItemAsync(string url)
    {
        await Init();

        SearchItem item = new SearchItem();
        //item.URL = url;

        _searchItems.Add(item);
        await Database.InsertAsync(item);

        _searchIndexByID[item.ID] = _searchIndexByID.Count;
        _searchIndexByUrl[item.URL] = _searchIndexByUrl.Count;

        //Updated?.Invoke(_searchItems);
    }

    //
    public async void UpdateItemAsync(string url)
    {
        await Init();

        var item = _searchItems[_searchIndexByUrl[url]];
        item.COUNT_MINOR++;

        await Database.UpdateAsync(item);

        //Updated?.Invoke(_searchItems);
    }

    //
    public async void ClearDatabaseAsync()
    {
        await Init();

        await Database.DeleteAllAsync<SearchItem>();

        _searchItems.Clear();
        _searchIndexByID.Clear();
        _searchIndexByUrl.Clear();

        //Updated?.Invoke(_searchItems);
    }


}
