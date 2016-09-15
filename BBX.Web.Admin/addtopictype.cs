using System;
using BBX.Common;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class addtopictype : UserControlsPageBase
    {
        public bool result = true;
        public int maxId;

        protected void Page_Load(object sender, EventArgs e)
        {
            string typename = Request["typename"];
            string typeorder = Request["typeorder"];
            string typedescription = Request["typedescription"];

            //if (TopicType.Exist(typename))
            //{
            //    this.result = false;
            //    return;
            //}
            //TopicTypes.CreateTopicTypes(typename, int.Parse(typeorder), typedescription);
            //this.maxId = TopicTypes.GetMaxTopicTypesId();
            //DNTCache.Current.RemoveObject("/Forum/TopicTypes");

            var entity = TopicType.FindByName(typename);
            if (entity != null)
            {
                this.result = false;
                return;
            }

            entity = new TopicType();
            entity.Name = typename;
            entity.DisplayOrder = Int32.Parse(typeorder);
            entity.Description = typedescription;
            entity.Save();

            maxId = entity.ID;
        }
    }
}