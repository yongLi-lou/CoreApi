using Qj.Utility.Helper;
using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;

namespace Qj.Utility
{
    /// <summary>
    /// 邮件发送
    /// </summary>
    public class EmailSentHelper
    {
        public static string strHost = "smtp.qq.com";//STMP服务器地址
        public static string strAccount = "2097088570@qq.com"; //SMTP服务帐号
        public static string strPwd = "dtnhzmvpgbcqdgba";//SMTP服务密码
        public static string strFrom = "2097088570@qq.com";  //发送方邮件地址

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="to">收件人（多个 以, 号隔开）</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="CC">抄送人（多个 以, 号隔开）</param>
        /// <param name="Files">附件地址（多个 以, 号隔开）</param>
        public static void SentEmail(string to, string title, string content, string CC = "", string Files = "")
        {
            SmtpClient _smtpClient = new SmtpClient();
            _smtpClient.EnableSsl = true;
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式
            _smtpClient.Host = strHost; ;//指定SMTP服务器
            _smtpClient.Port = 587;//端口号  默认25
            _smtpClient.Credentials = new System.Net.NetworkCredential(strAccount, strPwd);//用户名和密码

            MailMessage _mailMessage = new MailMessage(strFrom, to);

            //收件人
            _mailMessage.To.Clear();
            foreach (string ss in to.Split(','))
            {
                if (ss != "")
                {
                    _mailMessage.To.Add(ss);
                }
            }
            CC = CC == null ? "" : CC;
            //抄送人
            foreach (string ss in CC.Split(','))
            {
                if (ss != "")
                {
                    _mailMessage.CC.Add(CC);
                }
            }

            Files = Files == null ? "" : Files;
            //附件
            foreach (string ss in Files.Split(','))
            {
                //验证是否需要传输文件
                if (ss != "" && System.IO.File.Exists(ss))
                {
                    //二进制文件传输
                    byte[] filearr = File.ReadAllBytes(ss);
                    _mailMessage.Attachments.Add(new Attachment(new MemoryStream(filearr), ss.Substring(ss.LastIndexOf("/"))));
                    //原方法-附件-出现乱码
                    //oMail.Attachments.Add(new Attachment(csMails));
                }
            }

            _mailMessage.Subject = title;//主题
            _mailMessage.Body = content;//内容
            _mailMessage.BodyEncoding = System.Text.Encoding.UTF8;//正文编码
            _mailMessage.IsBodyHtml = true;//设置为HTML格式
            _mailMessage.Priority = MailPriority.High;//优先级

            try
            {
                _smtpClient.Send(_mailMessage);
            }
            catch (Exception e)
            {
                throw e; //"密码错误或者该邮箱未开启SMTP服务 ";
            }
        }
    }
}