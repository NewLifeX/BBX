using System;
using System.ComponentModel.Design;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.Design;
namespace BBX.Control
{
	public class TabControlDesigner : ControlDesigner
	{
		private DesignerVerbCollection _verbs;
		public override DesignerVerbCollection Verbs
		{
			get
			{
				if (this._verbs == null)
				{
					this._verbs = new DesignerVerbCollection(new DesignerVerb[]
					{
						new DesignerVerb("创建新的属性页...", new EventHandler(this.OnBuildTabStrip))
					});
				}
				return this._verbs;
			}
		}
		protected override string GetEmptyDesignTimeHtml()
		{
			return base.CreatePlaceHolderDesignTimeHtml("右击选择创建新的属性页");
		}
		private void OnBuildTabStrip(object sender, EventArgs e)
		{
			TabEditor tabEditor = new TabEditor();
			tabEditor.EditComponent(base.Component);
		}
		public override string GetDesignTimeHtml()
		{
			string result;
			try
			{
				TabControl tabControl = (TabControl)base.Component;
				if (tabControl.Items == null || tabControl.Items.Count == 0)
				{
					result = this.GetEmptyDesignTimeHtml();
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					StringWriter stringWriter = new StringWriter(stringBuilder);
					HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
					tabControl.RenderDownLevelContent(htmlTextWriter);
					htmlTextWriter.Flush();
					stringWriter.Flush();
					result = stringBuilder.ToString();
				}
			}
			catch (Exception ex)
			{
				result = base.CreatePlaceHolderDesignTimeHtml("生成设计时代码错误:\n\n" + ex.ToString());
			}
			return result;
		}
	}
}
