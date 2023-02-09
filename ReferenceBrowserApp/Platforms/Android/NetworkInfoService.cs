//using Android.OS;
using Android.Net;
using Android.Telecom;
using Java.Net;
using Java.Util;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        //var ipAddresses = Dns.GetHostAddresses(Dns.GetHostName());
        //foreach (var ipAddress in ipAddresses)
        //{
        //    Debug.Print($"{ipAddress.ToString()}");
        //}

        var AllNetworkInterfaces = Collections.List(Java.Net.NetworkInterface.NetworkInterfaces);

        foreach (var Interface in AllNetworkInterfaces)
        {
            Debug.WriteLine(Interface.ToString());

            var AddressInterface = (Interface as Java.Net.NetworkInterface).InterfaceAddresses;

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

        GatewayIP = MyIPAddress;
    }
}