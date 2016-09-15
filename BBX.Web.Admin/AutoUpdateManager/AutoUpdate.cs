using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace Discuz.Web.Admin.AutoUpdateManager
{
    [DesignerCategory("code"), DebuggerStepThrough, WebServiceBinding(Name = "CatchSoftInfoSoap", Namespace = "http://tempuri.org/")]
    public class AutoUpdate : SoapHttpClientProtocol
    {
        public AutoUpdate()
        {
            base.Url = "http://service.nt.discuz.net/AutoUpdate.asmx";
        }

        [SoapDocumentMethod("http://tempuri.org/GetFile", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        [return: XmlElement(DataType = "base64Binary")]
        public byte[] GetFile(string dbtype, bool isrequired, string version, string filename)
        {
            object[] array = base.Invoke("GetFile", new object[]
			{
				dbtype,
				isrequired,
				version,
				filename
			});
            return (byte[])array[0];
        }

        [SoapDocumentMethod("http://tempuri.org/GetVersionList", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public string GetVersionList()
        {
            object[] array = base.Invoke("GetVersionList", new object[0]);
            return (string)array[0];
        }
    }
}