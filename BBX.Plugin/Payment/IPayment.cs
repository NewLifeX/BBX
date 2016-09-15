namespace BBX.Plugin.Payment
{
    public interface IPayment
    {
        string PayUrl { get; }

        string CreateDigitalGoodsTradeUrl(ITrade _goods);

        string CreateNormalGoodsTradeUrl(ITrade _goods);
    }
}