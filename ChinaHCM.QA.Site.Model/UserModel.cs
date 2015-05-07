using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ChinaHCM.QA.Core.Model;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ChinaHCM.QA.Site.Model
{
    public class UserModel : BasicUserModel
    {
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置 登录账号
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "用户名")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "{0}至少{2}个字符，且不得超过{1}个字符")]
        [Remote("CheckUserName", "Account", AdditionalFields = "UserId", ErrorMessage = "{0}已经存在")]
        public string LoginName { get; set; }

        /// <summary>
        /// 获取或设置 用户昵称
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "昵称")]
        [StringLength(50, ErrorMessage = "{0}长度不得超过{1}个字符")]
        [Remote("CheckNickName", "Account", AdditionalFields = "UserId", ErrorMessage = "{0}已经存在")]
        public string NickName { get; set; }

        [Required(ErrorMessage = "请选择{0}")]
        [DisplayName("用户类型")]
        public UserLevel Level { get; set; }     

        [DisplayName("签名")]        
        public string UserSign { get; set; }    

        /// <summary>
        /// 是否新增用户
        /// </summary>
        public bool IsNewUser { get; set; }
    }
}
