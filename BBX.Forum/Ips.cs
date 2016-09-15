using System;
using System.Collections.Generic;
using Discuz.Common;
using Discuz.Entity;
using NewLife.IP;

namespace Discuz.Forum
{
    public class Ips
    {
        public static void AddBannedIp(string ipkey, double deteline, string username)
        {
            string[] array = ipkey.Split('.');
            deteline = ((deteline == 0.0) ? 1.0 : Math.Round(deteline));
            if ((Utils.StrToInt(array[0], 0) < 255 || Utils.StrToInt(array[1], 0) < 255 || Utils.StrToInt(array[2], 0) < 255 || Utils.StrToInt(array[3], 0) < 255) && array[0] != "0" && array[1] != "0" && array[2] != "0" && array[3] != "0")
            {
                Discuz.Data.Ips.AddBannedIp(new IpInfo
                {
                    Ip1 = TypeConverter.StrToInt(array[0]),
                    Ip2 = TypeConverter.StrToInt(array[1], 0),
                    Ip3 = TypeConverter.StrToInt(array[2], 0),
                    Ip4 = TypeConverter.StrToInt(array[3], 0),
                    Username = username,
                    Dateline = DateTime.Now.ToShortDateString(),
                    Expiration = DateTime.Now.AddDays(deteline).ToString("yyyy-MM-dd")
                });
            }
        }

        public static List<IpInfo> GetBannedIpList()
        {
            var bannedIpList = Discuz.Data.Ips.GetBannedIpList();
            foreach (var current in bannedIpList)
            {
                current.Location = Ips.GetLocation(current);
            }
            return bannedIpList;
        }

        public static string GetLocation(IpInfo info)
        {
            string iPValue = String.Format("{0}.{1}.{2}.{3}", info.Ip1, info.Ip2, info.Ip3, info.Ip4);
            var addr = Ip.GetAddress(iPValue);
            if (!String.IsNullOrEmpty(addr)) return addr;

            return "未知地址";
        }

        public static List<IpInfo> GetBannedIpList(int num, int pageid, out int counts)
        {
            var bannedIpList = Discuz.Data.Ips.GetBannedIpList(num, pageid);
            foreach (var current in bannedIpList)
            {
                current.Location = Ips.GetLocation(current);
            }
            counts = Discuz.Data.Ips.GetBannedIpCount();
            return bannedIpList;
        }

        public static void DelBanIp(string iplist)
        {
            if (!Utils.IsNumericList(iplist))
            {
                return;
            }
            Discuz.Data.Ips.DelBanIp(iplist);
        }

        public static void EditBanIp(string[] expiration, string[] hiddenexpiration, string[] hiddenid, int useradminid, int userid)
        {
            for (int i = 0; i < expiration.Length; i++)
            {
                var uid = Int32.Parse(hiddenid[i]);
                var user = User.FindByID(uid);
                if ((useradminid == 1 || userid == user.ID) && expiration[i] != hiddenexpiration[i])
                {
                    Discuz.Data.Ips.EditBanIp(Utils.StrToInt(hiddenid[i].ToString(), -1), expiration[i]);
                }
            }
        }
    }
}