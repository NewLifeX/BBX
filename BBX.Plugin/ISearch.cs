using System.Data;

namespace BBX.Plugin
{
    public interface ISearch
    {
        DataTable GetResult(int pagesize, string idstr);
    }
}