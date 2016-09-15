using System;
using System.Collections.Generic;
using Discuz.Common;

using Discuz.Config;
using Discuz.Data;
using Discuz.Entity;

namespace Discuz.Forum
{
    public class Invitation
    {
        private static string BuildInviteCode()
        {
            string[] array = new string[]
			{
				"A",
				"B",
				"C",
				"D",
				"E",
				"F",
				"G",
				"H",
				"I",
				"J",
				"K",
				"L",
				"M",
				"N",
				"O",
				"P",
				"Q",
				"R",
				"S",
				"T",
				"U",
				"V",
				"W",
				"X",
				"Y",
				"Z"
			};
            string text = "";
            Random random = new Random();
            int num = 0;
            while (num++ < 7)
            {
                text += array[random.Next(0, 25)];
            }
            return text;
        }

        private static bool IsInviteCodeExist(string code)
        {
            return Discuz.Data.Invitation.IsInviteCodeExist(code);
        }

        public static int CreateInviteCode(ShortUserInfo userInfo)
        {
            InvitationConfigInfo config = InvitationConfigInfo.Current;
            InviteCodeInfo inviteCodeInfo = new InviteCodeInfo();
            inviteCodeInfo.CreatorId = userInfo.Uid;
            inviteCodeInfo.Creator = userInfo.Username;
            inviteCodeInfo.Code = Invitation.BuildInviteCode();
            while (Invitation.IsInviteCodeExist(inviteCodeInfo.Code))
            {
                inviteCodeInfo.Code = Invitation.BuildInviteCode();
            }
            inviteCodeInfo.CreateTime = Utils.GetDateTime();
            inviteCodeInfo.InviteType = GeneralConfigInfo.Current.Regstatus;
            inviteCodeInfo.ExpireTime = Utils.GetDateTime(config.InviteCodeExpireTime);
            if (inviteCodeInfo.InviteType == 3)
            {
                inviteCodeInfo.MaxCount = ((config.InviteCodeMaxCount > 1) ? config.InviteCodeMaxCount : 1);
            }
            else
            {
                inviteCodeInfo.MaxCount = config.InviteCodeMaxCount;
            }
            return Discuz.Data.Invitation.CreateInviteCode(inviteCodeInfo);
        }

        public static InviteCodeInfo GetInviteCodeByUid(int userId)
        {
            if (userId > 0)
            {
                return Discuz.Data.Invitation.GetInviteCodeByUid(userId);
            }
            return null;
        }

        public static InviteCodeInfo GetInviteCodeById(int inviteId)
        {
            return Discuz.Data.Invitation.GetInviteCodeById(inviteId);
        }

        public static InviteCodeInfo GetInviteCodeByCode(string code)
        {
            return Discuz.Data.Invitation.GetInviteCodeByCode(code);
        }

        public static void DeleteInviteCode(int inviteId)
        {
            Discuz.Data.Invitation.DeleteInviteCode(inviteId);
        }

        public static void UpdateInviteCodeSuccessCount(int inviteId)
        {
            Discuz.Data.Invitation.UpdateInviteCodeSuccessCount(inviteId);
        }

        public static bool CheckInviteCode(InviteCodeInfo inviteCode)
        {
            GeneralConfigInfo config = GeneralConfigInfo.Current;
            if (inviteCode == null)
            {
                return false;
            }
            if (inviteCode.InviteType != config.Regstatus)
            {
                return false;
            }
            if (inviteCode.CreateTime != inviteCode.ExpireTime && Utils.StrDateDiffHours(inviteCode.ExpireTime, 0) > 0)
            {
                return false;
            }
            int num = (inviteCode.InviteType == 2) ? InvitationConfigInfo.Current.InviteCodeMaxCount : inviteCode.MaxCount;
            return num <= 0 || inviteCode.SuccessCount < num;
        }

        public static void ConvertInviteCodeToCredits(InviteCodeInfo inviteCode, int inviteCodePayCount)
        {
            int num = inviteCode.SuccessCount - inviteCodePayCount;
            if (num > -1)
            {
                CreditsFacade.Invite(inviteCode.CreatorId, inviteCode.SuccessCount);
            }
        }

        public static int GetUserInviteCodeCount(int creatorId)
        {
            if (creatorId > 0)
            {
                return Discuz.Data.Invitation.GetUserInviteCodeCount(creatorId);
            }
            return 0;
        }

        public static List<InviteCodeInfo> GetUserInviteCodeList(int creatorId, int pageIndex)
        {
            if (creatorId > 0)
            {
                if (pageIndex == 0)
                {
                    pageIndex = 1;
                }
                return Discuz.Data.Invitation.GetUserInviteCodeList(creatorId, pageIndex);
            }
            return null;
        }

        public static int GetTodayUserCreatedInviteCode(int creatorId)
        {
            if (creatorId > 0)
            {
                return Discuz.Data.Invitation.GetTodayUserCreatedInviteCode(creatorId);
            }
            return -1;
        }

        public static int ClearExpireInviteCode()
        {
            return Discuz.Data.Invitation.ClearExpireInviteCode();
        }
    }
}