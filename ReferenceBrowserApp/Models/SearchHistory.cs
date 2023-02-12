using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceBrowserApp.Models;

public class SearchHistory
{
    private List<SearchUri> _history = new();

    private int _location; // Browsing location in history list

    private ReferenceSite _reference;

    public SearchHistory(ReferenceSite reference)
    {
        _reference = reference;

        // First item in the history is reference 
        _history.Add(reference);
        _location = 0;
    }

    public bool IsCurrentUri(SearchUri uri)
    {
        return uri.ToString() == _history[_location].ToString();
    }

    /// <summary>
    /// Push search URI to history list if the URI
    /// is in the reference site.
    /// </summary>
    /// <param name="uri">Navigating URI</param>
    /// <returns>true if the URI is in the reference site</returns>
    public bool TryPush(SearchUri uri)
    {
        if (IsCurrentUri(uri)) return false;

        if (_reference.Contains(uri))
        {
            if (_location == _history.Count - 1)
            {
                _history.Add(uri);
                _location++;
            }
            else MakeNewBranch(uri);

            return true;
        }
        else return false;
    }

    /// <summary>
    /// Pop search URI from history list for browsing back.
    /// </summary>
    /// <param name="uri">Navigating URI in browsing back operation</param>
    /// <returns>true if pop from the list is succeeded</returns>
    public bool TryPop(out SearchUri uri)
    {
        if (_location == 0)
        {
            uri = _history[_location];
            
            return false;
        }
        else
        {
            uri = _history[_location-1];
            _location--;

            return true;
        }
    }


    /// <summary>
    /// Delete all forword history from the current location in the list
    /// and push new search URI.
    /// </summary>
    /// <param name="uri">Navigating URI</param>
    private void MakeNewBranch(SearchUri uri)
    {
        if (_history.Count > 1)
            _history.RemoveRange(_location+1, _history.Count-_location-1);

        TryPush(uri);
    }

    //
    public void GoForward()
    {
        if (_location < _history.Count-1) _location++;
    }

    public void GoBack()
    {
        if (_location > 0) _location--;
    }
}
