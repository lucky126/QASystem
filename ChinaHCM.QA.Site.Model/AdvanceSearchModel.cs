using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Site.Model
{
    public class AdvanceSearchModel : BasicSearchModel
    {
        /// <summary>
        /// 搜索范围
        /// </summary>
        [DisplayName("搜索范围")]
        public int SearchScope { get; set; }

        /// <summary>
        /// 搜索类型
        /// </summary>
        [Required]
        [DisplayName("搜索类型")]
        public SearchType SearchType { get; set; }

        /// <summary>
        /// 发帖时间
        /// </summary>
        [Required]
        [DisplayName("发帖时间")]
        public PostTimeType PostTime { get; set; }
    }
}
