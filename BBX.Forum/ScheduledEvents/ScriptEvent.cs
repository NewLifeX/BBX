using System;
using System.Text;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum.ScheduledEvents;

namespace BBX.Forum.ScheduledEvents
{
    public class ScriptEvent : IEvent
    {
        public void Execute(object state)
        {
            var cfgs = new Object[] { GeneralConfigInfo.Current, BaseConfigInfo.Current };

            foreach (var sei in ScriptEventConfigInfo.Current.ScriptEvents)
            {
                if (sei.Enabled && sei.ShouldExecute)
                {
                    var sb = new StringBuilder(sei.Script);
                    foreach (var cfg in cfgs)
                    {
                        foreach (var pi in cfg.GetType().GetProperties())
                        {
                            sb.Replace(String.Format("{{{0}.{1}}}", cfg.GetType().Name.ToLower().Replace("info", ""), pi.Name.ToLower()), pi.GetValue(cfg, null) + "");
                        }
                    }
                    sb.Replace("{nowdate}", DateTime.Now.ToString("yyyy-MM-dd"));

                    // 执行SQL
                    //DatabaseProvider.GetInstance().RunSql(sb.ToString());
                    User.Meta.Session.Execute(sb.ToString());
                }
            }
        }
    }
}