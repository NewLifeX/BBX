using System;
using System.Collections.Generic;
using System.Data;
using Discuz.Cache;
using Discuz.Common;

using Discuz.Data;
using Discuz.Entity;

namespace Discuz.Forum
{
    public class Helps
    {
        private static List<HelpInfo> helpListTree;

        public static List<HelpInfo> GetHelpList()
        {
            var cacheService = DNTCache.Current;
            Helps.helpListTree = (cacheService.RetrieveObject("/Forum/helplist") as List<HelpInfo>);
            if (Helps.helpListTree == null)
            {
                Helps.helpListTree = new List<HelpInfo>();
                List<HelpInfo> helpList = Help.GetHelpList();
                Helps.CreateHelpTree(helpList, 0);
                cacheService.AddObject("/Forum/helplist", Helps.helpListTree);
            }
            return Helps.helpListTree;
        }

        private static void CreateHelpTree(List<HelpInfo> helpList, int id)
        {
            foreach (HelpInfo current in helpList)
            {
                if (current.Pid == id)
                {
                    Helps.helpListTree.Add(current);
                    Helps.CreateHelpTree(helpList, current.Id);
                }
            }
        }

        public static HelpInfo GetMessage(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            return Help.GetMessage(id);
        }

        public static void UpdateHelp(int id, string title, string message, int pid, int orderby)
        {
            if (id > 0)
            {
                Help.UpdateHelp(id, title, message, pid, orderby);
            }
            DNTCache.Current.RemoveObject("/Forum/helplist");
        }

        public static void AddHelp(string title, string message, int pid)
        {
            Help.AddHelp(title, message, pid);
            DNTCache.Current.RemoveObject("/Forum/helplist");
        }

        public static void DelHelp(string idlist)
        {
            Help.DelHelp(idlist);
            DNTCache.Current.RemoveObject("/Forum/helplist");
        }

        public static DataTable GetHelpTypes()
        {
            return Help.GetHelpTypes();
        }

        public static List<HelpInfo> GetHelpList(int helpid)
        {
            List<HelpInfo> list = new List<HelpInfo>();
            foreach (HelpInfo current in Helps.GetHelpList())
            {
                if (current.Id == helpid || current.Pid == helpid)
                {
                    list.Add(current);
                }
            }
            return list;
        }

        public static bool UpOrder(string[] orderlist, string[] idlist)
        {
            if (orderlist.Length != idlist.Length)
            {
                return false;
            }
            for (int i = 0; i < orderlist.Length; i++)
            {
                string expression = orderlist[i];
                if (!Utils.IsNumeric(expression))
                {
                    return false;
                }
            }
            for (int j = 0; j < idlist.Length; j++)
            {
                Help.UpdateOrder(orderlist[j].ToString(), idlist[j].ToString());
            }
            DNTCache.Current.RemoveObject("/Forum/helplist");
            return true;
        }
    }
}