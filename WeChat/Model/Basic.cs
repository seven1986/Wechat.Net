namespace WeChat.Net.Model
{
  public class Basic
    {
        /// <summary>
        /// 公众账号ID 调用接口提交的公众账号ID
        /// </summary>
        public string appid { get; set; }

        /// <summary>
        /// 商户号 调用接口提交的商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 随机字符串 微信返回的随机字符串
        /// </summary>
        public string nonce_str { get; set; }

        /// <summary>
        /// 签名 微信返回的签名值，详见签名算法
        /// https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=4_3
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 业务结果 SUCCESS/FAIL
        /// </summary>
        public string result_code { get; set; }

        /// <summary>
        /// 返回状态码 SUCCESS/FAIL
        /// 此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断
        /// </summary>
        public string return_code { get; set; }

        /// <summary>
        /// 返回信息，如非空，为错误原因
        /// </summary>
        public string return_msg { get; set; }

        /// <summary>
        /// 错误代码 return_code为SUCCESS的时候有返回
        /// </summary>
        public string err_code { get; set; }

        /// <summary>
        /// 错误代码描述 return_code为SUCCESS的时候有返回
        /// </summary>
        public string err_code_des { get; set; }

    }
}
