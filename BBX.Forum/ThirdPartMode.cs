using Discuz.Common;
using Discuz.Entity;
using Discuz.Forum;
using XCode;

namespace Discuz.Plugin.PasswordMode
{
    public class ThirdPartMode : IPasswordMode
    {
        public bool CheckPassword(IUser userInfo, string postpassword)
        {
            if (userInfo == null)
            {
                return false;
            }
            string encryptedPassword = this.GetEncryptedPassword(userInfo, postpassword);
            return encryptedPassword == userInfo.Password;
        }

        public int CreateUserInfo(IUser userInfo)
        {
            userInfo.Salt = ForumUtils.CreateAuthStr(6, false);
            userInfo.Password = this.GetEncryptedPassword(userInfo, userInfo.Password);
            //return Discuz.Data.Users.CreateUser(userInfo);
            return (userInfo as IEntity).Save();
        }

        public IUser SaveUserInfo(IUser userInfo)
        {
            if (Utils.StrIsNullOrEmpty(userInfo.Salt))
            {
                userInfo.Salt = ForumUtils.CreateAuthStr(6, false);
            }
            userInfo.Password = this.GetEncryptedPassword(userInfo, userInfo.Password);
            //if (!Discuz.Data.Users.UpdateUser(userInfo))
            if ((userInfo as IEntity).Update() < 1)
            {
                return null;
            }
            return userInfo;
        }

        private string GetEncryptedPassword(IUser userInfo, string unEncryptPassword)
        {
            return Utils.MD5(Utils.MD5(unEncryptPassword) + userInfo.Salt);
        }
    }
}