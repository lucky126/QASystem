using System;
using System.Collections.Generic;
using System.Linq;

using ChinaHCM.QA.Site.Model;

namespace ChinaHCM.QA.Site.Web.Models
{
    public class SearchResult
    {
        /// <summary>
        /// 结果列表
        /// </summary>
        public IEnumerable<TopicView> topics { get; set; }

        /// <summary>
        /// 搜索条件
        /// </summary>
        public AdvanceSearchModel Search { get; set; }
    }
}