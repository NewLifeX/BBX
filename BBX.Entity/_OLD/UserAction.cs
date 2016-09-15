using System.Runtime.InteropServices;

namespace BBX.Entity
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct UserAction
    {
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct IndexShow
        {
            public static int ActionID { get { return 1; } } 

            public static string ActionName
            {
                get
                {
                    string actionName = GetActionName(IndexShow.ActionID);
                    if (!(actionName != ""))
                    {
                        return "IndexShow";
                    }
                    return actionName;
                }
            }

            public static string ActionDescription
            {
                get
                {
                    string actionDescription = GetActionDescription(IndexShow.ActionID);
                    if (!(actionDescription != ""))
                    {
                        return "浏览首页";
                    }
                    return actionDescription;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ShowForum
        {
            public static int ActionID { get { return 2; } } 

            public static string ActionName
            {
                get
                {
                    string actionName = GetActionName(ShowTopic.ActionID);
                    if (!(actionName != ""))
                    {
                        return "ShowForum";
                    }
                    return actionName;
                }
            }

            public static string ActionDescription
            {
                get
                {
                    string actionDescription = GetActionDescription(ShowTopic.ActionID);
                    if (!(actionDescription != ""))
                    {
                        return "浏览论坛板块";
                    }
                    return actionDescription;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ShowTopic
        {
            public static int ActionID { get { return 3; } } 

            public static string ActionName
            {
                get
                {
                    string actionName = GetActionName(ShowTopic.ActionID);
                    if (!(actionName != ""))
                    {
                        return "ShowTopic";
                    }
                    return actionName;
                }
            }

            public static string ActionDescription
            {
                get
                {
                    string actionDescription = GetActionDescription(ShowTopic.ActionID);
                    if (!(actionDescription != ""))
                    {
                        return "浏览帖子";
                    }
                    return actionDescription;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct Login
        {
            public static int ActionID { get { return 4; } } 

            public static string ActionName
            {
                get
                {
                    string actionName = GetActionName(Login.ActionID);
                    if (!(actionName != ""))
                    {
                        return "Login";
                    }
                    return actionName;
                }
            }

            public static string ActionDescription
            {
                get
                {
                    string actionDescription = GetActionDescription(Login.ActionID);
                    if (!(actionDescription != ""))
                    {
                        return "论坛登陆";
                    }
                    return actionDescription;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct PostTopic
        {
            public static int ActionID { get { return 5; } } 

            public static string ActionName
            {
                get
                {
                    string actionName = GetActionName(PostTopic.ActionID);
                    if (!(actionName != ""))
                    {
                        return "PostTopic";
                    }
                    return actionName;
                }
            }

            public static string ActionDescription
            {
                get
                {
                    string actionDescription = GetActionDescription(PostTopic.ActionID);
                    if (!(actionDescription != ""))
                    {
                        return "发表主题";
                    }
                    return actionDescription;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct PostReply
        {
            public static int ActionID { get { return 6; } } 

            public static string ActionName
            {
                get
                {
                    string actionName = GetActionName(PostReply.ActionID);
                    if (!(actionName != ""))
                    {
                        return "PostReply";
                    }
                    return actionName;
                }
            }

            public static string ActionDescription
            {
                get
                {
                    string actionDescription = GetActionDescription(PostReply.ActionID);
                    if (!(actionDescription != ""))
                    {
                        return "发表回复";
                    }
                    return actionDescription;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct ActivationUser
        {
            public static int ActionID { get { return 7; } } 

            public static string ActionName
            {
                get
                {
                    string actionName = GetActionName(ActivationUser.ActionID);
                    if (!(actionName != ""))
                    {
                        return "ActivationUser";
                    }
                    return actionName;
                }
            }

            public static string ActionDescription
            {
                get
                {
                    string actionDescription = GetActionDescription(ActivationUser.ActionID);
                    if (!(actionDescription != ""))
                    {
                        return "激活用户帐户";
                    }
                    return actionDescription;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct Register
        {
            public static int ActionID { get { return 8; } } 

            public static string ActionName
            {
                get
                {
                    string actionName = GetActionName(Register.ActionID);
                    if (!(actionName != ""))
                    {
                        return "Register";
                    }
                    return actionName;
                }
            }

            public static string ActionDescription
            {
                get
                {
                    string actionDescription = GetActionDescription(Register.ActionID);
                    if (!(actionDescription != ""))
                    {
                        return "注册新用户";
                    }
                    return actionDescription;
                }
            }
        }

        public static string GetActionDescriptionByID(int actionid)
        {
            if (actionid == IndexShow.ActionID)
            {
                return IndexShow.ActionDescription;
            }
            if (actionid == ShowForum.ActionID)
            {
                return ShowForum.ActionDescription;
            }
            if (actionid == ShowTopic.ActionID)
            {
                return ShowTopic.ActionDescription;
            }
            if (actionid == Login.ActionID)
            {
                return Login.ActionDescription;
            }
            if (actionid == PostTopic.ActionID)
            {
                return PostTopic.ActionDescription;
            }
            if (actionid == PostReply.ActionID)
            {
                return PostReply.ActionDescription;
            }
            if (actionid == ActivationUser.ActionID)
            {
                return ActivationUser.ActionDescription;
            }
            if (actionid == Register.ActionID)
            {
                return Register.ActionDescription;
            }
            return "";
        }

        private static string GetActionName(int actionid)
        {
            return "";
        }

        private static string GetActionDescription(int actionid)
        {
            return "";
        }
    }
}