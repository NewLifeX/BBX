using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace BBX.Forum
{
    [XmlRoot(Namespace = "http://tempuri.org/", IsNullable = false), XmlType(Namespace = "http://tempuri.org/")]
    public class AuthHeaderCS : SoapHeader
    {
        public string Username;
        public string Password;
        public string HttpLink;
    }
}