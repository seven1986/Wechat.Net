using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace WeChat
{
    public class Material : Basic
    {
        public Material(string _Accecc_Token)
        {
            base.Access_Token = _Accecc_Token;
        }

        #region 新增永久图文素材
        /// <summary>
        /// 新增永久图文素材
        /// </summary>
        /// <param name="ReqStr"></param>
        /// <returns></returns>
        public JObject Add(string title,
            string anthor,
            string digest,
            int show_cover_pic,
            string content,
            string content_source_url,
            string image)
        {
            var thumb_media_id = Image_Add(image);

            return Request(material_add, JsonConvert.SerializeObject(new
                {
                    articles = new object[]
                    {
                        new
                        {
                            title =title,
                            thumb_media_id=thumb_media_id,
                            author=anthor,
                            digest=digest,
                            show_cover_pic=show_cover_pic,
                            content=content,
                            content_source_url=content_source_url
                        }
                    }
                }));
        }
        #endregion

        #region 新增永久图文素材
        /// <summary>
        /// 新增永久多个图文素材
        /// 调用此接口前需要
        /// 1，调用Image_Add方法，上传主图获取素材ID
        /// 2，调用Image_Upload方法，上传详情中的图片获取链接地址
        /// 3，转换实体集合为字符串。格式参见http://mp.weixin.qq.com/wiki/14/7e6c03263063f4813141c3e17dd4350a.html
        /// </summary>
        /// <param name="ReqStr"></param>
        /// <returns></returns>
        public JObject AddRange(string jsonStr)
        {
            return Request(material_add, jsonStr);
        }
        #endregion

        #region 新增永久图文素材
        /// <summary>
        /// 新增永久图文素材
        /// </summary>
        /// <param name="ReqStr"></param>
        /// <returns></returns>
        public string Image_Upload(string _ImagePath)
        {
            try
            {
                var ImageItem = Upload(material_uploadimg, _ImagePath);

                if (ImageItem != null && ImageItem.HasValues)
                {
                    return ImageItem["url"].Value<string>();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region 获取素材列表
        /// <summary>
        /// 获取素材列表
        /// </summary>
        /// <param name="type">素材的类型，图片（image）、视频（video）、语音 （voice）、图文（news）</param>
        /// <param name="cardId">卡券ID</param>
        /// <returns></returns>
        public JObject BatchGet(string type,int PageIndex)
        {
            var ReqStr = JsonConvert.SerializeObject(
               new
               {
                   type = type,
                   offset = PageIndex * 20,
                   count = 20
               });

            var res = Request(material_batchget, ReqStr);

            if (res != null && res.HasValues&&res["errcode"]==null)
            {
                return res;
            }

            return null;
        }
        #endregion

        #region 查询单个获取永久素材
        /// <summary>
        /// 查询单个获取永久素材
        /// </summary>
        /// <param name="cardId">素材ID</param>
        /// <returns></returns>
        public JObject Detail(string media_id)
        {
            var ReqStr = JsonConvert.SerializeObject(
               new
               {
                   media_id = media_id
               });

            var res = Request(material_get, ReqStr);

            if (res != null)
            {
                return res;
            }

            return null;
        }
        #endregion

        #region 上传图片素材
        /// <summary>
        /// 上传图片素材
        /// </summary>
        /// <param name="path">地址</param>
        /// <param name="temporary">是否为临时素材，默认为false</param>
        /// <returns></returns>
        public string Image_Add(string path,bool temporary=false)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) { return string.Empty; }

            string url = "https://api.weixin.qq.com/cgi-bin/material/add_material?access_token=" + Access_Token + "&type=image";

            if(temporary)
            {
                url = "https://api.weixin.qq.com/cgi-bin/media/upload?access_token=" + Access_Token + "&type=image";
            }

            // 设置参数
            var request = WebRequest.Create(url) as HttpWebRequest;
            var cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            int pos = path.LastIndexOf("\\");
            string fileName = path.Substring(pos + 1);

            //请求头部信息
            var sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"media\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
            var postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var bArr = new byte[fs.Length];
            fs.Read(bArr, 0, bArr.Length);
            fs.Close();

            Stream postStream = request.GetRequestStream();
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            postStream.Write(bArr, 0, bArr.Length);
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            postStream.Close();

            //发送请求并获取相应回应数据
            var response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            var instream = response.GetResponseStream();
            var sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();

            return JsonConvert.DeserializeObject<JObject>(content)["media_id"].Value<string>();
        }
        #endregion
    }
}
