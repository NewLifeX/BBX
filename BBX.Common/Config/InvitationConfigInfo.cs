using System;
using System.ComponentModel;
using NewLife.Xml;

namespace BBX.Config
{
    [XmlConfigFile("config/invitation.config", 15000)]
    /// <summary>邀请设置</summary>
    [Description("邀请设置")]
    [Serializable]
    public class InvitationConfigInfo : XmlConfig2<InvitationConfigInfo>
    {
        private int m_invitecodeexpiretime;
        public int InviteCodeExpireTime { get { return m_invitecodeexpiretime; } set { m_invitecodeexpiretime = value; } }

        private int m_invitecodemaxcount;
        public int InviteCodeMaxCount { get { return m_invitecodemaxcount; } set { m_invitecodemaxcount = value; } }

        private int m_invitecodeaddcreditsline;
        public int InviteCodePayCount { get { return m_invitecodeaddcreditsline; } set { m_invitecodeaddcreditsline = value; } }

        private string m_invitecodeprice = "0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00";
        public string InviteCodePrice { get { return m_invitecodeprice; } set { m_invitecodeprice = value; } }

        private string m_invitationloginuserdescription = "";
        public string InvitationLoginUserDescription { get { return m_invitationloginuserdescription; } set { m_invitationloginuserdescription = value; } }

        private string m_invitationvisitordescription = "";
        public string InvitationVisitorDescription { get { return m_invitationvisitordescription; } set { m_invitationvisitordescription = value; } }

        private string m_invitationemailmodel = "";
        public string InvitationEmailTemplate { get { return m_invitationemailmodel; } set { m_invitationemailmodel = value; } }

        private int m_invitecodeusermaxbuy = 25;
        public int InviteCodeMaxCountToBuy { get { return m_invitecodeusermaxbuy; } set { m_invitecodeusermaxbuy = value; } }

        private int m_invitecodeusercreateperday = 5;
        public int InviteCodeUserCreatePerDay { get { return m_invitecodeusercreateperday; } set { m_invitecodeusercreateperday = value; } }
    }
}