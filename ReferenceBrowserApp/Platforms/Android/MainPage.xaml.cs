using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceBrowserApp;

public partial class MainPage
{
    partial void InitializeOnPlatform()
    {
        MainActivity.Instance.SetBackPressedAction(typeof(MainPage), MoveToSubPage);
    }

}
