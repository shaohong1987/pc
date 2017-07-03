namespace TheseThree.Admin.Models.Entities
{
    public class Message
    {
        //提示内容
        public string Msg { get; set; }

        //返回的数据
        public object Data { get; set; }

        //状态，-1:出错了,0：失败，1：成功
        public MessageType Status { get; set; }
    }
}