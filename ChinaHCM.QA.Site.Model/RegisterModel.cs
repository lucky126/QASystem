using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Site.Model
{
    public class BasicUserModel
    { 
        /// <summary>
        /// 获取或设置 登录密码
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "{0}至少{2}个字符，且不得超过{1}个字符")]
        public string Password { get; set; } 

        /// <summary>
        /// 获取或设置 真实姓名
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "真实姓名")]
        [StringLength(50, ErrorMessage = "{0}长度不得超过{1}个字符")]
        public string RealName { get; set; }

        /// <summary>
        /// 获取或设置 性别
        /// </summary>
        [Display(Name = "性别")]
        public Gender Gender { get; set; }

        /// <summary>
        /// 获取或设置 电子邮箱
        /// </summary>
        [Display(Name = "电子邮箱")]
        [StringLength(255, ErrorMessage = "{0}长度不得超过{1}个字符")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9]+\.[A-Za-z]{2,4}", ErrorMessage = "请输入合法的{0}")]
        public string UserEmail { get; set; }
    }

    public class RegisterModel : BasicUserModel
    {
        /// <summary>
        /// 获取或设置 登录账号
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "用户名")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "{0}至少{2}个字符，且不得超过{1}个字符")]
        [Remote("CheckUserName", "Account", ErrorMessage = "{0}已经存在")]
        public string LoginName { get; set; }

        /// <summary>
        /// 获取或设置 登录密码
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "密码确认")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "{0}至少{2}个字符，且不得超过{1}个字符")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "密码必须一致")]
        public string RePassword { get; set; }

        /// <summary>
        /// 获取或设置 用户昵称
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "昵称")]
        [StringLength(50, ErrorMessage = "{0}长度不得超过{1}个字符")]
        [Remote("CheckNickName", "Account", ErrorMessage = "{0}已经存在")]
        public string NickName { get; set; }

        /// <summary>
        /// 获取或设置 密码保护问题
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "密码保护问题")]
        [StringLength(50, ErrorMessage = "{0}长度不得超过{1}个字符")]
        public string UserQuesion { get; set; }

        /// <summary>
        /// 获取或设置 密码保护问题答案
        /// </summary>
        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "密码保护问题答案")]
        [StringLength(50, ErrorMessage = "{0}长度不得超过{1}个字符")]
        public string UserAnswer { get; set; }

        /// <summary>
        /// 获取或设置 登录成功后返回地址
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }
    }
}
