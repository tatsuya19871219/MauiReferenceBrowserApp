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
        
    }

    public override string ToString()
    {
        //return _uri.AbsoluteUri;
        return "https://" + Host + LocalPath;
    }
}
