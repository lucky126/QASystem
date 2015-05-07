using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaHCM.QA.Core.Model
{
    public class AnswerInfo
    {
        /// <summary>
        /// 主贴ID
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// 版区ID
        /// </summary>
        public int BoardId { get; set; }

        /// <summary>
        /// 发帖人ID
        /// </summary>
        public int PostUserId { get; set; }

        /// <summary>
        /// 发帖人IP
        /// </summary>
        public string PostIp { get; set; }

        /// <summary>
        /// 发帖内容
        /// </summary>
        public string ReplyContent { get; set; }

        /// <summary>
        /// 答案的显示方式
        /// </summary>
        public Mode AnswerMode { get; set; }
    }
}
