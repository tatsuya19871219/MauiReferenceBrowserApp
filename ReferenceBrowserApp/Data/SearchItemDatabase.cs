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

    public SearchItemDatabase()
    {
    }

    async Task Init()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await Database.CreateTableAsync<SearchItem>();
    }

    //
    public async Task<List<SearchItem>> GetItemsAsync()
    {
        await Init();

        return await Database.Table<SearchItem>().ToListAsync();
    }

    public async Task<bool> HasItemByURLAsync(string url)
    {
        var item = await GetItemByUrlAsync(url);

        if(item is null) return false;
        return true;

    }

    public async Task<SearchItem> GetItemByUrlAsync(string url)
    {
        await Init();

        return await Database.Table<SearchItem>().Where(x => x.URL == url).FirstOrDefaultAsync();
    }

    //
    public async Task<int> SaveItemAsync(SearchItem item)
    {
        await Init();

        if (item.ID != 0)
        {
            return await Database.UpdateAsync(item);
        }
        else
        {
            return await Database.InsertAsync(item);
        }
    }

    //
    public async void ClearDatabaseAsync()
    {
        await Init();

        await Database.DeleteAllAsync<SearchItem>();
    }
}
