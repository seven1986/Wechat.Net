namespace WeChat.Net.Model
{
    public class FansModel
    {
        public int total
        {
            get; set;
        }
        public int count
        {
            get; set;
        }

        public FansItem data { get; set; }
        public string next_openid { get; set; }
    }

    public class FansItem
    {
        public string[] openid { get; set; }
    }

    public class FansInfo
    {
        public int subscribe { get; set;}
        public string openid { get; set; }
        public string nickname { get; set; }
        public int sex { get; set; }
        public string language { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public int subscribe_time { get; set; }
        public string remark { get; set; }
        public int groupid { get; set; }
    }
}
