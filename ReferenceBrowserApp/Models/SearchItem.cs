using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceBrowserApp.Models;

public class SearchItem
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    public string URL { get; set; }

    public int COUNT_MAJOR { get; set; }

    public int COUNT_MINOR { get; set; }

    public SearchItem() 
    {
        COUNT_MINOR = 1;
    }

}
