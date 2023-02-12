using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceBrowserApp.Models;

public class ReferenceSite : SearchUri
{
    //readonly SearchUri _referenceUri;

    readonly public string Name;
    //readonly public string URI;

    //readonly public string AbsoluteUri;

    public ReferenceSite(string name, string uri) : base(uri)
    {
        Name = name; 
        //URI = uri;

        //_referenceUri = new SearchUri(uri);

        //AbsoluteUri = _referenceUri.AbsoluteUri;
    }

    //public SearchUri GetURI() => _referenceUri;


    /// <summary>
    /// Judge if search URI is in the reference site
    /// </summary>
    /// <param name="searchUri">Navigating URI</param>
    /// <returns>true if search URI is in the reference site</returns>
    public bool Contains(SearchUri searchUri)
    {
        if (this.Host != searchUri.Host) return false;

        string[] searchSegments = searchUri.Segments;
        string[] refSegments = this.Segments;

        if (searchSegments.Length < refSegments.Length) return false;

        for (int i=0; i< refSegments.Length; i++)
            if (refSegments[i] != searchSegments[i]) return false;

        return true;
    }

}
