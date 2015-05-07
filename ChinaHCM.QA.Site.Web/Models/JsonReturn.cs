using System;
using System.Collections.Generic;
using System.Linq;

namespace ChinaHCM.QA.Site.Web.Models
{
    public class JsonReturn
    {
        /// <summary>
        /// 成功与否标记，：0，成功
        /// </summary>
        public int Flag { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// 返回URL
        /// </summary>
        public string ReturlUrl { get; set; }
    }
}