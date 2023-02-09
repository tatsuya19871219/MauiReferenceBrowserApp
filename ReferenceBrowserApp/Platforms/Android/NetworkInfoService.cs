//using Android.OS;
using Android.Net;
using Android.Telecom;
using Java.Net;
using Java.Util;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
//using System.Net.NetworkInformation;
//using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceBrowserApp.Services;

public partial class NetworkInfoService
{

    InetAddress _ipAddress;
    InetAddress _broadcast;

    public partial void Invoke()
    {

        var AllNetworkInterfaces = Collections.List(NetworkInterface.NetworkInterfaces);

        foreach (NetworkInterface Interface in AllNetworkInterfaces)
        {
            Debug.WriteLine(Interface.ToString());

            var AddressInterface = Interface.InterfaceAddresses;

            foreach (var AInterface in AddressInterface)
            {
                Debug.WriteLine($"\t{AInterface.ToString()}");

                if (AInterface.Broadcast != null)
                {
                    _ipAddress = AInterface.Address;
                    _broadcast = AInterface.Broadcast;
                }
            }

        }


        MyIPAddress = _ipAddress.HostAddress;

        GatewayIP = _broadcast.HostAddress.ToString().Split('.')
                                            .Select(p => p == "255" ? "1" : p)
                                            .ToList()
                                            .Aggregate((text, next) => text + "." + next);
    }
}