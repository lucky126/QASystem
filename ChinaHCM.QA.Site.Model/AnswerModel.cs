using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Site.Model
{
    public class AnswerModel
    {
        /// <summary>
        /// 版区ID
        /// </summary>
        public int BoardId { get; set; }

        /// <summary>
        /// 主贴ID
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// 发帖人ID
        /// </summary>
        public int PostUserId { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "回复内容")]
        [DataType(DataType.MultilineText)]
        public string ReplyContent { get; set; }

        /// <summary>
        /// 答案的显示方式
        /// </summary>
        public Mode AnswerMode { get; set; }
    }
}
