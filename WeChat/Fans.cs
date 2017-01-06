using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeChat
{
    public class Fans: Basic
    {
        public Fans()
        {
            base.Access_Token = base.RequestToken();
        }

        public Fans(string _Accecc_Token)
        {
            base.Access_Token = _Accecc_Token;
        }

        #region 查询粉丝
        public FansModel Batch_Get(string next_openid = "")
        {
            var res = Get("https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + Access_Token + "&next_openid=" + next_openid);

            var result = JsonConvert.DeserializeObject<FansModel>(res);

            return result;
        }
        #endregion

        public FansInfo Detail(string OPENID)
        {
            var res = Get("https://api.weixin.qq.com/cgi-bin/user/info?access_token="+ Access_Token + "&openid="+ OPENID + "&lang=zh_CN");

            var result = JsonConvert.DeserializeObject<FansInfo>(res);

            return result;
        }

        public JObject SendMsg(string openID,string content)
        {
            var ReqStr = JsonConvert.SerializeObject(new
            {
                touser = openID,
                msgtype = "text",
                text = new
                {
                    content = content
                }
            });

            var res = Request("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + Access_Token, ReqStr);

            return res;
        }

        public JObject SendByImage(string openID, string media_id)
        {
            var ReqStr = JsonConvert.SerializeObject(new
            {
                touser = openID,
                msgtype = "image",
                image = new
                {
                    media_id = media_id
                }
            });

            var res = Request("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + Access_Token, ReqStr);

            return res;
        }

        public JObject SendByNews(string openID, object _articles)
        {
            var ReqStr = JsonConvert.SerializeObject(new
            {
                touser = openID,
                msgtype = "news",
                news = new
                {
                    articles = _articles
                }
            });

            var res = Request("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + Access_Token, ReqStr);

            return res;
        }
    }
}
