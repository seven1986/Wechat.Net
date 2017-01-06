using Newtonsoft.Json;
using System.Dynamic;

namespace WeChat
{
    public class Messagee : Basic
    {
        public Messagee(string _Accecc_Token)
        {
            base.Access_Token = _Accecc_Token;
        }

        /// <summary>
        /// 群发消息
        /// </summary>
        /// <param name="group_id">为0代表群发全部</param>
        /// <param name="media_id">素材ID</param>
        public string SendAll(long group_id,string media_id)
        {
            dynamic reqEntity = new ExpandoObject();
                    reqEntity.filter = new ExpandoObject();
                    reqEntity.mpnews = new ExpandoObject();

            if (group_id==0)
            {
                reqEntity.filter.is_to_all = true;
            }
            else
            {
                reqEntity.filter.is_to_all = false;
                reqEntity.filter.group_id = group_id.ToString();
            }

            reqEntity.mpnews.media_id = media_id;

            reqEntity.msgtype = "mpnews";

            var res = Request(base.Message_SendAll,JsonConvert.SerializeObject(reqEntity));

            return res["msg_id"].Tostring();
        }


        /// <summary>
        /// 群发消息
        /// </summary>
        /// <param name="openid">粉丝的OpenID</param>
        /// <param name="media_id">素材ID</param>
        public string Preview(string openid, string media_id)
        {
            var res = Request(base.Message_Preview, JsonConvert.SerializeObject(new
            {
                touser=openid,
                mpnews = new
                {
                    media_id= media_id
                },
                msgtype= "mpnews"
            }));

            return res["errcode"].ToString();
        }
    }
}
