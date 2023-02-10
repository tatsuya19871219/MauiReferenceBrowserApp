using Android.Icu.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceBrowserApp.Models;

public class ReferenceSite
{
    readonly SearchUri _referenceUri;

    readonly public string Name;
    readonly public string URI;

    public ReferenceSite(string id, string uri)
    {
        Name = id; URI = uri;

        _referenceUri = new SearchUri(uri);
    }


    /// <summary>
    /// Judge if search URI is in the reference site
    /// </summary>
    /// <param name="searchUri">Navigating URI</param>
    /// <returns>true if search URI is in the reference site</returns>
    public bool Contains(SearchUri searchUri)
    {
        if (_referenceUri.Host != searchUri.Host) return false;

        string[] searchSegments = searchUri.Segments;
        string[] refSegments = _referenceUri.Segments;

        for (int i=0; i< refSegments.Length; i++)
            if (refSegments[i] != searchUri.Segments[i]) return false;

        return true;
    }

}
