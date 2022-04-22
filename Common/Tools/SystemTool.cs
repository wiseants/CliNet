using System;
using System.Net.NetworkInformation;

namespace Common.Tools
{
    /// <summary>
    /// 시스템 관련 툴.
    /// </summary>
    public class SystemTool
    {
        /// https://www.codeproject.com/Questions/371096/get-maq-address-in-message-box-using-csharp
        /// <summary>
        /// MAC 주소 반환.
        /// </summary>
        /// <returns></returns>
        public static PhysicalAddress MacAddress
        {
            get
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        return nic.GetPhysicalAddress();
                    }
                }

                return null;
            }
        }

        // https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa
        /// <summary>
        /// 유닉스타임 컨버터.
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime JavaTimeStampToDateTime(double javaTimeStamp)
        {
            // Java timestamp is milliseconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(javaTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
