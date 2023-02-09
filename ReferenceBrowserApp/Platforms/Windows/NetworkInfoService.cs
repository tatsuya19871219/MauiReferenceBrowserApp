using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace ReferenceBrowserApp.Services;

public partial class NetworkInfoService
{

    GatewayIPAddressInformation _gateway;
    IPAddress _ipAddress;

    public partial void Invoke()
    {
        //// check default gateway address
        NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface adapter in adapters)
        {
            IPInterfaceProperties adapterPropaties = adapter.GetIPProperties();
            GatewayIPAddressInformationCollection addresses = adapterPropaties.GatewayAddresses; // Windows only

            if (addresses.Count > 0)
            {
                Debug.WriteLine(adapter.Description);
                foreach (GatewayIPAddressInformation address in addresses)
                {
                    Debug.WriteLine("  Gateway Address ......................... : {0}",
                        address.Address.ToString());

                    _gateway = address;
                }
                Debug.WriteLine("");
            }
        }

        // check my ip address
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        //_ipAddress = ipHostInfo.AddressList[5];

        //
        //var gatewayStr = _gateway.Address.ToString().Split('.');
        List<int> gatewayNum = _gateway.Address.ToString().Split('.').Select(p => int.Parse(p)).ToList();

        Debug.Print("Gateway: {0}", string.Join(", ", gatewayNum));

        foreach (IPAddress address in ipHostInfo.AddressList)
        {
            // check suitable gateway for the address
            try
            {
                List<int> ipNum = address.ToString().Split('.').Select(p => int.Parse(p)).ToList();

                Debug.Print("IP address: {0}", string.Join(", ", ipNum));

                if (gatewayNum[2] == ipNum[2]) _ipAddress = address;

            }
            catch
            {

            }
        }

        MyIPAddress = _ipAddress.ToString();
        GatewayIP = _gateway.Address.ToString();

    }
}