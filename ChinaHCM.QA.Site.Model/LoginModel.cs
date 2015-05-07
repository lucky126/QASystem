using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ChinaHCM.QA.Site.Model
{
    /// <summary>
    /// 用户登录模型
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 获取或设置 登录账号
        /// </summary>
        [Required(ErrorMessage="请输入{0}")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置 登录密码
        /// </summary>
        [Required(ErrorMessage="请输入{0}")]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置 是否记住登录
        /// </summary>
        [Display(Name = "记住登录")]
        public bool IsRememberLogin { get; set; }

        /// <summary>
        /// 获取或设置 登录成功后返回地址
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }
    }
}