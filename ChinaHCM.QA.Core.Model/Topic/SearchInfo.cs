using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaHCM.QA.Core.Model
{
    public class SearchInfo
    {
        /// <summary>
        /// 搜索标题、关键字
        /// </summary>
        public string SearchContext { get; set; }

        /// <summary>
        /// 搜索范围
        /// </summary>
        public int SearchScope { get; set; }

        /// <summary>
        /// 搜索类型
        /// </summary>
        public SearchType SearchType { get; set; }

        /// <summary>
        /// 发帖时间
        /// </summary>
        public PostTimeType PostTime { get; set; }
    }
}
