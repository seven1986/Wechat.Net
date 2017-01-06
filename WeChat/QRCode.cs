using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeChat
{
    public class QRCode: Basic
    {
        public QRCode()
        {
            this.Access_Token = base.RequestToken();
        }

        public QRCode(string _Accecc_Token) { base.Access_Token = _Accecc_Token; }

        /// <summary>
        /// 创建商家二维码
        /// </summary>
        /// <param name="MechantID">商家ID</param>
        /// <returns></returns>
        public string Create(long MechantID)
       {
           var ReqStr = JsonConvert.SerializeObject(new
           {
                action_name = "QR_LIMIT_SCENE",
                action_info = new
                {
                    scene = new
                    {
                        scene_id = MechantID
                    }
                }
            });

           var res = Request(QRCode_Create, ReqStr);

           if (res != null && res.HasValues && res["ticket"]!= null)
           {
               return QRCode_Show + res["ticket"].Value<string>();
           }

           return string.Empty;
       }
    }
}
