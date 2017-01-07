using Newtonsoft.Json.Linq;
using System;

namespace WeChat.Net
{
    public class Menu : Basic
    {
        public Menu(string _Accecc_Token)
        {
            base.Access_Token = _Accecc_Token;
        }
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="ReqStr"></param>
        /// <returns></returns>
        public Boolean Add(string ReqStr)
        {
            var res = Request(menu_add, ReqStr);

            if (res != null && res.HasValues && res["errcode"].Value<int>() == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 同步菜单
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        public JObject Get()
        {
            var res = Request(menu_Get, "");
            if (res != null)
            {
                return res;
            }
            return null;
        }


    }
}
