using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ChinaHCM.QA.Site.Model
{
    public class ChangePasswordModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置 登录密码
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "旧密码")]
        [DataType(DataType.Password)]
        [Remote("CheckPassword", "Account", AdditionalFields = "UserId", ErrorMessage = "{0}输入错误")]
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置 登录密码
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "新密码")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "{0}至少{2}个字符，且不得超过{1}个字符")]
        public string NewPassword { get; set; }

        /// <summary>
        /// 获取或设置 登录密码
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "密码确认")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "{0}至少{2}个字符，且不得超过{1}个字符")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "密码必须一致")]
        public string RePassword { get; set; }
    }
}
