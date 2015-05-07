using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChinaHCM.QA.Site.Model
{
    public class BasicSearchModel
    {
        /// <summary>
        /// 搜索标题、关键字
        /// </summary>
        [Required(ErrorMessage="请输入{0}")]
        [DisplayName("搜索内容")]
        public string SearchContext { get; set; }

        /// <summary>
        /// 搜索的版区
        /// </summary>
        public int BoardId { get; set; }
    }
}
