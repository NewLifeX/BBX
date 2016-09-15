using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BBX.Control
{
    internal class TabEditorForm : Form
    {
        private TreeView treeView1;
        private PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button cancel;
        private IContainer components;
        private System.Windows.Forms.Button button3;
        private TabControl _tabStrip;
        private System.Windows.Forms.Button delnode;
        private ToolTip toolTip1;
        public TabPageCollection Tabs;

        private void LoadNodes(TabPage oItem, TreeNode oTreeNode)
        {
            oTreeNode.Tag = oItem;
            foreach (TabPage tabPage in oItem.Controls)
            {
                TreeNode treeNode = new TreeNode(tabPage.Caption);
                this.LoadNodes(tabPage, treeNode);
                oTreeNode.Nodes.Add(treeNode);
            }
        }

        public TabEditorForm(TabControl oTabStrip)
        {
            this.InitializeComponent();
            this._tabStrip = oTabStrip;
            this.Tabs = oTabStrip.Items;
            foreach (TabPage tabPage in this.Tabs)
            {
                TreeNode treeNode = new TreeNode(tabPage.Caption);
                this.LoadNodes(tabPage, treeNode);
                this.treeView1.Nodes.Add(treeNode);
            }
            this.treeView1.HideSelection = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.treeView1 = new TreeView();
            this.propertyGrid1 = new PropertyGrid();
            this.save = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.delnode = new System.Windows.Forms.Button();
            this.toolTip1 = new ToolTip(this.components);
            base.SuspendLayout();
            this.treeView1.Location = new Point(12, 44);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new Size(307, 310);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new TreeViewEventHandler(this.treeView1_AfterSelect);
            this.propertyGrid1.LineColor = SystemColors.ScrollBar;
            this.propertyGrid1.Location = new Point(336, 1);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new Size(326, 353);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.PropertyValueChanged += new PropertyValueChangedEventHandler(this.propertyGrid1_ValueChanged);
            this.save.Location = new Point(432, 371);
            this.save.Name = "save";
            this.save.Size = new Size(106, 25);
            this.save.TabIndex = 2;
            this.save.Text = " 保 存 ";
            this.save.Click += new EventHandler(this.save_Click);
            this.cancel.Location = new Point(557, 371);
            this.cancel.Name = "cancel";
            this.cancel.Size = new Size(105, 25);
            this.cancel.TabIndex = 3;
            this.cancel.Text = " 取 消 ";
            this.cancel.Click += new EventHandler(this.cancel_Click);
            this.button3.Location = new Point(12, 1);
            this.button3.Name = "button3";
            this.button3.Size = new Size(101, 34);
            this.button3.TabIndex = 4;
            this.button3.Text = "添加属性页";
            this.toolTip1.SetToolTip(this.button3, "添加属性页");
            this.button3.Click += new EventHandler(this.button3_Click);
            this.delnode.Location = new Point(130, 1);
            this.delnode.Name = "delnode";
            this.delnode.Size = new Size(87, 34);
            this.delnode.TabIndex = 6;
            this.delnode.Text = "删除属性页";
            this.toolTip1.SetToolTip(this.delnode, "删除属性页");
            this.delnode.Click += new EventHandler(this.delnode_Click);
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(683, 398);
            base.Controls.Add(this.delnode);
            base.Controls.Add(this.button3);
            base.Controls.Add(this.cancel);
            base.Controls.Add(this.save);
            base.Controls.Add(this.propertyGrid1);
            base.Controls.Add(this.treeView1);
            //base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.Name = "TabEditorForm";
            this.Text = "BBX TabPage Designer";
            base.ResumeLayout(false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TabPage tabPage = new TabPage();
            tabPage.Caption = "新属性页";
            this.Tabs.Add(tabPage);
            TreeNode treeNode = new TreeNode("新属性页");
            treeNode.Tag = tabPage;
            this.treeView1.Nodes.Add(treeNode);
            this.treeView1.SelectedNode = this.treeView1.Nodes[this.treeView1.Nodes.Count - 1];
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.propertyGrid1.SelectedObject = e.Node.Tag;
        }

        private void delnode_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode != null)
            {
                TabPage pTab = (TabPage)this.treeView1.SelectedNode.Tag;
                this._tabStrip.Items.Remove(pTab);
                this.treeView1.SelectedNode.Remove();
            }
        }

        private void propertyGrid1_ValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == "Caption")
            {
                this.treeView1.SelectedNode.Text = (string)e.ChangedItem.Value;
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }
    }
}