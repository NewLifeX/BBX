namespace BBX.Entity
{
    public class MyAttachmentInfo : AttachmentInfo
    {
        private string simplename = string.Empty;

        public string SimpleName { get { return simplename; } set { simplename = value; } }
    }
}