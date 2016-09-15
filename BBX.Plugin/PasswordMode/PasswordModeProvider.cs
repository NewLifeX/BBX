using System;
using NewLife.Model;

namespace Discuz.Plugin.PasswordMode
{
    public class PasswordModeProvider
    {
        private static IPasswordMode _passwordMode = ObjectContainer.Current.Resolve<IPasswordMode>();

        private PasswordModeProvider() { }

        public static IPasswordMode GetInstance()
        {
            return _passwordMode;
        }
    }
}