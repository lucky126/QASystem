using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.Mvc;

using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Site.Model
{
    public class BoardModel
    {
        /// <summary>
        /// 版区ID
        /// </summary>
        public int BoardId { get; set; }

        /// <summary>
        /// 版区名称
        /// </summary>
        [DisplayName("版区名称")]
        public string Name { get; set; }

        /// <summary>
        /// 父版区ID
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// 是否公共版区
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// 管理员列表字符串
        /// </summary>   
        public string MasterList { get; set; }

        /// <summary>
        /// 管理员列表
        /// </summary>   
        public IEnumerable<int> Master { get; set; }

        /// <summary>
        /// 版区类型
        /// </summary>
        public BoardType BoardType { get; set; }
    }
}
