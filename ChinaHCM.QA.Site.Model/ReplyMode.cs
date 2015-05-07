using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ChinaHCM.QA.Site.Model
{
    public class ReplyMode
    {
        /// <summary>
        /// 回复用显示标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 提交按钮文字
        /// </summary>
        public string SubmitButtonValue { get; set; }

        /// <summary>
        /// 是否显示关闭按钮
        /// </summary>
        public bool IsShowCloseButton { get; set; }

        /// <summary>
        /// 关闭按钮调用JS脚本
        /// </summary>
        public string CloseJSFunction { get; set; }

        /// <summary>
        /// 表单提交的Action名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 版区ID
        /// </summary>
        public int BoardId { get; set; }

        /// <summary>
        /// 主贴ID
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// 帖子ID
        /// </summary>
        public int BBSId { get; set; }

        /// <summary>
        /// 发帖人ID
        /// </summary>
        public int PostUserId { get; set; }

        [Required(ErrorMessage="请输入{0}")]
        [Display(Name="回复内容")]
        [DataType(DataType.MultilineText)]
        [StringLength(500,ErrorMessage="{0}长度不得超过{1}字符")]
        public string ReplyContent { get; set; }
    }
}
