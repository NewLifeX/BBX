using Discuz.Entity;

namespace Discuz.Plugin.PasswordMode
{
    public interface IPasswordMode
    {
        bool CheckPassword(IUser userInfo, string postpassword);

        int CreateUserInfo(IUser userInfo);

        IUser SaveUserInfo(IUser userInfo);
    }
}