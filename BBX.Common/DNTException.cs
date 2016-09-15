using System;

namespace Discuz.Common
{
    public class DNTException : Exception
    {
        public DNTException()
        {
        }

        public DNTException(string msg)
            : base(msg)
        {
        }
    }
}