namespace WeChat.Model
{
    /// <summary>
    /// 统一下单返回实体
    /// </summary>
    public class unifiedOrderResponse:Basic
    {

        /// <summary>
        /// 设备号 自定义参数，可以为请求支付的终端设备号等
        /// </summary>
        public string device_info { get; set; }


        /// <summary>
        /// 预支付交易会话标识 微信生成的预支付回话标识，用于后续接口调用中使用，该值有效期为2小时
        /// </summary>
        public string prepay_id { get; set; }

        /// <summary>
        /// 交易类型 交易类型，取值为：JSAPI，NATIVE，APP等，说明详见参数规定
        /// https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=4_2
        /// </summary>
        public string trade_type { get; set; }

        /// <summary>
        /// 二维码链接 trade_type为NATIVE时有返回，用于生成二维码，展示给用户进行扫码支付
        /// </summary>
        public string code_url { get; set; }
    }


    /// <summary>
    /// 订单查询返回实体
    /// </summary>
    public class orderQueryResponse : Basic
    {
        /// <summary>
        /// 设备号 微信支付分配的终端设备号
        /// </summary>
        public string device_info { get; set; }

        /// <summary>
        /// 用户标识 用户在商户appid下的唯一标识
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 是否关注公众账号 用户是否关注公众账号，Y-关注，N-未关注，仅在公众账号类型支付有效
        /// </summary>
        public string is_subscribe { get; set; }

        /// <summary>
        /// 交易类型 调用接口提交的交易类型
        /// </summary>
        public string trade_type { get; set; }

        /// <summary>
        /// 交易状态
        /// SUCCESS—支付成功
        /// REFUND—转入退款
        /// NOTPAY—未支付
        /// CLOSED—已关闭
        /// REVOKED—已撤销（刷卡支付）
        /// USERPAYING--用户支付中
        /// PAYERROR--支付失败(其他原因，如银行返回失败)
        /// </summary>
        public string trade_state { get; set; }


        /// <summary>
        /// 付款银行 银行类型，采用字符串类型的银行标识
        /// </summary>
        public string bank_type { get; set; }

        /// <summary>
        /// 总金额 订单总金额，单位为分
        /// </summary>
        public string total_fee { get; set; }


        /// <summary>
        /// 货币种类 货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
        /// </summary>
        public string fee_type { get; set; }

        /// <summary>
        /// 现金支付金额 现金支付金额订单现金支付金额，详见支付金额
        /// </summary>
        public string cash_fee { get; set; }

        /// <summary>
        /// 现金支付货币类型 货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
        /// </summary>
        public string cash_fee_type { get; set; }

        /// <summary>
        /// 代金券或立减优惠金额 代金券或立减优惠”金额<=订单总金额，订单总金额-“代金券或立减优惠”金额=现金支付金额，详见支付金额
        /// </summary>
        public string coupon_fee { get; set; }

        /// <summary>
        /// 代金券或立减优惠使用数量 代金券或立减优惠使用数量
        /// </summary>
        public string coupon_count { get; set; }

        /// <summary>
        /// 微信支付订单号 微信支付订单号
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 商户订单号 商户系统的订单号，与请求一致。
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 附加数据 附加数据，原样返回
        /// </summary>
        public string attach { get; set; }

        /// <summary>
        /// 支付完成时间 订单支付时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。其他详见时间规则
        /// </summary>
        public string time_end { get; set; }

        /// <summary>
        /// 交易状态描述 对当前查询订单状态的描述和下一步操作的指引
        /// </summary>
        public string trade_state_desc { get; set; }
    }
}
