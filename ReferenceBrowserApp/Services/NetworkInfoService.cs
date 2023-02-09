using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceBrowserApp.Services;

public partial class NetworkInfoService
{
    public string MyIPAddress { get; private set; }

    public string GatewayIP { get; private set; }

    public partial void Invoke();

}