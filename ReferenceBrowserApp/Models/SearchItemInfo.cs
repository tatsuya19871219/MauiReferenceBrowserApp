using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceBrowserApp.Models;

public partial class SearchItemInfo : ObservableObject
{
    ReferenceSite _reference;
    SearchUri _searchUri;

    [ObservableProperty]
    string uriString;

    //public string PageName { get; private set; }
    [ObservableProperty]
    string pageName;

    [ObservableProperty]
    string directoryName;

    public int Id;

    public int MinorCount;

    public int MajorCount;

    public string Details { get; private set; }

    public string URI { get; private set; }


    public SearchItemInfo(ReferenceSite reference, SearchUri searchUri, int countMajor, int countMinor, int id)
    {
        _reference = reference;
        _searchUri = searchUri;

        UriString = searchUri.ToString();

        Id = id;
        MinorCount = countMinor;
        MajorCount = countMajor;

        //Uri uri;// = new Uri(item.URL);

        URI = UriString;

        string[] segments = searchUri.Segments;

        PageName = segments[segments.Length-1];

        DirectoryName = segments[segments.Length-2];

        if (DirectoryName == "maui/") DirectoryName = "/";

        Details = String.Format("Count {0} ({1})     ID:{2}",
                                MinorCount, MajorCount, Id);
    }
}
