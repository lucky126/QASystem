using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaHCM.QA.Core.Model
{
    public class BoardInfo
    {
        /// <summary>
        /// 版区ID
        /// </summary>
        public int BoardId { get; set; }

        /// <summary>
        /// 版区名称
        /// </summary>
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
        /// 版区类型
        /// </summary>
        public BoardType BoardType { get; set; }
    }
}
