using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaHCM.QA.Core.Model
{
    /// <summary>
    /// 注册信息类
    /// </summary>
    public class UserInfo : LoginInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户等级
        /// </summary>
        public UserLevel Level { get; set; }

        /// <summary>
        ///   获取或设置 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 获取或设置 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 获取或设置 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 获取或设置 电子邮箱
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// 用户签名
        /// </summary>
        public string UserSign { get; set; }

        /// <summary>
        /// 获取或设置 密码保护问题
        /// </summary>
        public string UserQuesion { get; set; }

        /// <summary>
        /// 获取或设置 密码保护问题答案
        /// </summary>
        public string UserAnswer { get; set; }
    }
}
