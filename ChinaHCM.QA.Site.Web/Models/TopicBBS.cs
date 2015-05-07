using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ChinaHCM.QA.Site.Model;
using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Site.Web.Models
{
    public class TopicBBS
    {
        /// <summary>
        /// 主贴信息
        /// </summary>
        public TopicView Topic { get; set; }

        /// <summary>
        /// 帖子列表
        /// </summary>
        public IEnumerable<BBSView> BBSs { get; set; }

        /// <summary>
        /// 主贴帖子
        /// </summary>
        public BBSView TopicBBSs { get; set; }

        /// <summary>
        /// 是否有正确答案
        /// </summary>
        public bool HasAnswer { get; set; }

        /// <summary>
        /// 正确答案帖子
        /// </summary>
        public BBSView Answer { get; set; }

        /// <summary>
        /// 非主贴非正确答案的回复
        /// </summary>
        public IEnumerable<BBSView> OtherBBSs { get; set; }

        /// <summary>
        /// 是否结贴不许非管理员修改和回复
        /// </summary>
        public bool IsFinish { get; set; }

        /// <summary>
        /// 非主贴非正确答案的回复数(亦可用于讨论帖的回帖数)
        /// </summary>
        public int AnswerCnt { get; set; }

        /// <summary>
        /// 当前用户ID
        /// </summary>
        public int CurrentUserId { get; set; }

        public TopicBBS(TopicView topic, IEnumerable<BBSView> bbss, int currentUseId)
            : this(topic, bbss, currentUseId, false) { }

        public TopicBBS(TopicView topic, IEnumerable<BBSView> bbss, int currentUseId, bool isAdmin)
        {
            Topic = topic;
            BBSs = bbss;
            CurrentUserId = currentUseId;

            if (BBSs.Count(b => b.IsAnswer) == 1 || Topic.IsFinish || Topic.IsLock)
            {
                IsFinish = true;
            }
            else
            {
                IsFinish = false;
            }
            //得到主贴
            TopicBBSs = BBSs.FirstOrDefault(b => b.IsTopic == true);
            //判断是否存在正确答案
            HasAnswer = BBSs.Count(b => b.IsAnswer) == 1;
            //得到正确答案
            Answer = HasAnswer ? BBSs.FirstOrDefault(b => b.IsAnswer == true) : null;
            //得到正确答案个数
            AnswerCnt = BBSs.Count(b => b.IsTopic != true && b.IsAnswer != true);
          
            //非管理员处理逻辑
            if (!isAdmin)
            {
                //判断正确答案是否允许所有人查阅
                if (HasAnswer  && Answer.AnswerMode == Mode.Private && CurrentUserId != topic.PostUserId)
                {
                    Answer.Content = "***仅对发帖人公开***";
                }
                //判断帖子是否被隐藏
                foreach (BBSView b in BBSs)
                {
                    if (b.DisplayMode == Mode.Private && CurrentUserId != b.PostUserId)
                    {
                        b.Content = "***该内容已被管理员设置为隐藏***";
                    }
                }
            }

            //得到其他回答帖子
            OtherBBSs = BBSs.Where(b => b.IsTopic != true && b.IsAnswer != true);
        }
    }
}