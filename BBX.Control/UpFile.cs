using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BBX.Control
{
    [DefaultProperty("UpFilePath"), ToolboxItem(true), ToolboxData("<{0}:UpFile runat=server></{0}:UpFile>")]
    public class UpFile : WebControl
    {
        protected System.Web.UI.WebControls.TextBox tb = new System.Web.UI.WebControls.TextBox();
        protected Label Msglabel = new Label();
        protected System.Web.UI.WebControls.Button UploadButton = new System.Web.UI.WebControls.Button();
        protected HtmlInputFile FileUpload = new HtmlInputFile();
        
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string Text { get { return tb.Text; } set { tb.Text = value; } }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string FileName { get { return FileUpload.Value; } } 

        private string m_waterMarkText;
        [Bindable(true), Category("Appearance"), DefaultValue(null)]
        public string WaterMarkText { get { return m_waterMarkText; } set { m_waterMarkText = value; } }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string UpFilePath
        {
            get
            {
                object obj = this.ViewState["RequiredFieldType"];
                if (obj != null)
                {
                    return obj.ToString();
                }
                return "";
            }
            set { ViewState["RequiredFieldType"] = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string HttpPath
        {
            get
            {
                object obj = this.ViewState["HttpPath"];
                if (obj != null)
                {
                    return obj.ToString();
                }
                return "";
            }
            set { ViewState["HttpPath"] = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("jpg,gif,")]
        public string FileType
        {
            get
            {
                object obj = this.ViewState["FileType"];
                if (obj != null)
                {
                    return obj.ToString();
                }
                return "";
            }
            set { ViewState["FileType"] = value; }
        }

        [Bindable(false), Category("Behavior"), DefaultValue("不生成缩略图"), Description("要滚动的对象。"), TypeConverter(typeof(ThumbnailImageConverter))]
        public string ThumbnailImage
        {
            get
            {
                object obj = this.ViewState["ThumbnailImage"];
                if (obj != null)
                {
                    return obj.ToString();
                }
                return "";
            }
            set { ViewState["ThumbnailImage"] = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("true")]
        public bool IsShowTextArea
        {
            get
            {
                object obj = this.ViewState["IsShowTextArea"];
                return obj == null || obj.ToString() == "True";
            }
            set
            {
                this.ViewState["IsShowTextArea"] = value;
                this.tb.Visible = value;
                this.UploadButton.Visible = value;
            }
        }

        public UpFile()
        {
            this.FileUpload.Size = 32;
            this.Controls.Add(this.FileUpload);
            this.UploadButton.Text = "上传";
            this.Controls.Add(this.UploadButton);
            this.Controls.Add(this.Msglabel);
            this.FileUpload.Attributes.Add("onfocus", "this.className='colorfocus';");
            this.FileUpload.Attributes.Add("onblur", "this.className='colorblur';");
            this.FileUpload.Attributes.Add("Class", "colorblur");
            this.tb.TextMode = TextBoxMode.MultiLine;
            this.tb.Width = 285;
            this.tb.Attributes.Add("onfocus", "this.className='colorfocus';");
            this.tb.Attributes.Add("onblur", "this.className='colorblur';");
            this.tb.Attributes.Add("rows", "2");
            this.tb.Attributes.Add("cols", "53");
            this.tb.CssClass = "colorblur";
            this.Controls.Add(this.tb);
            this.Width = 350;
            this.Height = 30;
            this.BorderStyle = BorderStyle.Dotted;
            this.BorderWidth = 0;
            this.UploadButton.Click += new EventHandler(this.UpFile_Click);
        }

        protected override void Render(HtmlTextWriter output)
        {
            this.CreateChildControls();
            if (base.HintInfo != "")
            {
                output.WriteBeginTag("span id=\"" + this.ClientID + "\"  onmouseover=\"showhintinfo(this," + base.HintLeftOffSet + "," + base.HintTopOffSet + ",'" + base.HintTitle + "','" + base.HintInfo + "','" + base.HintHeight + "','" + base.HintShowType + "');\" onmouseout=\"hidehintinfo();\">");
            }
            base.Render(output);
            if (base.HintInfo != "")
            {
                output.WriteEndTag("span");
            }
        }

        protected override void CreateChildControls()
        {
            this.UploadButton.Click += new EventHandler(this.UpFile_Click);
        }

        public string UpdateFile()
        {
            string upFilePath = this.UpFilePath;
            if (this.FileUpload.PostedFile != null)
            {
                HttpPostedFile postedFile = this.FileUpload.PostedFile;
                int contentLength = postedFile.ContentLength;
                if (contentLength == 0)
                {
                    this.Msglabel.Text = "<br /><font color=red>没有选定被上传的文件</font></b>";
                    return "";
                }
                if (this.FileType.IndexOf(Path.GetExtension(postedFile.FileName).ToLower()) < 0)
                {
                    this.Msglabel.Text = "<br /><font color=red>文件必须是以" + this.FileType.Replace("|", " , ") + "为扩展名的文件</font></b>";
                    return "";
                }
                byte[] array = new byte[contentLength];
                postedFile.InputStream.Read(array, 0, contentLength);
                DateTime now = DateTime.Now;
                string str = now.ToString("yyyy-MM-dd") + "_" + now.Hour.ToString() + "-" + now.Minute.ToString() + "-" + now.Second.ToString() + Path.GetExtension(postedFile.FileName).ToLower();
                int num = 0;
                while (File.Exists(upFilePath + str))
                {
                    num++;
                    str = Path.GetFileNameWithoutExtension(postedFile.FileName) + num.ToString() + Path.GetExtension(postedFile.FileName).ToLower();
                }
                if (!(Path.GetExtension(postedFile.FileName).ToLower() == ".jpg"))
                {
                    if (!(Path.GetExtension(postedFile.FileName).ToLower() == ".gif"))
                    {
                        goto IL_31E;
                    }
                }
                try
                {
                    FileStream fileStream = new FileStream(upFilePath + str, FileMode.Create);
                    fileStream.Write(array, 0, array.Length);
                    fileStream.Close();
                    Bitmap bitmap = new Bitmap(upFilePath + str);
                    this.Text = this.HttpPath + str;
                    if (this.ThumbnailImage == "生成缩略图")
                    {
                        int num2 = bitmap.Width / 3;
                        int num3 = bitmap.Height / 3;
                        string str2 = now.ToShortDateString() + "_" + now.Hour.ToString() + "-" + now.Minute.ToString() + "-" + now.Second.ToString() + "thum" + Path.GetExtension(postedFile.FileName).ToLower();
                        this.GetThumbnailImage(num2, num3, num2 / 2 - 60, num3 - 20, upFilePath + str, upFilePath + str2);
                    }
                    bitmap.Dispose();
                    this.Msglabel.Text = "上传成功！";
                    string result = this.Text;
                    return result;
                }
                catch (ArgumentException ex)
                {
                    this.Msglabel.Text = ex.ToString();
                    goto IL_373;
                }
            IL_31E:
                postedFile.SaveAs(upFilePath + str);
                try
                {
                    this.Text = this.HttpPath + str;
                    string result = this.Text;
                    return result;
                }
                catch (ArgumentException ex2)
                {
                    this.Msglabel.Text = ex2.ToString();
                    File.Delete(upFilePath + str);
                    string result = "";
                    return result;
                }
            }
        IL_373:
            return "";
        }

        private void UpFile_Click(object sender, EventArgs e)
        {
            this.UpdateFile();
        }

        public void GetThumbnailImage(int width, int height, int left, int right, string picpath, string picthumpath)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(picpath);
            System.Drawing.Image thumbnailImage = image.GetThumbnailImage(width, height, new System.Drawing.Image.GetThumbnailImageAbort(this.ThumbnailCallback), IntPtr.Zero);
            Bitmap bitmap = new Bitmap(thumbnailImage);
            Graphics graphics = Graphics.FromImage(bitmap);
            if (this.WaterMarkText == null || string.IsNullOrEmpty(this.WaterMarkText))
            {
                graphics.DrawString(null, new Font("Courier New", 14f), new SolidBrush(Color.White), (float)left, (float)right);
            }
            else
            {
                graphics.DrawString(this.WaterMarkText, new Font("Courier New", 14f), new SolidBrush(Color.Blue), (float)left, (float)right);
            }
            bitmap.Save(picthumpath, ImageFormat.Jpeg);
            bitmap.Dispose();
        }

        public bool ThumbnailCallback()
        {
            return true;
        }
    }
}