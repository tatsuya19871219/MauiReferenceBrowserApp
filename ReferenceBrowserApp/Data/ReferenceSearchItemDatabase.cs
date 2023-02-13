using ReferenceBrowserApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceBrowserApp.Data;

public class ReferenceSearchItemDatabase : SearchItemDatabase
{
    ReferenceSite _reference;

    public ReferenceSearchItemDatabase() : base()
    { 
    }

    public void SetReferenceSite(ReferenceSite reference)
    {
        _reference = reference;
    }


    async public Task<bool> TryPushSearchUri(SearchUri uri, int count = 1)
    {
        if (_reference.Contains(uri))
        {
            if (_reference.ToString() == uri.ToString()) return false;

            if (HasItemBySearchUri(uri)) await UpdateItemAsync(uri, count);
            else await AddNewItemAsync(uri, count);

            return true;
        }

        return false;
    }

    public List<SearchItemInfo> GetSearchItemInfos()
    {
        var list = new List<SearchItemInfo>();
        
        if (_reference != null)
        {
            foreach (var item in GetItems())
                list.Add(ConstructSearchItemInfo(item));
        }

        return list;
    }

    SearchItemInfo ConstructSearchItemInfo(SearchItem item)
    {
        return new SearchItemInfo(_reference, new SearchUri(item.URL), item.COUNT_MAJOR, item.COUNT_MINOR, item.ID);
    }

    async public Task PreparationToSync()
    {
        await UpdateDatabaseAsMajorAsync();
    }

}
