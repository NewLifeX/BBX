using System;
using System.Text;
using System.Web;
using NewLife.Web;
using XCode.Membership;

namespace BBX.Entity
{
    class MyManageProvider : ManageProvider, IErrorInfoProvider
    {
        /// <summary>当前用户</summary>
        public override IManageUser Current
        {
            get
            {
                if (HttpContext.Current == null) return null;

                var Session = HttpContext.Current.Session;
                if (Session == null) return null;

                var online = Online.Current;
                return online == null ? null : online.User;
            }
            set
            {
                if (value == null) Online.Current = null;
            }
        }

        public override IManageUser FindByName(string account)
        {
            return BBX.Entity.User.FindByName(account);
        }

        public override IManageUser FindByID(object userid)
        {
            return BBX.Entity.User.FindByID((Int32)userid);
        }

        //public object GetService(Type serviceType)
        //{
        //    throw new NotImplementedException();
        //}

        //public IManageUser Login(string account, string password)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Logout(IManageUser user)
        //{
        //    throw new NotImplementedException();
        //}

        public override Type UserType { get { return typeof(User); } }

        #region IErrorInfoProvider 成员
        void IErrorInfoProvider.AddInfo(Exception ex, StringBuilder builder)
        {
            var user = Current;
            if (user != null)
            {
                if (user["GroupTitle"] != null)
                    builder.AppendFormat("登录：{0}({1})\r\n", user.Name, user["GroupTitle"]);
                else
                    builder.AppendFormat("登录：{0}\r\n", user.Name);
            }
        }
        #endregion

        #region IManageProvider 成员
        public override IManageUser Login(string name, string password, bool rememberme)
        {
            throw new NotImplementedException();
        }

        public override IManageUser Register(string name, string password, string rolename, bool enable)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}