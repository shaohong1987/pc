using cn.jpush.api;
using cn.jpush.api.common;
using cn.jpush.api.push.mode;

namespace TheseThree.Admin.Utils
{
    public class PushHelper
    {
        public const string AppKey = "6c0b58e6f2e72dac11c55bb9";
        public const string MasterSecret = "a1a8ed874f486ae3188de5d5";

        public static void DoPush(int type,string[] flag,string content,string title)
        {
            JPushClient client = new JPushClient(AppKey, MasterSecret);
            PushPayload payload = null;
            switch (type)
            {
                case -1:
                    payload = PushObject_All_All_Alert(content);
                    break;
                case 0:
                    payload = PushObject_all_alias_alert(flag, content);
                    break;
                case 1:
                    payload = PushObject_Android_Tag_AlertWithTitle(flag, content, title);
                    break;
                case 2:
                    payload = PushObject_iOS_Tag_AlertWithTitle(flag, content);
                    break;
            }
            if (payload != null)
            {
                try
                {
                    client.SendPush(payload);
                    System.Threading.Thread.Sleep(1000);
                }
                catch (APIRequestException)
                {
                }
            }
        }

        /// <summary>
        /// 推送给所有平台所有用户
        /// </summary>
        /// <returns></returns>
        public static PushPayload PushObject_All_All_Alert(string content)
        {
            PushPayload pushPayload = new PushPayload
            {
                platform = Platform.all(),
                audience = Audience.all(),
                notification = new Notification().setAlert(content)
            };
            return pushPayload;
        }


        /// <summary>
        /// 推送给所有平台别名是alias的用户
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static PushPayload PushObject_all_alias_alert(string[] alias,string content)
        {

            PushPayload pushPayloadAlias = new PushPayload();
            pushPayloadAlias.platform = Platform.android();
            pushPayloadAlias.audience = Audience.s_alias(alias);
            pushPayloadAlias.notification = new Notification().setAlert(content);
            return pushPayloadAlias;
        }

        /// <summary>
        /// 推送给所有android平台tag的用户
        /// </summary>
        /// <returns></returns>
        public static PushPayload PushObject_Android_Tag_AlertWithTitle(string[] tag,string content,string title)
        {
            PushPayload pushPayload = new PushPayload
            {
                platform = Platform.android(),
                audience = Audience.s_tag(tag),
                notification = Notification.android(content, title)
            };
            return pushPayload;
        }

        /// <summary>
        /// 推送给所有ios平台的tag用户
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static PushPayload PushObject_iOS_Tag_AlertWithTitle(string[] tag, string content)
        {
            PushPayload pushPayload = new PushPayload
            {
                platform = Platform.ios(),
                audience = Audience.s_tag(tag),
                notification = Notification.ios(content)
            };
            return pushPayload;
        }

    }
}