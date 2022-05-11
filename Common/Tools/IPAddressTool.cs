using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tools
{
    public class IPAddressTool
    {
        public static bool IsInRange(string startIpAddr, string endIpAddr, string address)
        {
            long ipStart = BitConverter.ToInt32(IPAddress.Parse(startIpAddr).GetAddressBytes().Reverse().ToArray(), 0);

            long ipEnd = BitConverter.ToInt32(IPAddress.Parse(endIpAddr).GetAddressBytes().Reverse().ToArray(), 0);

            long ip = BitConverter.ToInt32(IPAddress.Parse(address).GetAddressBytes().Reverse().ToArray(), 0);

            return ip >= ipStart && ip <= ipEnd;
        }

        /// <summary>
        /// https://stackoverflow.com/questions/6803073/get-local-ip-address
        /// 로컬 아이피 가져오기.
        /// </summary>
        /// <returns>로컬 아이피.</returns>
        public static string LocalIpAddress
        {
            get
            {
                foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }

                throw new Exception("No network adapters with an IPv4 address in the system!");
            }
        }
    }
}
