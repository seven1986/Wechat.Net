using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace WeChat.Net
{
    public class MessageTemplate: Basic
    {
       public MessageTemplate(string _Accecc_Token)
        {
            base.Access_Token = _Accecc_Token;
        }


        /// <summary>
        /// 发送模板消息
        /// 其中TemplateObject参数参考如下
        ///  var temp = new
        ///  {
        ///     first = new { value = "seven", color = "#743A3A" },
        ///     keyword1 = new { value = "0.01", color = "#FF0000" },
        ///     keyword2 = new { value = "0.01", color = "#C4C400" },
        ///     remark = new { value = "感谢您的关注", color = "0000FF" }
        ///  };
        /// </summary>
        /// <param name="OpenID">用户的OpenID</param>
        /// <param name="TemplateID">模板消息ID</param>
        /// <param name="url">URL置空，则在发送后，点击模板消息会进入一个空白页面（ios），或无法点击（android）。</param>
        /// <param name="topcolor">上边框颜色</param>
        /// <param name="TemplateObject">具体替换的模板内容</param>
        /// <returns></returns>
        public string Send(string OpenID, string TemplateID, string url, string topcolor,Object TemplateObject)
       {
           var ReqStr = JsonConvert.SerializeObject(new
           {
               touser = OpenID,
               template_id = TemplateID,
               url = url,
               topcolor = topcolor,
               data = TemplateObject
           });

           var res = Request(TemplateMsg_Send, ReqStr);

           if (res != null && res.HasValues && res["errcode"].Value<int>() ==0)
           {
               return res["msgid"].Value<string>();
           }

           return string.Empty;
       }
    }
}
