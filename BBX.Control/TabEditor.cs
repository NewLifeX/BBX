using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace BBX.Control
{
    internal class TabEditor : WindowsFormsComponentEditor
    {
        public override bool EditComponent(ITypeDescriptorContext context, object component, IWin32Window owner)
        {
            TabControl tabControl = (TabControl)component;
            IServiceProvider site = tabControl.Site;
            IComponentChangeService componentChangeService = null;
            DesignerTransaction designerTransaction = null;
            bool flag = false;
            try
            {
                if (site != null)
                {
                    IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
                    designerTransaction = designerHost.CreateTransaction("BuildTabStrip");
                    componentChangeService = (IComponentChangeService)site.GetService(typeof(IComponentChangeService));
                    if (componentChangeService != null)
                    {
                        try
                        {
                            componentChangeService.OnComponentChanging(component, null);
                        }
                        catch (CheckoutException ex)
                        {
                            if (ex == CheckoutException.Canceled)
                            {
                                return false;
                            }
                            throw ex;
                        }
                    }
                }
                try
                {
                    TabEditorForm tabEditorForm = new TabEditorForm(tabControl);
                    if (tabEditorForm.ShowDialog(owner) == DialogResult.OK)
                    {
                        flag = true;
                    }
                }
                finally
                {
                    if (flag && componentChangeService != null)
                    {
                        componentChangeService.OnComponentChanged(tabControl, null, null, null);
                    }
                }
            }
            finally
            {
                if (designerTransaction != null)
                {
                    if (flag)
                    {
                        designerTransaction.Commit();
                    }
                    else
                    {
                        designerTransaction.Cancel();
                    }
                }
            }
            return flag;
        }
    }
}