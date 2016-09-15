using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Discuz.Common
{
    /// <summary>FTP²Ù×÷·â×°</summary>
    public class FTP
    {
        public string server;
        public string user;
        public string pass;
        public int port;
        public int timeout;
        public string errormessage;
        private string messages;
        private Socket main_sock;
        private Socket listening_sock;
        private Socket data_sock;
        private FileStream file;
        private int response;
        private string bucket;
        private int byteLength = 512;

        public bool IsConnected { get { return main_sock != null && this.main_sock.Connected; } }

        public bool MessagesAvailable { get { return messages.Length > 0; } }

        public string Messages
        {
            get
            {
                string result = this.messages;
                this.messages = "";
                return result;
            }
        }

        private string responseStr;
        public string ResponseString { get { return responseStr; } }

        private long bytes_total;
        public long BytesTotal { get { return bytes_total; } }

        private long file_size;
        public long FileSize { get { return file_size; } }

        private bool passive_mode;
        public bool PassiveMode { get { return passive_mode; } set { passive_mode = value; } }

        public FTP()
        {
            this.server = null;
            this.user = null;
            this.pass = null;
            this.port = 21;
            this.passive_mode = true;
            this.main_sock = null;
            this.listening_sock = null;
            this.data_sock = null;
            this.file = null;
            this.bucket = "";
            this.bytes_total = 0L;
            this.timeout = 10000;
            this.messages = "";
            this.errormessage = "";
        }

        public FTP(string server, string user, string pass)
        {
            this.server = server;
            this.user = user;
            this.pass = pass;
            this.port = 21;
            this.passive_mode = true;
            this.main_sock = null;
            this.listening_sock = null;
            this.data_sock = null;
            this.file = null;
            this.bucket = "";
            this.bytes_total = 0L;
            this.timeout = 10000;
            this.messages = "";
            this.errormessage = "";
        }

        public FTP(string server, int port, string user, string pass)
        {
            this.server = server;
            this.user = user;
            this.pass = pass;
            this.port = port;
            this.passive_mode = true;
            this.main_sock = null;
            this.listening_sock = null;
            this.data_sock = null;
            this.file = null;
            this.bucket = "";
            this.bytes_total = 0L;
            this.timeout = 10000;
            this.messages = "";
            this.errormessage = "";
        }

        public FTP(string server, int port, string user, string pass, int mode)
        {
            this.server = server;
            this.user = user;
            this.pass = pass;
            this.port = port;
            this.passive_mode = (mode <= 1);
            this.main_sock = null;
            this.listening_sock = null;
            this.data_sock = null;
            this.file = null;
            this.bucket = "";
            this.bytes_total = 0L;
            this.timeout = 10000;
            this.messages = "";
            this.errormessage = "";
        }

        public FTP(string server, int port, string user, string pass, int mode, int timeout_sec)
        {
            this.server = server;
            this.user = user;
            this.pass = pass;
            this.port = port;
            this.passive_mode = (mode <= 1);
            this.main_sock = null;
            this.listening_sock = null;
            this.data_sock = null;
            this.file = null;
            this.bucket = "";
            this.bytes_total = 0L;
            this.timeout = ((timeout_sec <= 0) ? 2147483647 : (timeout_sec * 1000));
            this.messages = "";
            this.errormessage = "";
        }

        private void Fail()
        {
            this.Disconnect();
            this.errormessage += this.responseStr;
        }

        private void SetBinaryMode(bool mode)
        {
            if (mode)
            {
                this.SendCommand("TYPE I");
            }
            else
            {
                this.SendCommand("TYPE A");
            }
            this.ReadResponse();
            if (this.response != 200)
            {
                this.Fail();
            }
        }

        private void SendCommand(string command)
        {
            byte[] bytes = Encoding.ASCII.GetBytes((command + "\r\n").ToCharArray());
            if (command.Length > 3 && command.Substring(0, 4) == "PASS")
            {
                this.messages = "\rPASS xxx";
            }
            else
            {
                this.messages = "\r" + command;
            }
            try
            {
                this.main_sock.Send(bytes, bytes.Length, SocketFlags.None);
            }
            catch (Exception ex)
            {
                try
                {
                    this.Disconnect();
                    this.errormessage += ex.Message;
                }
                catch
                {
                    this.main_sock.Close();
                    this.file.Close();
                    this.main_sock = null;
                    this.file = null;
                }
            }
        }

        private void FillBucket()
        {
            byte[] array = new byte[this.byteLength];
            int num = 0;
            while (this.main_sock != null)
            {
                if (this.main_sock.Available >= 1)
                {
                    break;
                }
                Thread.Sleep(50);
                num += 50;
                if (num > this.timeout)
                {
                    this.Disconnect();
                    this.errormessage += "Timed out waiting on server to respond.";
                    return;
                }
            }
            while (this.main_sock != null && this.main_sock.Available > 0)
            {
                long num2 = (long)this.main_sock.Receive(array, this.byteLength, SocketFlags.None);
                this.bucket += Encoding.ASCII.GetString(array, 0, (int)num2);
                Thread.Sleep(50);
            }
        }

        private string GetLineFromBucket()
        {
            int i;
            if ((i = this.bucket.IndexOf('\n')) < 0)
            {
                while (i < 0)
                {
                    this.FillBucket();
                    i = this.bucket.IndexOf('\n');
                }
            }
            string result = this.bucket.Substring(0, i);
            this.bucket = this.bucket.Substring(i + 1);
            return result;
        }

        private void ReadResponse()
        {
            this.messages = "";
            string lineFromBucket;
            while (true)
            {
                lineFromBucket = this.GetLineFromBucket();
                if (Regex.Match(lineFromBucket, "^[0-9]+ ").Success)
                {
                    break;
                }
                this.messages = this.messages + Regex.Replace(lineFromBucket, "^[0-9]+-", "") + "\n";
            }
            this.responseStr = lineFromBucket;
            this.response = int.Parse(lineFromBucket.Substring(0, 3));
        }

        private void OpenDataSocket()
        {
            if (this.passive_mode)
            {
                this.Connect();
                this.SendCommand("PASV");
                this.ReadResponse();
                if (this.response != 227) this.Fail();

                string[] array;
                try
                {
                    int num = this.responseStr.IndexOf('(') + 1;
                    int length = this.responseStr.IndexOf(')') - num;
                    array = this.responseStr.Substring(num, length).Split(',');
                }
                catch (Exception)
                {
                    this.Disconnect();
                    this.errormessage = this.errormessage + "Malformed PASV response: " + this.responseStr;
                    return;
                }
                if (array.Length >= 6)
                {
                    string host = string.Format("{0}.{1}.{2}.{3}", array[0], array[1], array[2], array[3]);
                    int num2 = (int.Parse(array[4]) << 8) + int.Parse(array[5]);
                    try
                    {
                        this.CloseDataSocket();
                        this.data_sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        this.data_sock.Connect(host, num2);
                        return;
                    }
                    catch (Exception ex)
                    {
                        this.errormessage = this.errormessage + "Failed to connect for data transfer: " + ex.Message;
                        return;
                    }
                }
                else
                {
                    this.Disconnect();
                    this.errormessage = this.errormessage + "Malformed PASV response: " + this.responseStr;
                    return;
                }
            }

            this.Connect();
            try
            {
                this.CloseDataSocket();
                this.listening_sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                string text = this.main_sock.LocalEndPoint.ToString();
                int num3 = text.IndexOf(':');
                if (num3 < 0)
                {
                    this.errormessage = this.errormessage + "Failed to parse the local address: " + text;
                    return;
                }
                string text2 = text.Substring(0, num3);
                IPEndPoint localEP = new IPEndPoint(IPAddress.Parse(text2), 0);
                this.listening_sock.Bind(localEP);
                text = this.listening_sock.LocalEndPoint.ToString();
                num3 = text.IndexOf(':');
                if (num3 < 0)
                {
                    this.errormessage = this.errormessage + "Failed to parse the local address: " + text;
                }
                int num4 = int.Parse(text.Substring(num3 + 1));
                this.listening_sock.Listen(1);
                string command = string.Format("PORT {0},{1},{2}", text2.Replace('.', ','), num4 / 256, num4 % 256);
                this.SendCommand(command);
                this.ReadResponse();
                if (this.response != 200) this.Fail();
            }
            catch (Exception ex2)
            {
                this.errormessage = this.errormessage + "Failed to connect for data transfer: " + ex2.Message;
                return;
            }
        }

        private void ConnectDataSocket()
        {
            if (this.data_sock != null) return;

            try
            {
                this.data_sock = this.listening_sock.Accept();
                this.listening_sock.Close();
                this.listening_sock = null;
                if (this.data_sock == null) throw new Exception("Winsock error: " + Convert.ToString(Marshal.GetLastWin32Error()));
            }
            catch (Exception ex)
            {
                this.errormessage = this.errormessage + "Failed to connect for data transfer: " + ex.Message;
            }
        }

        private void CloseDataSocket()
        {
            if (this.data_sock != null)
            {
                if (this.data_sock.Connected) this.data_sock.Close();

                this.data_sock = null;
            }
        }

        public void Disconnect()
        {
            this.CloseDataSocket();
            if (this.main_sock != null)
            {
                if (this.main_sock.Connected)
                {
                    this.SendCommand("QUIT");
                    this.main_sock.Close();
                }
                this.main_sock = null;
            }
            if (this.file != null) this.file.Close();

            this.file = null;
        }

        public void Connect(string server, int port, string user, string pass)
        {
            this.server = server;
            this.user = user;
            this.pass = pass;
            this.port = port;
            this.Connect();
        }

        public void Connect(string server, string user, string pass)
        {
            this.server = server;
            this.user = user;
            this.pass = pass;
            this.Connect();
        }

        public bool Connect()
        {
            if (this.server == null) this.errormessage += "No server has been set.\r\n";

            if (this.user == null) this.errormessage += "No server has been set.\r\n";

            if (this.main_sock != null && this.main_sock.Connected) return true;

            try
            {
                this.main_sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.main_sock.Connect(this.server, this.port);
            }
            catch (Exception ex)
            {
                this.errormessage += ex.Message;
                return false;
            }
            this.ReadResponse();
            if (this.response != 220) this.Fail();

            this.SendCommand("USER " + this.user);
            this.ReadResponse();
            int num = this.response;
            if (num != 230 && num == 331)
            {
                if (this.pass == null)
                {
                    this.Disconnect();
                    this.errormessage += "No password has been set.";
                    return false;
                }
                this.SendCommand("PASS " + this.pass);
                this.ReadResponse();
                if (this.response != 230)
                {
                    this.Fail();
                    return false;
                }
            }
            return true;
        }

        public ArrayList List()
        {
            byte[] array = new byte[this.byteLength];
            string text = "";
            int num = 0;
            var arrayList = new ArrayList();
            this.Connect();
            this.OpenDataSocket();
            this.SendCommand("LIST");
            this.ReadResponse();
            int num2 = this.response;
            if (num2 != 125 && num2 != 150)
            {
                this.CloseDataSocket();
                throw new Exception(this.responseStr);
            }
            this.ConnectDataSocket();
            while (true)
            {
                if (this.data_sock.Available < 1)
                {
                    Thread.Sleep(50);
                    num += 50;
                    if (num <= this.timeout / 10) continue;
                }

                //IL_C3:
                while (this.data_sock.Available > 0)
                {
                    long num3 = (long)this.data_sock.Receive(array, array.Length, SocketFlags.None);
                    text += Encoding.ASCII.GetString(array, 0, (int)num3);
                    Thread.Sleep(50);
                }
                this.CloseDataSocket();
                this.ReadResponse();
                if (this.response != 226) throw new Exception(this.responseStr);

                string[] array2 = text.Split('\n');
                for (int i = 0; i < array2.Length; i++)
                {
                    string text2 = array2[i];
                    if (text2.Length > 0 && !Regex.Match(text2, "^total").Success)
                    {
                        arrayList.Add(text2.Substring(0, text2.Length - 1));
                    }
                }
                return arrayList;
            }

            //goto IL_C3;
        }

        public ArrayList ListFiles()
        {
            ArrayList arrayList = new ArrayList();
            foreach (string text in this.List())
            {
                if (text.Length > 0 && text[0] != 'd' && text.ToUpper().IndexOf("<DIR>") < 0)
                {
                    arrayList.Add(text);
                }
            }
            return arrayList;
        }

        public ArrayList ListDirectories()
        {
            ArrayList arrayList = new ArrayList();
            foreach (string text in this.List())
            {
                if (text.Length > 0 && (text[0] == 'd' || text.ToUpper().IndexOf("<DIR>") >= 0))
                {
                    arrayList.Add(text);
                }
            }
            return arrayList;
        }

        public string GetFileDateRaw(string fileName)
        {
            this.Connect();
            this.SendCommand("MDTM " + fileName);
            this.ReadResponse();
            if (this.response != 213)
            {
                this.errormessage += this.responseStr;
                return "";
            }
            return this.responseStr.Substring(4);
        }

        public DateTime GetFileDate(string fileName)
        {
            return this.ConvertFTPDateToDateTime(this.GetFileDateRaw(fileName));
        }

        private DateTime ConvertFTPDateToDateTime(string input)
        {
            if (input.Length < 14)
            {
                throw new ArgumentException("Input Value for ConvertFTPDateToDateTime method was too short.");
            }
            int year = (int)Convert.ToInt16(input.Substring(0, 4));
            int month = (int)Convert.ToInt16(input.Substring(4, 2));
            int day = (int)Convert.ToInt16(input.Substring(6, 2));
            int hour = (int)Convert.ToInt16(input.Substring(8, 2));
            int minute = (int)Convert.ToInt16(input.Substring(10, 2));
            int second = (int)Convert.ToInt16(input.Substring(12, 2));
            return new DateTime(year, month, day, hour, minute, second);
        }

        public string GetWorkingDirectory()
        {
            this.Connect();
            this.SendCommand("PWD");
            this.ReadResponse();
            if (this.response != 257)
            {
                this.errormessage += this.responseStr;
            }
            string text;
            try
            {
                text = this.responseStr.Substring(this.responseStr.IndexOf("\"", 0) + 1);
                text = text.Substring(0, text.LastIndexOf("\""));
                text = text.Replace("\"\"", "\"");
            }
            catch (Exception ex)
            {
                this.errormessage += ex.Message;
                return null;
            }
            return text;
        }

        public bool ChangeDir(string path)
        {
            this.Connect();
            this.SendCommand("CWD " + path);
            this.ReadResponse();
            if (this.response != 250)
            {
                this.errormessage += this.responseStr;
                return false;
            }
            return true;
        }

        public void MakeDir(string dir)
        {
            this.Connect();
            this.SendCommand("MKD " + dir);
            this.ReadResponse();
            int num = this.response;
            if (num != 250 && num != 257)
            {
                this.errormessage += this.responseStr;
            }
        }

        public void RemoveDir(string dir)
        {
            this.Connect();
            this.SendCommand("RMD " + dir);
            this.ReadResponse();
            if (this.response != 250)
            {
                this.errormessage += this.responseStr;
            }
        }

        public void RemoveFile(string filename)
        {
            this.Connect();
            this.SendCommand("DELE " + filename);
            this.ReadResponse();
            if (this.response != 250)
            {
                this.errormessage += this.responseStr;
            }
        }

        public void RenameFile(string oldfilename, string newfilename)
        {
            this.Connect();
            this.SendCommand("RNFR " + oldfilename);
            this.ReadResponse();
            if (this.response != 350)
            {
                this.errormessage += this.responseStr;
                return;
            }
            this.SendCommand("RNTO " + newfilename);
            this.ReadResponse();
            if (this.response != 250)
            {
                this.errormessage += this.responseStr;
            }
        }

        public long GetFileSize(string filename)
        {
            this.Connect();
            this.SendCommand("SIZE " + filename);
            this.ReadResponse();
            if (this.response != 213)
            {
                this.errormessage += this.responseStr;
            }
            return long.Parse(this.responseStr.Substring(4));
        }

        public bool OpenUpload(string filename)
        {
            return this.OpenUpload(filename, filename, false);
        }

        public bool OpenUpload(string filename, string remotefilename)
        {
            return this.OpenUpload(filename, remotefilename, false);
        }

        public bool OpenUpload(string filename, bool resume)
        {
            return this.OpenUpload(filename, filename, resume);
        }

        public bool OpenUpload(string filename, string remote_filename, bool resume)
        {
            this.Connect();
            this.SetBinaryMode(true);
            this.OpenDataSocket();
            this.bytes_total = 0L;
            try
            {
                this.file = new FileStream(filename, FileMode.Open);
            }
            catch (Exception ex)
            {
                this.file = null;
                this.errormessage += ex.Message;
                bool result = false;
                return result;
            }
            this.file_size = this.file.Length;
            if (resume)
            {
                long fileSize = this.GetFileSize(remote_filename);
                this.SendCommand("REST " + fileSize);
                this.ReadResponse();
                if (this.response == 350)
                {
                    this.file.Seek(fileSize, SeekOrigin.Begin);
                }
            }
            this.SendCommand("STOR " + remote_filename);
            this.ReadResponse();
            int num = this.response;
            if (num != 125 && num != 150)
            {
                this.file.Close();
                this.file = null;
                this.errormessage += this.responseStr;
                return false;
            }
            this.ConnectDataSocket();
            return true;
        }

        public void OpenDownload(string filename)
        {
            this.OpenDownload(filename, filename, false);
        }

        public void OpenDownload(string filename, bool resume)
        {
            this.OpenDownload(filename, filename, resume);
        }

        public void OpenDownload(string remote_filename, string localfilename)
        {
            this.OpenDownload(remote_filename, localfilename, false);
        }

        public void OpenDownload(string remote_filename, string local_filename, bool resume)
        {
            this.Connect();
            this.SetBinaryMode(true);
            this.bytes_total = 0L;
            try
            {
                this.file_size = this.GetFileSize(remote_filename);
            }
            catch
            {
                this.file_size = 0L;
            }
            if (resume && File.Exists(local_filename))
            {
                try
                {
                    this.file = new FileStream(local_filename, FileMode.Open);
                }
                catch (Exception ex)
                {
                    this.file = null;
                    throw new Exception(ex.Message);
                }
                this.SendCommand("REST " + this.file.Length);
                this.ReadResponse();
                if (this.response != 350)
                {
                    throw new Exception(this.responseStr);
                }
                this.file.Seek(this.file.Length, SeekOrigin.Begin);
                this.bytes_total = this.file.Length;
            }
            else
            {
                try
                {
                    this.file = new FileStream(local_filename, FileMode.Create);
                }
                catch (Exception ex2)
                {
                    this.file = null;
                    throw new Exception(ex2.Message);
                }
            }
            this.OpenDataSocket();
            this.SendCommand("RETR " + remote_filename);
            this.ReadResponse();
            int num = this.response;
            if (num != 125 && num != 150)
            {
                this.file.Close();
                this.file = null;
                this.errormessage += this.responseStr;
                return;
            }
            this.ConnectDataSocket();
        }

        public long DoUpload()
        {
            byte[] array = new byte[this.byteLength];
            long num;
            try
            {
                num = (long)this.file.Read(array, 0, array.Length);
                this.bytes_total += num;
                this.data_sock.Send(array, (int)num, SocketFlags.None);
                if (num <= 0L)
                {
                    this.file.Close();
                    this.file = null;
                    this.CloseDataSocket();
                    this.ReadResponse();
                    int num2 = this.response;
                    if (num2 != 226 && num2 != 250)
                    {
                        this.errormessage += this.responseStr;
                        long result = -1L;
                        return result;
                    }
                    this.SetBinaryMode(false);
                }
            }
            catch (Exception ex)
            {
                this.file.Close();
                this.file = null;
                this.CloseDataSocket();
                this.ReadResponse();
                this.SetBinaryMode(false);
                this.errormessage += ex.Message;
                long result = -1L;
                return result;
            }
            return num;
        }

        public long DoDownload()
        {
            byte[] array = new byte[this.byteLength];
            long num;
            try
            {
                num = (long)this.data_sock.Receive(array, array.Length, SocketFlags.None);
                if (num <= 0L)
                {
                    this.CloseDataSocket();
                    this.file.Close();
                    this.file = null;
                    this.ReadResponse();
                    int num2 = this.response;
                    long result;
                    if (num2 != 226 && num2 != 250)
                    {
                        this.errormessage += this.responseStr;
                        result = -1L;
                        return result;
                    }
                    this.SetBinaryMode(false);
                    result = num;
                    return result;
                }
                else
                {
                    this.file.Write(array, 0, (int)num);
                    this.bytes_total += num;
                }
            }
            catch (Exception ex)
            {
                this.CloseDataSocket();
                this.file.Close();
                this.file = null;
                this.ReadResponse();
                this.SetBinaryMode(false);
                this.errormessage += ex.Message;
                long result = -1L;
                return result;
            }
            return num;
        }
    }
}