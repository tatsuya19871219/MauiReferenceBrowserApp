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
}
