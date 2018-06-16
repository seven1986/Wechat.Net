using System;
using System.Collections.Generic;
using System.Text;
using WeChat.Net.Core.Model;

namespace WeChat.Net.Core
{
    /// <summary>
    /// 微信支付
    /// https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=9_1
    /// </summary>
    public class Pay : Basic
    {
        public Pay()
        {
            base.Access_Token = base.RequestToken();
        }

        public Pay(string _Accecc_Token)
        {
            base.Access_Token = _Accecc_Token;
        }


        /// <summary>
        /// 扫码付
        /// 统一下单 for Native (生成预支付交易单)
        /// </summary>
        /// <param name="trade_no">商户订单号（商户系统内部的订单号,32个字符内、可包含字母）</param>
        /// <param name="total_fee">总金额（订单总金额，单位为分）</param>
        /// <param name="product_id">商品ID（此id为二维码中包含的商品ID，商户自行定义）</param>
        /// <param name="subject">商品描述（商品或支付单简要描述）</param>
        /// <param name="client_ip">终端IP（调用微信支付API的机器IP）</param>
        /// <returns></returns>
        public unifiedOrderResponse NativePay(string trade_no, long total_fee, string product_id, string subject, string client_ip = "127.0.0.1")
        {
            var result = this.unifiedOrder("WEB",
                 DateTime.Now.Ticks.ToString(),
                 subject,
                 string.Empty,
                 string.Empty,
                 trade_no,
                 "CNY",
                 total_fee,
                 client_ip,
                 string.Empty,
                 string.Empty,
                 string.Empty,
                 Basic.payment_notice_address,
                 "NATIVE",
                 product_id,
                 string.Empty,
                 string.Empty);

            return result;
        }


        /// <summary>
        /// 网页、公众号支付
        /// 统一下单 for HTML5 (生成预支付交易单)
        /// </summary>
        /// <param name="trade_no">商户订单号（商户系统内部的订单号,32个字符内、可包含字母）</param>
        /// <param name="total_fee">总金额（订单总金额，单位为分）</param>
        /// <param name="openid">用户在商户appid下的唯一标识</param>
        /// <param name="subject">商品描述（商品或支付单简要描述）</param>
        /// <param name="client_ip">终端IP（APP和网页支付提交用户端ip）</param>
        /// <returns></returns>
        public unifiedOrderResponse H5Pay(string trade_no, long total_fee, string openid, string subject, string client_ip = "127.0.0.1")
        {
            var result = this.unifiedOrder("WEB",
                 DateTime.Now.Ticks.ToString(),
                 subject,
                 string.Empty,
                 string.Empty,
                 trade_no,
                 "CNY",
                 total_fee,
                 client_ip,
                 string.Empty,
                 string.Empty,
                 string.Empty,
                 Basic.payment_notice_address,
                 "JSAPI",
                 string.Empty,
                 string.Empty,
                 openid);

            return result;
        }

        /// <summary>
        /// APP原生支付
        /// 统一下单 for APP (生成预支付交易单)
        /// </summary>
        /// <param name="device_info">设备号（终端设备号(门店号或收银设备ID)</param>
        /// <param name="trade_no">商户订单号（商户系统内部的订单号,32个字符内、可包含字母）</param>
        /// <param name="total_fee">总金额（订单总金额，单位为分）</param>
        /// <param name="subject">商品描述（商品或支付单简要描述）</param>
        /// <param name="client_ip">终端IP（APP和网页支付提交用户端ip）</param>
        /// <returns></returns>
        public unifiedOrderResponse APPPay(string device_info, string trade_no, long total_fee, string subject, string client_ip = "127.0.0.1")
        {
            var result = this.unifiedOrder(device_info,
                 DateTime.Now.Ticks.ToString(),
                 subject,
                 string.Empty,
                 string.Empty,
                 trade_no,
                 "CNY",
                 total_fee,
                 client_ip,
                 string.Empty,
                 string.Empty,
                 string.Empty,
                 Basic.payment_notice_address,
                 "APP",
                 string.Empty,
                 string.Empty,
                 string.Empty);

            return result;
        }

