namespace WeChat
{
   public class WeChatSDK
    {
        /// <summary>
        /// 初始化微信SDK配置
        /// </summary>
        /// <param name="app_id">公众账号应用ID</param>
        /// <param name="app_secret">公众账号应用密钥</param>
        /// <param name="mch_id">微支付商户号</param>
        /// <param name="mch_key">微支付的key</param>
        /// <param name="mch_certPath">微支付证书路径</param>
        public static void Initialize(string app_id, string app_secret,string mch_id,string mch_key,string mch_certPath)
        {
            Basic.app_id = app_id;
            Basic.app_secret = app_secret;
            Basic.mch_id = mch_id;
            Basic.mch_key = mch_key;
            Basic.mch_certPath = mch_certPath;
        }
    }
}
