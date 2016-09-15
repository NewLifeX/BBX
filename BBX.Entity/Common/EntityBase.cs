using System;
using System.Linq;
using NewLife.Reflection;
using System.Collections.Generic;
using BBX.Config;
using XCode;

namespace BBX.Entity
{
    /// <summary>实体基类</summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityBase<TEntity> : Entity<TEntity> where TEntity : EntityBase<TEntity>, new()
    {
        static EntityBase()
        {
            // 给表名加上前缀
            var prf = BaseConfigInfo.Current.Tableprefix;
            if (!prf.IsNullOrWhiteSpace())
            {
                var table = Meta.TableName;
                if (!table.StartsWith(prf, StringComparison.OrdinalIgnoreCase))
                {
                    Meta.Table.TableName = prf + table;
                    Meta.TableName = prf + table;
                }
            }
        }

        #region 实体转换
        /// <summary>转为另一个实体类对象。快速反射</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T Cast<T>() where T : class,new()
        {
            var entity = new T();
            CastTo(entity);

            return entity;
        }

        public virtual void CastTo(Object entity)
        {
            var fs = GetAllFields();
            foreach (var pi in entity.GetType().GetProperties())
            {
                var name = "";
                if (pi.CanWrite && fs.TryGetValue(pi.Name, out name))
                {
                    var v = this[name];
                    // 不要出现空字符串，否则会有很多麻烦事
                    if (v == null && pi.PropertyType == typeof(String)) v = "";
                    entity.SetValue(pi, v);
                }
            }
        }

        protected virtual Dictionary<String, String> GetAllFields()
        {
            //return Meta.AllFields.ToDictionary(e => e.Name, e => e.Name, StringComparer.OrdinalIgnoreCase);
            var dic = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in Meta.AllFields)
            {
                dic.Add(item.Name, item.Name);
                // 字段名也加上去，方便新旧实体对象兼容
                if (item.Name != item.ColumnName) dic.Add(item.ColumnName, item.Name);
            }
            return dic;
        }
        #endregion
    }

    public static class MyEntityHelper
    {
        /// <summary>清理所有字符串字段数据的前后空格</summary>
        public static void TrimField(this IEntity entity)
        {
            var eop = EntityFactory.CreateOperate(entity.GetType());
            foreach (var item in eop.Fields)
            {
                if (item.Type != typeof(String)) continue;
                if (item.Length <= 0 || item.Length > 300) continue;

                // 避免Null和Empty被判为不相等
                var v1 = entity[item.Name] + "";
                var v2 = v1.Trim();
                if (v1 != v2) entity.SetItem(item.Name, v2);
            }
        }
    }
}