namespace BBX.Web.Admin
{
    public struct UserGroup2
    {
        public int id;
        public string grouptitle;
        public int creditshigher;
        public int creditslower;

        public UserGroup2(int id, string grouptitle, int creditshigher, int creditslower)
        {
            this.id = id;
            this.grouptitle = grouptitle;
            this.creditshigher = creditshigher;
            this.creditslower = creditslower;
        }
    }
}