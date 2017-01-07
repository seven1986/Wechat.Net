using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace WeChat.Net
{
    /// <summary>
    /// 用户分组
    /// </summary>
    public class Groups : Basic
    {
        public Groups(string _Accecc_Token)
        {
            base.Access_Token = _Accecc_Token;
        }

        public GroupsGetReponse Get()
        {
            var res = Get(base.Groups_Get);

            var result = JsonConvert.DeserializeObject<GroupsGetReponse>(res);

            return result;
        }

        public long Add(string name)
        {
            var res = Request(base.Groups_Add, JsonConvert.SerializeObject(new
            {
                group = new
                {
                    name = name
                }
            }));

            try
            {
                return res["group"]["id"].Value<long>();
            }
            catch
            {
                throw new Exception(JsonConvert.SerializeObject(res));
            }
        }

        public bool Update(long id ,string name)
        {
            //微信平台中（0默认组，1屏蔽组，2星标组）无法修改、删除。
            if (id == 0 || id == 1 || id == 2) { return true; }

            var res = Request(base.Groups_Update,
                JsonConvert.SerializeObject(
                    new
                    {
                        group = new
                        {
                            id = id,
                            name = name
                        }
                    }));

            return res["errcode"].Value<long>() == 0;
        }

        public void Delete(long id)
        {
            if (id == 0 || id == 1 || id == 2) { return; }
            var res = Request(base.Groups_Delete, JsonConvert.SerializeObject(
                new
                {
                    group = new
                    {
                        id = id
                    }
                }));
        }

        public bool UserUpdate(string openid,long id)
        {
            var res = Request(base.Groups_UserUpdate, JsonConvert.SerializeObject(
               new
               {
                   openid = openid,
                   to_groupid = id
               }));

            try
            {
                return res["errcode"].Value<int>() == 0;
            }
            catch
            {
                throw new Exception(JsonConvert.SerializeObject(res));
            }
        }
    }

    public class GroupsGetReponse
    {
        public List<GroupsItem> groups { get; set; }

        public class GroupsItem
        {
            public long id { get; set; }
            public string name { get; set; }
            public long count { get; set; }
        }
    }
}
