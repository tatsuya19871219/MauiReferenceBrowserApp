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
    public string[] Segments => _uri.Segments;

    public SearchUri(string uriString)
    {
        _uri = new Uri(uriString);
    }
}