        /// <summary>
        /// 统一下单(生成预支付交易单)
        /// https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=9_1
        /// </summary>
        /// <param name="device_info">设备号（终端设备号(门店号或收银设备ID)，注意：PC网页或公众号内支付请传"WEB"）</param>
        /// <param name="nonce_str">随机字符串（随机字符串，不长于32位。DateTime.Now.Ticks.ToString()）</param>
        /// <param name="body">商品描述（商品或支付单简要描述） 必填</param>
        /// <param name="detail">商品详情（商品名称明细列表）非必填
        /// 使用Json格式，传输签名前请务必使用CDATA标签将JSON文本串保护起来。
        /// {
        /// "cost_price":608800,// 可选 32 订单原价，商户侧一张小票订单可能被分多次支付，订单原价用于记录整张小票的支付金额。当订单原价与支付金额不相等则被判定为拆单，无法享受优惠。
        /// "receipt_id":"wx123",// 可选 32 商家小票ID
        ///  "goods_detail":[  //goods_detail 服务商必填
        ///     {
        ///     "goods_id":"商品编码", //商品的编号
        ///     "wxpay_goods_id":"1001", //微信支付定义的统一商品编号
        ///     "goods_name":"iPhone6s 16G", //商品名称
        ///     "quantity":1, //商品数量
        ///     "price":528800 //商品单价，如果商户有优惠，需传输商户优惠后的单价
        ///     }
        ///  ]
        ///  }
        /// </param>
        /// <param name="attach">附加数据 非必填 String(127)（附加数据，在查询API和支付通知中原样返回，该字段主要用于商户携带订单的自定义数据）</param>
        /// <param name="out_trade_no">商户订单号 必填（商户系统内部的订单号,32个字符内、可包含字母）</param>
        /// <param name="fee_type">货币类型（符合ISO 4217标准的三位字母代码，默认人民币：CNY）</param>
        /// <param name="total_fee">总金额（订单总金额，单位为分）</param>
        /// <param name="spbill_create_ip">终端IP（APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP）</param>
        /// <param name="time_start">交易起始时间 非必填（订单生成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010）</param>
        /// <param name="time_expire">交易结束时间 非必填（订单失效时间，格式为yyyyMMddHHmmss，如2009年12月27日9点10分10秒表示为20091227091010）</param>
        /// <param name="goods_tag">商品标记（商品标记，代金券或立减优惠功能的参数）</param>
        /// <param name="notify_url">通知地址（接收微信支付异步通知回调地址，通知url必须为直接可访问的url，不能携带参数）</param>
        /// <param name="trade_type">交易类型（取值如下：JSAPI，NATIVE，APP）</param>
        /// <param name="product_id">商品ID（trade_type=NATIVE，此参数必传。此id为二维码中包含的商品ID，商户自行定义）</param>
        /// <param name="limit_pay">指定支付方式（no_credit--指定不能使用信用卡支付）</param>
        /// <param name="openid">用户标识（trade_type=JSAPI，此参数必传，用户在商户appid下的唯一标识）</param>
        /// <returns>返回一个长度为2的数组，1：预支付交易会话标识，2：二维码链接（trade_type为NATIVE是有返回，可将该参数值生成二维码展示出来进行扫码支付）</returns>
        public unifiedOrderResponse unifiedOrder(string device_info,string nonce_str,
            string body,string detail,string attach,string out_trade_no,string fee_type,long total_fee,
            string spbill_create_ip,string time_start,string time_expire,string goods_tag,string notify_url,
            string trade_type,string product_id,string limit_pay,string openid)
        {
            #region 拼接参数及生成签名
            var param = new SortedList<string, string>();
                param.Add("appid", Basic.app_id);
                param.Add("mch_id", Basic.mch_id);
            if (!string.IsNullOrEmpty(device_info)) { param.Add("device_info", device_info); }
            if (!string.IsNullOrEmpty(nonce_str)) { param.Add("nonce_str", nonce_str); }
            if (!string.IsNullOrEmpty(body)) { param.Add("body", body); }
            if (!string.IsNullOrEmpty(detail)) { param.Add("detail", detail); }
            if (!string.IsNullOrEmpty(attach)) { param.Add("attach", attach); }
            if (!string.IsNullOrEmpty(out_trade_no)) { param.Add("out_trade_no", out_trade_no); }
            if (!string.IsNullOrEmpty(fee_type)) { param.Add("fee_type", fee_type); }
            if (total_fee>0) { param.Add("total_fee", total_fee.ToString()); }
            if (!string.IsNullOrEmpty(spbill_create_ip)) { param.Add("spbill_create_ip", spbill_create_ip); }
            if (!string.IsNullOrEmpty(time_start)) { param.Add("time_start", time_start); }
            if (!string.IsNullOrEmpty(time_expire)) { param.Add("time_expire", time_expire); }
            if (!string.IsNullOrEmpty(goods_tag)) { param.Add("goods_tag", goods_tag); }
            if (!string.IsNullOrEmpty(notify_url)) { param.Add("notify_url", notify_url); }
            if (!string.IsNullOrEmpty(trade_type)) { param.Add("trade_type", trade_type); }
            if (!string.IsNullOrEmpty(product_id)) { param.Add("product_id", product_id); }
            if (!string.IsNullOrEmpty(limit_pay)) { param.Add("limit_pay", limit_pay); }
            if (!string.IsNullOrEmpty(openid)) { param.Add("openid", openid); }
            var stringA = new StringBuilder();
            foreach (var kv in param)
            {
                stringA.Append(kv.Key + "=" + kv.Value + "&");
            }
            string stringSignTemp = stringA.ToString() + "key=" + Basic.mch_key;
            string sign = MD5(stringSignTemp).ToUpper();
            param.Add("sign", sign);
            #endregion

            #region 生成请求字符串
            var RequestBody = new StringBuilder();
            RequestBody.AppendLine("<xml>");
            foreach (var v in param)
            {
                RequestBody.Append("<" + v.Key + ">");
                RequestBody.Append(v.Value);
                RequestBody.AppendLine("</" + v.Key + ">");
            }
            RequestBody.AppendLine("</xml>");
            #endregion

            var ResponseStr = Request_XML(pay_unifiedorder, RequestBody.ToString());

            var Result = Deserialize<unifiedOrderResponse>(ResponseStr);

            return Result;
        }


        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="out_trade_no">商户订单号（商户系统内部的订单号,32个字符内、可包含字母）</param>
        public orderQueryResponse Query(string out_trade_no)
        {
            #region 拼接参数及生成签名
            var param = new SortedList<string, string>();
            param.Add("appid", Basic.app_id);
            param.Add("mch_id", Basic.mch_id);
            param.Add("nonce_str", DateTime.Now.Ticks.ToString());
            if (!string.IsNullOrEmpty(out_trade_no)) { param.Add("out_trade_no", out_trade_no); }
            var stringA = new StringBuilder();
            foreach (var kv in param)
            {
                stringA.Append(kv.Key + "=" + kv.Value + "&");
            }
            string stringSignTemp = stringA.ToString() + "key=" + Basic.mch_key;
            string sign = MD5(stringSignTemp).ToUpper();
            param.Add("sign", sign);
            #endregion

            #region 生成请求字符串
            var RequestBody = new StringBuilder();
            RequestBody.AppendLine("<xml>");
            foreach (var v in param)
            {
                RequestBody.Append("<" + v.Key + ">");
                RequestBody.Append(v.Value);
                RequestBody.AppendLine("</" + v.Key + ">");
            }
            RequestBody.AppendLine("</xml>");
            #endregion

            var ResponseStr = Request_XML(pay_orderquery, RequestBody.ToString());

            var Result = Deserialize<orderQueryResponse>(ResponseStr);

            return Result;
        }
    }
}
