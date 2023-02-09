using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceBrowserApp.Models;

public partial class SearchItemInfo : ObservableObject
{
    public SearchItem Item { get; private set; }

    public int ID => Item.ID;

    //public string PageName { get; private set; }
    [ObservableProperty]
    public string pageName;

    public string DirectoryName { get; private set; }

    public int MinorCount => Item.COUNT_MINOR;

    public int MajorCount => Item.COUNT_MAJOR;

    public string Details { get; private set; }

    public string URI { get; private set; }

    public SearchItemInfo(SearchItem item)
    {
        Item = item;

        //Id = item.ID;
        //MinorCount = item.COUNT_MINOR;
        //MajorCount = item.COUNT_MAJOR;

        var uri = new Uri(item.URL);

        URI = uri.ToString();

        string[] segments = uri.Segments;

        PageName = segments[segments.Length-1];

        DirectoryName = segments[segments.Length-2];

        if (DirectoryName == "maui/") DirectoryName = "/";

        Details = String.Format("Count {0} ({1})     ID:{2}",
            MinorCount, MajorCount, ID);
    }
}
