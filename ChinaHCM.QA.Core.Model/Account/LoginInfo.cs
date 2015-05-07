using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaHCM.QA.Core.Model
{
    /// <summary>
    ///  登录信息类
    /// </summary>
    public class LoginInfo
    {
        /// <summary>
        ///   获取或设置 登录账号
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        ///   获取或设置 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///  获取或设置 IP地址
        /// </summary>
        public string IpAddress { get; set; }
    }
}
