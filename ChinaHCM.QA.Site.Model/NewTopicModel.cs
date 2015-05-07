using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ChinaHCM.QA.Site.Model
{
    public class NewTopicModel
    {
        public int BoardId { get; set; }

        public int PostUserId { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "您的提问")]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "{0}长度不得超过{1}字符")]
        public string Title { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "补充说明")]
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "{0}长度不得超过{1}字符")]
        public string Content { get; set; }

        [Display(Name = "关键词")]
        [DataType(DataType.Text)]
        [StringLength(256, ErrorMessage = "{0}长度不得超过{1}字符")]
        public string Keywords { get; set; }
    }
}
