namespace BBX.Control
{
    public interface IWebControl
    {
        string HintTitle { get; set; }

        string HintInfo { get; set; }

        int HintLeftOffSet { get; set; }

        int HintTopOffSet { get; set; }

        int HintHeight { get; set; }
    }
}