using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Cache;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class topictypesgrid : AdminPage
    {
        protected HtmlForm Form1;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.Button SaveTopicType;
        protected BBX.Control.Button delButton;
        protected BBX.Control.TextBox typename;
        protected BBX.Control.TextBox displayorder;
        protected BBX.Control.TextBox description;
        protected BBX.Control.Button AddNewRec;
        protected Hint Hint1;
        protected BBX.Control.TextBox topictypename;
        protected BBX.Control.Button Search;
        protected BBX.Control.Button ResetSearchTable;
        protected Panel searchtable;

        private void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindData("");
            }
        }

        public void BindData(string searthKeyWord)
        {
            this.DataGrid1.AllowCustomPaging = false;
            this.DataGrid1.TableHeaderName = "主题分类";
            this.DataGrid1.BindData(TopicType.SearchWithCache(searthKeyWord));
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.DataGrid1.Sort = e.SortExpression.ToString();
        }

        private int GetDisplayOrder(string topicTypeName, DataTable topicTypes)
        {
            foreach (DataRow dataRow in topicTypes.Rows)
            {
                if (dataRow["name"].ToString().Trim() == topicTypeName.Trim())
                {
                    return int.Parse(dataRow["displayorder"].ToString());
                }
            }
            return -1;
        }

        private string GetTopicTypeString(string topicTypes, string topicName)
        {
            string[] array = topicTypes.Split('|');
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                if (text.IndexOf("," + topicName.Trim() + ",") != -1)
                {
                    return text;
                }
            }
            return "";
        }

        public void AddNewRec_Click(object sender, EventArgs e)
        {
            if (!this.CheckValue(this.typename.Text, this.displayorder.Text, this.description.Text))
            {
                return;
            }

            var entity = TopicType.FindByName(this.typename.Text);
            if (entity != null)
            {
                base.RegisterStartupScript("", "<script>alert('数据库中已存在相同的主题分类名称');window.location.href='forum_topictypesgrid.aspx';</script>");
                return;
            }

            //TopicTypes.CreateTopicTypes(this.typename.Text, int.Parse(this.displayorder.Text), this.description.Text);
            entity = new TopicType();
            entity.Name = typename.Text;
            entity.DisplayOrder = Int32.Parse(displayorder.Text);
            entity.Description = description.Text;
            entity.Save();

            AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "添加主题分类", "添加主题分类,名称为:" + this.typename.Text);
            //XCache.Remove("/Forum/TopicTypes");
            base.RegisterStartupScript("", "<script>window.location.href='forum_topictypesgrid.aspx';</script>");
        }

        public void delButton_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (Request["id"] != "")
                {
                    string @string = Request["id"];
                    this.DeleteForumTypes(@string);

                    //TopicTypes.DeleteTopicTypes(@string);
                    var entity = TopicType.FindByID(Int32.Parse(@string));
                    if (entity != null) entity.Delete();
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除主题分类", "删除主题分类,ID为:" + Request["id"].Replace("0 ", ""));
                    //XCache.Remove("/Forum/TopicTypes");
                    base.Response.Redirect("forum_topictypesgrid.aspx");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='forum_attachtypesgrid.aspx';</script>");
            }
        }

        private bool CheckValue(string typename, string displayorder, string description)
        {
            if (String.IsNullOrEmpty(typename) || typename.Length > 100)
            {
                base.RegisterStartupScript("", "<script>alert('主题分类名称不能为空');window.location.href='forum_topictypesgrid.aspx';</script>");
                return false;
            }
            if (String.IsNullOrEmpty(displayorder) || displayorder.ToInt() < 0)
            {
                base.RegisterStartupScript("", "<script>alert('显示顺序不能为空 ');window.location.href='forum_topictypesgrid.aspx';</script>");
                return false;
            }
            if (description.Length > 500)
            {
                base.RegisterStartupScript("", "<script>alert('描述不能长于500个符');window.location.href='forum_topictypesgrid.aspx';</script>");
                return false;
            }
            if (typename.IndexOf("|") > 0)
            {
                base.RegisterStartupScript("", "<script>alert('不能含有非法字符 | ');window.location.href='forum_topictypesgrid.aspx';</script>");
                return false;
            }
            return true;
        }

        public string LinkForum(string id)
        {
            return Forums.GetForumLinkOfAssociatedTopicType(id.ToInt());
        }

        private void DeleteForumTypes(string idlist)
        {
            //TopicTypes.DeleteForumTopicTypes(idlist);
            var list = TopicType.FindAllByIDs(idlist.SplitAsInt());
            if (list.Count > 0) list.Delete();
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[1].Controls[0]).Width = 150;
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[1].Controls[0]).MaxLength = 30;
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0]).Width = 30;
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0]).Width = 250;
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0]).MaxLength = 500;
            }
        }

        private void SaveTopicType_Click(object sender, EventArgs e)
        {
            int num = 0;
            bool flag = false;
            foreach (object current in this.DataGrid1.GetKeyIDArray())
            {
                string s = current.ToString();
                Int32 typeid = Int32.Parse(s);
                string controlValue = this.DataGrid1.GetControlValue(num, "name");
                string controlValue2 = this.DataGrid1.GetControlValue(num, "displayorder");
                string controlValue3 = this.DataGrid1.GetControlValue(num, "description");
                if (!this.CheckValue(controlValue, controlValue2, controlValue3) || TopicType.IsExist(controlValue, typeid))
                {
                    flag = true;
                }
                else
                {
                    var sortedList = TopicType.GetTopicTypeArray();
                    //DataTable existTopicTypeOfForum = Forums.GetExistTopicTypeOfForum();
                    DataTable topicTypes = TopicType.FindAllWithCache().ToDataTable();
                    foreach (var item in XForum.Root.AllChilds)
                    {
                        var text = item.Field.TopicTypes + "";
                        if (String.IsNullOrEmpty(text.Trim())) continue;

                        string topicTypeString = this.GetTopicTypeString(text, sortedList[typeid].ToString().Trim());
                        if (!(String.IsNullOrEmpty(topicTypeString)))
                        {
                            string value = topicTypeString.Replace("," + sortedList[typeid].ToString().Trim() + ",", "," + controlValue + ",");
                            text = text.Replace(topicTypeString + "|", "");
                            ArrayList arrayList = new ArrayList();
                            string[] array = text.Split('|');
                            for (int i = 0; i < array.Length; i++)
                            {
                                string text2 = array[i];
                                if (text2 != "")
                                {
                                    arrayList.Add(text2);
                                }
                            }
                            bool flag2 = false;
                            for (int j = 0; j < arrayList.Count; j++)
                            {
                                int displayOrder = this.GetDisplayOrder(arrayList[j].ToString().Split(',')[1], topicTypes);
                                if (displayOrder > int.Parse(controlValue2))
                                {
                                    arrayList.Insert(j, value);
                                    flag2 = true;
                                    break;
                                }
                            }
                            if (!flag2)
                            {
                                arrayList.Add(value);
                            }
                            text = "";
                            foreach (object current2 in arrayList)
                            {
                                text = text + current2.ToString() + "|";
                            }

                            //TopicTypes.UpdateForumTopicType(text, int.Parse(dataRow["fid"].ToString()));
                            //var ff = ForumField.FindByID(int.Parse(dataRow["fid"].ToString()));
                            var ff = item.Field;
                            if (ff != null)
                            {
                                ff.TopicTypes = text;
                                ff.Save();
                            }
                            XCache.Remove("/Forum/TopicTypesOption" + item.ID);
                            XCache.Remove("/Forum/TopicTypesLink" + item.ID);
                        }
                    }

                    //TopicTypes.UpdateTopicTypes(controlValue, int.Parse(controlValue2), controlValue3, typeid);
                    var entity = TopicType.FindByID(typeid);
                    if (entity != null)
                    {
                        entity.Name = controlValue;
                        entity.DisplayOrder = Int32.Parse(controlValue);
                        entity.Description = controlValue3;
                        entity.Save();
                    }
                    num++;
                }
            }

            //XCache.Remove("/Forum/TopicTypes");
            XCache.Remove(CacheKeys.FORUM_FORUM_LIST);
            if (flag)
            {
                base.RegisterStartupScript("", "<script>alert('数据库中已存在相同的主题分类名称或为空，该记录不能被更新！');window.location.href='forum_topictypesgrid.aspx';</script>");
                return;
            }
            base.RegisterStartupScript("PAGE", "window.location.href='forum_topictypesgrid.aspx';");
        }

        private void Search_Click(object sender, EventArgs e)
        {
            this.BindData(this.topictypename.Text);
            this.searchtable.Visible = false;
            this.ResetSearchTable.Visible = true;
        }

        private void ResetSearchTable_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("forum_topictypesgrid.aspx");
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AddNewRec.Click += new EventHandler(this.AddNewRec_Click);
            this.delButton.Click += new EventHandler(this.delButton_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.SaveTopicType.Click += new EventHandler(this.SaveTopicType_Click);
            this.Search.Click += new EventHandler(this.Search_Click);
            this.ResetSearchTable.Click += new EventHandler(this.ResetSearchTable_Click);
            this.DataGrid1.ColumnSpan = 5;
        }
    }
}