using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace WeChat.Net
{
    public class RedPack: Basic
    {
       public RedPack(string _Accecc_Token)
        {
            base.Access_Token = _Accecc_Token;
        }

       /// <summary>
       /// 发放红包
       /// </summary>
       /// <returns></returns>
       public string send(string mch_billno, string mch_id, string wxappid, string nick_name,
           string send_name, string re_openid, string total_amount, string min_value, string max_value,
           string total_num, string wishing, string client_ip, string act_name, string act_id,
           string remark, string logo_imgurl, string share_content, string share_url, string share_imgurl,
           string nonce_str, string mch_key, string mch_certPath)
       {
           #region 拼接参数及生成签名
           var param = new SortedList<string, string>();
           if (!string.IsNullOrEmpty(nonce_str)) { param.Add("nonce_str", nonce_str); }
           if (!string.IsNullOrEmpty(mch_billno)) { param.Add("mch_billno", mch_billno); }
           if (!string.IsNullOrEmpty(mch_id)) { param.Add("mch_id", mch_id); }
           if (!string.IsNullOrEmpty(wxappid)) { param.Add("wxappid", wxappid); }
           if (!string.IsNullOrEmpty(nick_name)) { param.Add("nick_name", nick_name); }
           if (!string.IsNullOrEmpty(send_name)) { param.Add("send_name", send_name); }
           if (!string.IsNullOrEmpty(re_openid)) { param.Add("re_openid", re_openid); }
           if (!string.IsNullOrEmpty(total_amount)) { param.Add("total_amount", total_amount); }
           if (!string.IsNullOrEmpty(min_value)) { param.Add("min_value", min_value); }
           if (!string.IsNullOrEmpty(max_value)) { param.Add("max_value", max_value); }
           if (!string.IsNullOrEmpty(total_num)) { param.Add("total_num", total_num); }
           if (!string.IsNullOrEmpty(wishing)) { param.Add("wishing", wishing); }
           if (!string.IsNullOrEmpty(client_ip)) { param.Add("client_ip", client_ip); }
           if (!string.IsNullOrEmpty(act_name)) { param.Add("act_name", act_name); }
           if (!string.IsNullOrEmpty(remark)) { param.Add("remark", remark); }
           if (!string.IsNullOrEmpty(logo_imgurl)) { param.Add("logo_imgurl", logo_imgurl); }
           if (!string.IsNullOrEmpty(share_content)) { param.Add("share_content", share_content); }
           if (!string.IsNullOrEmpty(share_url)) { param.Add("share_url", share_url); }
           if (!string.IsNullOrEmpty(share_imgurl)) { param.Add("share_imgurl", share_imgurl); }
           if (!string.IsNullOrEmpty(act_id)) { param.Add("act_id", act_id); }
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

           var ResponseStr = Request_XML(redpack_send, RequestBody.ToString(), mch_id, mch_certPath);

           try
           {
               if (!string.IsNullOrEmpty(ResponseStr))
               {
                   var Res = XElement.Load(new StringReader(ResponseStr));

                   if (Res.Element("return_code").Value.Equals("SUCCESS"))
                   {
                       if (Res.Element("result_code").Value.Equals("SUCCESS"))
                       {
                           string send_listid = Res.Element("send_listid").Value;

                           return send_listid;
                       }
                   }
               }

               return string.Empty;
           }
           catch
           {
               return string.Empty;
           }
       }
    }
}
