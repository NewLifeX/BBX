namespace Discuz.Entity
{
    public enum TradeStatusEnum
    {
        UnStart,
        WAIT_BUYER_PAY,
        WAIT_SELLER_CONFIRM_TRADE,
        WAIT_SYS_CONFIRM_PAY,
        WAIT_SELLER_SEND_GOODS,
        WAIT_BUYER_CONFIRM_GOODS,
        WAIT_SYS_PAY_SELLER,
        TRADE_FINISHED,
        TRADE_CLOSED,
        WAIT_SELLER_AGREE = 10,
        SELLER_REFUSE_BUYER,
        WAIT_BUYER_RETURN_GOODS,
        WAIT_SELLER_CONFIRM_GOODS,
        WAIT_ALIPAY_REFUND,
        ALIPAY_CHECK,
        OVERED_REFUND,
        REFUND_SUCCESS,
        REFUND_CLOSED
    }
}