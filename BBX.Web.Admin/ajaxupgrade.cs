using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;
using Discuz.Web.Admin.AutoUpdateManager;
using ICSharpCode.SharpZipLib.Zip;

namespace Discuz.Web.Admin
{
    public class ajaxupgrade : AdminPage
    {
        private string ver;
        private string upgradedir;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ver = Request["ver"];
            this.upgradedir = BaseConfigs.GetForumPath.ToLower() + "cache/upgrade/" + this.ver;
            try
            {
                if (Request["op"] == "downupgradefile")
                {
                    bool flag = Request["upgradetype"] == "required";
                    if (!flag)
                    {
                        this.ver = "dnt" + Utils.Version + "/" + this.ver;
                    }
                    this.SaveFile(BaseConfigs.GetDbType.ToLower(), flag, this.ver, "begin.aspx");
                    this.SaveFile(BaseConfigs.GetDbType.ToLower(), flag, this.ver, "sql.config");
                    this.SaveFile(BaseConfigs.GetDbType.ToLower(), flag, this.ver, "end.aspx");
                }
                if (Request["op"] == "downzip")
                {
                    bool flag2 = Request["upgradetype"] == "required";
                    if (!flag2)
                    {
                        this.ver = "dnt" + Utils.Version + "/" + this.ver;
                    }
                    this.SaveFile(BaseConfigs.GetDbType.ToLower(), flag2, this.ver, "upgrade.zip");
                }
                if (Request["op"] == "unzip" && File.Exists(Utils.GetMapPath(this.upgradedir + "/upgrade.zip")))
                {
                    this.UnZipFile(Utils.GetMapPath(this.upgradedir + "/upgrade.zip"), Utils.GetMapPath(this.upgradedir + "/upgrade"));
                }
                if (Request["op"] == "dispose" && Directory.Exists(Utils.GetMapPath(this.upgradedir)))
                {
                    if (!Directory.Exists(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "cache/upgradebackup/" + this.ver)))
                    {
                        Directory.CreateDirectory(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "cache/upgradebackup/" + this.ver));
                    }
                    this.DisposeFile("");
                }
                if (Request["op"] == "runsql")
                {
                    string a = Request["step"];
                    string text = (Request["optional"] != "") ? "&optional=true" : "";
                    if (a == "1")
                    {
                        if (File.Exists(Utils.GetMapPath(this.upgradedir + "/begin.aspx")))
                        {
                            base.Server.Transfer(this.upgradedir + "/begin.aspx?ver=" + this.ver + text);
                        }
                        else
                        {
                            a = "2";
                        }
                    }
                    if (a == "2")
                    {
                        if (File.Exists(Utils.GetMapPath(this.upgradedir + "/sql.config")))
                        {
                            this.RunSql();
                        }
                        a = "3";
                    }
                    if (a == "3")
                    {
                        if (File.Exists(Utils.GetMapPath(this.upgradedir + "/end.aspx")))
                        {
                            base.Server.Transfer(this.upgradedir + "/end.aspx?ver=" + this.ver + text);
                        }
                        else
                        {
                            a = "4";
                        }
                    }
                    if (a == "4")
                    {
                        if (text == "")
                        {
                            this.SaveRequiredUpgradeInfo();
                        }
                        else
                        {
                            this.SaveOptionalUpgradeInfo();
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                base.Response.Write(ex.Message);
            }
            catch (ThreadAbortException ex2)
            {
                string arg_397_0 = ex2.Message;
            }
            catch (Exception ex3)
            {
                base.Response.Write(ex3.Message);
            }
        }

        private void SaveRequiredUpgradeInfo()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
            xmlDocument.SelectSingleNode("/localupgrade/requiredupgrade").InnerText = this.ver;
            xmlDocument.Save(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
            if (Directory.Exists(Utils.GetMapPath(this.upgradedir)))
            {
                Directory.Delete(Utils.GetMapPath(this.upgradedir), true);
            }
        }

        private void SaveOptionalUpgradeInfo()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
            XmlNode xmlNode = xmlDocument.SelectSingleNode("/localupgrade/optionalupgrade/dnt" + Utils.Version);
            if (xmlNode == null)
            {
                xmlNode = xmlDocument.CreateElement("dnt" + Utils.Version);
            }
            XmlElement xmlElement = xmlDocument.CreateElement("item");
            xmlElement.InnerText = this.ver;
            xmlNode.AppendChild(xmlElement);
            if (xmlDocument.SelectSingleNode("/localupgrade/optionalupgrade") == null)
            {
                xmlDocument.SelectSingleNode("/localupgrade").AppendChild(xmlDocument.CreateElement("optionalupgrade"));
            }
            xmlDocument.SelectSingleNode("/localupgrade/optionalupgrade").AppendChild(xmlNode);
            xmlDocument.Save(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
            if (Directory.Exists(Utils.GetMapPath(this.upgradedir)))
            {
                Directory.Delete(Utils.GetMapPath(this.upgradedir), true);
            }
        }

        private void RunSql()
        {
            StringBuilder stringBuilder = new StringBuilder();
            BaseConfigInfo baseConfig = BaseConfigInfo.Current;
            string mapPath = Utils.GetMapPath(this.upgradedir + "/sql.config");
            if (!File.Exists(mapPath))
            {
                mapPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "upgrade/" + this.ver + "/sql.config");
            }
            using (StreamReader streamReader = new StreamReader(mapPath, Encoding.UTF8))
            {
                stringBuilder.Append(streamReader.ReadToEnd());
                streamReader.Close();
            }
            string[] array = stringBuilder.Replace("dnt_", baseConfig.Tableprefix).ToString().Trim().Split(new string[]
			{
				"GO\r\n",
				"go\r\n"
			}, StringSplitOptions.RemoveEmptyEntries);
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text = array2[i];
                if (!(text.Trim() == ""))
                {
                    try
                    {
                        Databases.RunSql(text);
                    }
                    catch (Exception ex)
                    {
                        base.Response.Write(ex.Message);
                    }
                }
            }
        }

        private bool UnZipFile(string zipFilePath, string unZipDir)
        {
            if (unZipDir == string.Empty)
            {
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            }
            if (!unZipDir.EndsWith("\\"))
            {
                unZipDir += "\\";
            }
            if (!Directory.Exists(unZipDir))
            {
                Directory.CreateDirectory(unZipDir);
            }
            using (ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                ZipEntry nextEntry;
                while ((nextEntry = zipInputStream.GetNextEntry()) != null)
                {
                    string text = Path.GetDirectoryName(nextEntry.Name);
                    string fileName = Path.GetFileName(nextEntry.Name);
                    if (text.Length > 0)
                    {
                        Directory.CreateDirectory(unZipDir + text);
                    }
                    if (!text.EndsWith("\\"))
                    {
                        text += "\\";
                    }
                    if (fileName != string.Empty)
                    {
                        using (FileStream fileStream = File.Create(unZipDir + nextEntry.Name))
                        {
                            byte[] array = new byte[2048];
                            while (true)
                            {
                                int num = zipInputStream.Read(array, 0, array.Length);
                                if (num <= 0)
                                {
                                    break;
                                }
                                fileStream.Write(array, 0, num);
                            }
                        }
                    }
                }
            }
            return true;
        }

        private void DisposeFile(string path)
        {
            string mapPath = Utils.GetMapPath(this.upgradedir + "/upgrade/" + path);
            string mapPath2 = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + path);
            string mapPath3 = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "cache/upgradebackup/" + Request["ver"] + "/" + path);
            DirectoryInfo directoryInfo = new DirectoryInfo(mapPath);
            if (!Directory.Exists(mapPath2))
            {
                Directory.CreateDirectory(mapPath2);
            }
            if (!Directory.Exists(mapPath3))
            {
                Directory.CreateDirectory(mapPath3);
            }
            DirectoryInfo[] directories = directoryInfo.GetDirectories();
            for (int i = 0; i < directories.Length; i++)
            {
                DirectoryInfo directoryInfo2 = directories[i];
                this.DisposeFile(path + directoryInfo2.Name + "/");
            }
            FileInfo[] files = directoryInfo.GetFiles();
            for (int j = 0; j < files.Length; j++)
            {
                FileInfo fileInfo = files[j];
                try
                {
                    if (File.Exists(mapPath2 + fileInfo.Name))
                    {
                        if (File.Exists(mapPath3 + fileInfo.Name))
                        {
                            File.Delete(mapPath3 + fileInfo.Name);
                        }
                        File.Move(mapPath2 + fileInfo.Name, mapPath3 + fileInfo.Name);
                    }
                    File.Move(mapPath + fileInfo.Name, mapPath2 + fileInfo.Name);
                }
                catch (UnauthorizedAccessException ex)
                {
                    string arg_16E_0 = ex.Message;
                    throw new UnauthorizedAccessException("对路径\"" + mapPath2 + "\"的访问被拒绝。可能无写权限。");
                }
            }
        }

        private void SaveFile(string dbtype, bool isrequired, string version, string filename)
        {
            AutoUpdate autoUpdate = new AutoUpdate();
            byte[] file = autoUpdate.GetFile(dbtype, isrequired, version, filename);
            if (file.Length == 0)
            {
                return;
            }
            if (!Directory.Exists(Utils.GetMapPath(this.upgradedir)))
            {
                Directory.CreateDirectory(Utils.GetMapPath(this.upgradedir));
            }
            FileStream fileStream = new FileStream(Utils.GetMapPath(this.upgradedir + "/" + filename), FileMode.Create);
            fileStream.Write(file, 0, file.Length);
            fileStream.Close();
        }
    }
}