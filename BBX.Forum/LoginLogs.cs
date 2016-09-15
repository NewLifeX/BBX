using System;
using BBX.Common;
using BBX.Entity;

namespace BBX.Forum
{
	public class LoginLogs
	{
		private static object lockHelper = new object();
		public static int UpdateLoginLog(string ip, bool update)
		{
			int result;
			lock (lockHelper)
			{
				var ff = Failedlogin.FindByIP(ip);
				if (ff != null)
				{
					int num = ff.ErrCount;
					if (ff.LastUpdate.AddMinutes(15) > DateTime.Now)
					{
						if (num >= 5 || !update)
						{
							result = num;
						}
						else
						{
							Failedlogin.AddErrLoginCount(ip);
							result = num + 1;
						}
					}
					else
					{
						Failedlogin.ResetErrLoginCount(ip);
						result = 1;
					}
				}
				else
				{
					if (update)
					{
						Failedlogin.AddErrLoginRecord(ip);
					}
					result = 1;
				}
			}
			return result;
		}
		public static int DeleteLoginLog(string ip)
		{
			if (!Utils.IsIP(ip)) return 0;

			return Failedlogin.DeleteErrLoginRecord(ip);
		}
	}
}