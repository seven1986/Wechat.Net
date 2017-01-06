using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace WeChat
{
    public class Pay : Basic
    {
        public Pay(string _Accecc_Token)
        {
            base.Access_Token = _Accecc_Token;
        }

        /// <summary>
        /// 统一下单(生成预支付交易单)
        /// </summary>
        /// <param name="device_info">设备号（终端设备号(门店号或收银设备ID)，注意：PC网页或公众号内支付请传"WEB"）</param>
        /// <param name="nonce_str">随机字符串（随机字符串，不长于32位。DateTime.Now.Ticks.ToString()）</param>
        /// <param name="body">商品描述（商品或支付单简要描述）</param>
        /// <param name="detail">商品详情（商品名称明细列表）</param>
        /// <param name="attach">附加数据（附加数据，在查询API和支付通知中原样返回，该字段主要用于商户携带订单的自定义数据）</param>
        /// <param name="out_trade_no">商户订单号（商户系统内部的订单号,32个字符内、可包含字母）</param>
        /// <param name="fee_type">货币类型（符合ISO 4217标准的三位字母代码，默认人民币：CNY）</param>
        /// <param name="total_fee">总金额（订单总金额，单位为分）</param>
        /// <param name="spbill_create_ip">终端IP（APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP）</param>
        /// <param name="time_start">交易起始时间（订单生成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010）</param>
        /// <param name="time_expire">交易结束时间（订单失效时间，格式为yyyyMMddHHmmss，如2009年12月27日9点10分10秒表示为20091227091010）</param>
        /// <param name="goods_tag">商品标记（商品标记，代金券或立减优惠功能的参数）</param>
        /// <param name="notify_url">通知地址（接收微信支付异步通知回调地址，通知url必须为直接可访问的url，不能携带参数）</param>
        /// <param name="trade_type">交易类型（取值如下：JSAPI，NATIVE，APP）</param>
        /// <param name="product_id">商品ID（trade_type=NATIVE，此参数必传。此id为二维码中包含的商品ID，商户自行定义）</param>
        /// <param name="limit_pay">指定支付方式（no_credit--指定不能使用信用卡支付）</param>
        /// <param name="openid">用户标识（trade_type=JSAPI，此参数必传，用户在商户appid下的唯一标识）</param>
        /// <returns>返回一个长度为2的数组，1：预支付交易会话标识，2：二维码链接（trade_type为NATIVE是有返回，可将该参数值生成二维码展示出来进行扫码支付）</returns>
        public string[] Unifiedorder(string device_info,string nonce_str,
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

            try
            {
                    if (!string.IsNullOrEmpty(ResponseStr))
                    {
                        var Res = XElement.Load(new StringReader(ResponseStr));

                        if (Res.Element("return_code").Value.Equals("SUCCESS"))
                        {
                        if (Res.Element("result_code").Value.Equals("SUCCESS"))
                        {
                            var prepay_id = Res.Element("prepay_id").Value;

                            var code_url = string.Empty;

                            if (trade_type.Equals("NATIVE"))
                            {
                                code_url = Res.Element("code_url").Value;
                            }

                            return new string[2] { prepay_id, code_url };
                        }
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="appid">公众账号ID（微信分配的公众账号ID（企业号corpid即为此appId））</param>
        /// <param name="mch_id">商户号（微信支付分配的商户号）</param>
        /// <param name="out_trade_no">商户订单号（商户系统内部的订单号,32个字符内、可包含字母）</param>
        /// <param name="nonce_str">随机字符串（随机字符串，不长于32位。DateTime.Now.Ticks.ToString()）</param>
        /// <param name="mch_key">微信支付的key</param>
        /// <returns>返回一个长度为2的数组，1：预支付交易会话标识，2：二维码链接（trade_type为NATIVE是有返回，可将该参数值生成二维码展示出来进行扫码支付）</returns>
        public Pay_OrderQueryEntity Orderquery(string appid, string mch_id, string out_trade_no, string nonce_str,string mch_key)
        {
            #region 拼接参数及生成签名
            var param = new SortedList<string, string>();
            if (!string.IsNullOrEmpty(appid)) { param.Add("appid", appid); }
            if (!string.IsNullOrEmpty(mch_id)) { param.Add("mch_id", mch_id); }
            if (!string.IsNullOrEmpty(nonce_str)) { param.Add("nonce_str", nonce_str); }
            if (!string.IsNullOrEmpty(out_trade_no)) { param.Add("out_trade_no", out_trade_no); }
            var stringA = new StringBuilder();
            foreach (var kv in param)
            {
                stringA.Append(kv.Key + "=" + kv.Value + "&");
            }
            string stringSignTemp = stringA.ToString() + "key=" + mch_key;
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

            try
            {
                if (!string.IsNullOrEmpty(ResponseStr))
                {
                    var Res = XElement.Load(new StringReader(ResponseStr));

                    if (Res.Element("return_code").Value.Equals("SUCCESS"))
                    {
                        if (Res.Element("result_code").Value.Equals("SUCCESS"))
                        {
                            var result = new Pay_OrderQueryEntity();
                            var attach = Res.Element("attach");
                            if (attach!=null){result.attach = attach.Value;}

                            var cash_fee_type = Res.Element("cash_fee_type");
                            if (cash_fee_type != null) { result.cash_fee_type = cash_fee_type.Value; }

                            var device_info = Res.Element("device_info");
                            if (device_info != null) { result.device_info = device_info.Value; }

                            var is_subscribe = Res.Element("is_subscribe");
                            if (is_subscribe != null) { result.is_subscribe = is_subscribe.Value; }

                            var openid = Res.Element("openid");
                            if (openid != null) { result.openid = openid.Value; }

                            var _out_trade_no = Res.Element("out_trade_no");
                            if (_out_trade_no != null) { result.out_trade_no = _out_trade_no.Value; }

                            var time_end = Res.Element("time_end");
                            if (time_end != null) { result.time_end = time_end.Value; }

                            var total_fee = Res.Element("total_fee");
                            if (total_fee != null) { result.total_fee = total_fee.Value; }

                            var trade_state = Res.Element("trade_state");
                            if (trade_state != null) { result.trade_state = trade_state.Value; }

                            var trade_state_desc = Res.Element("trade_state_desc");
                            if (trade_state_desc != null) { result.trade_state_desc = trade_state_desc.Value; }

                            var trade_type = Res.Element("trade_type");
                            if (trade_type != null) { result.trade_type = trade_type.Value; }

                            var bank_type = Res.Element("bank_type");
                            if (bank_type != null) { result.bank_type = bank_type.Value; }

                            var transaction_id = Res.Element("transaction_id");
                            if (transaction_id != null) { result.transaction_id = transaction_id.Value; }

                            var fee_type = Res.Element("fee_type");
                            if (fee_type!=null){ result.fee_type = fee_type.Value;}

                            var cash_fee = Res.Element("cash_fee");
                            if (cash_fee!=null){result.cash_fee = int.Parse(cash_fee.Value);}

                            var coupon_count = Res.Element("coupon_count");
                            if (coupon_count!=null){result.coupon_count = int.Parse(coupon_count.Value);}

                            var coupon_fee = Res.Element("coupon_fee");
                            if (coupon_fee!=null){result.coupon_fee = int.Parse(coupon_fee.Value);}

                            /*
                            var coupon_batch_id_n = Res.Element("coupon_batch_id_$n");
                            if (coupon_batch_id_n != null) { result.coupon_batch_id_n = coupon_batch_id_n.Value; }
                            var coupon_id_n = Res.Element("coupon_id_$n");
                            if (coupon_id_n != null) { result.coupon_id_n = coupon_id_n.Value; }
                            var coupon_fee_n = Res.Element("coupon_fee_$n");
                            if (!string.IsNullOrEmpty(coupon_fee_n)) { result.coupon_fee_n = int.Parse(coupon_fee_n); }
                            */

                            return result;
                        }
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 企业付款
        /// 返回的参数还没做处理，暂时用不到这个接口，不写了
        /// </summary>
        public Pay_OrderQueryEntity transfers(string appid, string mchid, string device_info,
            string nonce_str,string partner_trade_no, string openid,int amount,string desc,string spbill_create_ip, string mch_key, string mch_certPath)
        {
            #region 拼接参数及生成签名
            var param = new SortedList<string, string>();
            if (!string.IsNullOrEmpty(appid)) { param.Add("mch_appid", appid); }
            if (!string.IsNullOrEmpty(mchid)) { param.Add("mchid", mchid); }
            if (!string.IsNullOrEmpty(device_info)) { param.Add("device_info", device_info); }
            if (!string.IsNullOrEmpty(nonce_str)) { param.Add("nonce_str", nonce_str); }
            if (!string.IsNullOrEmpty(partner_trade_no)) { param.Add("partner_trade_no", partner_trade_no); }
            if (!string.IsNullOrEmpty(openid)) { param.Add("openid", openid); }

            param.Add("check_name", "NO_CHECK");
            param.Add("amount", amount.ToString());
            if (!string.IsNullOrEmpty(desc)) { param.Add("desc", desc); }
            if (!string.IsNullOrEmpty(spbill_create_ip)) { param.Add("spbill_create_ip", spbill_create_ip); }

            var stringA = new StringBuilder();
            foreach (var kv in param)
            {
                stringA.Append(kv.Key + "=" + kv.Value + "&");
            }
            string stringSignTemp = stringA.ToString() + "key=" + mch_key;
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

            var ResponseStr = Request_XML(pay_transfers, RequestBody.ToString(), mchid, mch_certPath);

            try
            {
                if (!string.IsNullOrEmpty(ResponseStr))
                {
                    var Res = XElement.Load(new StringReader(ResponseStr));

                    if (Res.Element("return_code").Value.Equals("SUCCESS"))
                    {
                        if (Res.Element("result_code").Value.Equals("SUCCESS"))
                        {
                            return null;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    /// <summary>
    /// 查询订单返回的实体
    /// </summary>
    public class Pay_OrderQueryEntity
    {
        /// <summary>
        /// 设备号。微信支付分配的终端设备号
        /// </summary>
        public string device_info { get; set; }
        /// <summary>
        /// 用户标识。用户在商户appid下的唯一标识
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 是否关注公众账号。用户是否关注公众账号，Y-关注，N-未关注，仅在公众账号类型支付有效
        /// </summary>
        public string is_subscribe { get; set; }
        /// <summary>
        /// 交易类型。调用接口提交的交易类型，取值如下：JSAPI，NATIVE，APP，MICROPAY
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 交易状态。UCCESS—支付成功
        /// REFUND—转入退款
        /// NOTPAY—未支付
        /// CLOSED—已关闭
        /// REVOKED—已撤销（刷卡支付）
        /// USERPAYING--用户支付中
        /// PAYERROR--支付失败(其他原因，如银行返回失败)
        /// </summary>
        public string trade_state { get; set; }
        /// <summary>
        /// 付款银行。银行类型，采用字符串类型的银行标识
        /// </summary>
        public string bank_type { get; set; }
        /// <summary>
        /// 总金额。订单总金额，单位为分
        /// </summary>
        public string total_fee { get; set; }
        /// <summary>
        /// 货币种类。货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型https://pay.weixin.qq.com/wiki/doc/api/native.php?chapter=4_2
        /// </summary>
        public string fee_type { get; set; }
        /// <summary>
        /// 现金支付金额。现金支付金额订单现金支付金额，详见支付金额https://pay.weixin.qq.com/wiki/doc/api/native.php?chapter=4_2
        /// </summary>
        public int cash_fee { get; set; }
        /// <summary>
        /// 现金支付货币类型。货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型https://pay.weixin.qq.com/wiki/doc/api/native.php?chapter=4_2
        /// </summary>
        public string cash_fee_type { get; set; }
        /// <summary>
        /// 代金券或立减优惠金额。代金券或立减优惠”金额<=订单总金额，订单总金额-“代金券或立减优惠”金额=现金支付金额，详见支付金额https://pay.weixin.qq.com/wiki/doc/api/native.php?chapter=4_2
        /// </summary>
        public int coupon_fee { get; set; }
        /// <summary>
        /// 代金券或立减优惠使用数量。代金券或立减优惠使用数量
        /// </summary>
        public int coupon_count { get; set; }
        /// <summary>
        /// 代金券或立减优惠批次ID。代金券或立减优惠批次ID ,$n为下标，从0开始编号
        /// </summary>
        public string coupon_batch_id_n{ get; set; }
        /// <summary>
        /// 代金券或立减优惠ID。代金券或立减优惠ID, $n为下标，从0开始编号
        /// </summary>
        public string coupon_id_n{ get; set; }
        /// <summary>
        /// 单个代金券或立减优惠支付金额。单个代金券或立减优惠支付金额, $n为下标，从0开始编号
        /// </summary>
        public int coupon_fee_n{ get; set; }
        /// <summary>
        /// 微信支付订单号。
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 商户订单。商户系统的订单号，与请求一致。
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 附加数据。附加数据，原样返回
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// 支付完成时间。订单支付时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。其他详见时间规则https://pay.weixin.qq.com/wiki/doc/api/native.php?chapter=4_2
        /// </summary>
        public string time_end { get; set; }
        /// <summary>
        /// 交易状态描述。对当前查询订单状态的描述和下一步操作的指引
        /// </summary>
        public string trade_state_desc { get; set; }
    }
}
