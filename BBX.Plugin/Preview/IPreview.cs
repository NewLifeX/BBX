using BBX.Entity;

namespace BBX.Plugin.Preview
{
    public interface IPreview
    {
        bool UseFTP { get; set; }

        string GetPreview(string fileName, Attachment attachment);

        void OnSaved(string fileName);
    }
}