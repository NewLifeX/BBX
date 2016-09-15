using BBX.Entity;

namespace BBX.Plugin
{
    public interface IUser2
    {
        void Create(UserInfo user);

        void Ban(int userid);

        void UnBan(int userid);

        void Delete(int userid);

        void LogIn(UserInfo user);

        void LogOut(UserInfo user);
    }
}