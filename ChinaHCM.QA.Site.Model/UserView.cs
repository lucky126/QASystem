using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaHCM.QA.Core.Model;
using System.ComponentModel;

namespace ChinaHCM.QA.Site.Model
{
    public class UserView
    {
        public int UserId { get; private set; }

        [DisplayName("用户类型")]
        public UserLevel Level { get; private set; }

        [DisplayName("用户名")]
        public string LoginName { get; private set; }

        [DisplayName("密码")]
        public string Password { get; private set; }

        [DisplayName("电子邮箱")]
        public string UserEmail { get; private set; }

        [DisplayName("用户签名")]
        public string UserSign { get; private set; }

        [DisplayName("昵称")]
        public string NickName { get; private set; }

        [DisplayName("真实姓名")]
        public string RealName { get; private set; }

        [DisplayName("性别")]
        public string Gender { get; private set; }

        [DisplayName("主帖数")]
        public int TopicCnt { get; private set; }

        [DisplayName("回帖数")]
        public int PostCnt { get; private set; }

        [DisplayName("被删帖数")]
        public int DelCnt { get; private set; }

        [DisplayName("登录次数")]
        public int LoginCnt { get; private set; }

        [DisplayName("积分")]
        public int Grade { get; private set; }

        [DisplayName("上次登录时间")]
        public System.DateTime LastLoginTime { get; private set; }

        [DisplayName("添加时间")]
        public System.DateTime AddTime { get; private set; }

        public bool IsAdmin { get; set; }

        public static UserView FromUser(e_Users user)
        {
            return new UserView
            {
                UserId = user.UserId,
                Level = (UserLevel)user.Level,
                LoginName = user.LoginName,
                Password = user.Password,
                RealName=user.RealName,
                NickName = user.NickName,
                Gender=user.Gender,
                UserEmail=user.UserEmail,
                UserSign=user.UserSign,
                TopicCnt = user.TopicCnt,
                PostCnt = user.PostCnt,
                DelCnt=user.DelCnt,
                LoginCnt = user.LoginCnt,
                Grade = user.Grade,
                AddTime=user.AddTime,
                LastLoginTime = user.LastLoginTime,
                IsAdmin = (UserLevel)user.Level >= UserLevel.Admin
            };
        }
    }
}
