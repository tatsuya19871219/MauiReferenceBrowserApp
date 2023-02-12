using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceBrowserApp.Models;

public class SearchUri
{
    readonly Uri _uri;

    public string Host => _uri.Host;

    public string LocalPath => _uri.LocalPath;

    public string[] Segments => _uri.Segments;

    //readonly public string AbsoluteUri;

    public SearchUri(string uriString)
    {
        _uri = new Uri(uriString);
        
        //Host = _searchUri.Host;
        //Segments = _searchUri.Segments;

        //AbsoluteUri = _searchUri.AbsoluteUri;
    }

    //public List<string> GetSegments()
    //{
    //    string dummy = _searchUri?.Segments[0];

    //    return new List<string>(_searchUri?.Segments);
    //}

    public override string ToString()
    {
        //return _uri.AbsoluteUri;
        return "https://" + Host + LocalPath;
    }
}
