using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

using ChinaHCM.QA.Component.Data;
using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Site.Model
{
    public class TopicView
    {
        /// <summary>
        /// 版区ID
        /// </summary>
        public int BoardId { get; private set; }

        /// <summary>
        /// 帖子ID
        /// </summary>
        public int TopicId { get; private set; }

        [DisplayName("标题")]
        public string Title { get; private set; }

        /// <summary>
        /// 关键词
        /// </summary>
        public IEnumerable<string> Keywords { get; private set; }

        /// <summary>
        /// 帖子类型
        /// </summary>
        public BoardType TopicType { get; private set; }

        /// <summary>
        /// 发帖人ID
        /// </summary>
        public int PostUserId { get; private set; }

        /// <summary>
        /// 发帖人昵称
        /// </summary>
        [DisplayName("发帖人")]
        public string PostUserName { get; private set; }

        [DisplayName("发帖时间")]
        public DateTime PostTime { get; private set; }

        /// <summary>
        /// 最后发帖人ID
        /// </summary>
        public Nullable<int> LastPostUserId { get; set; }

        /// <summary>
        /// 最后发帖人昵称
        /// </summary>
        public string LastPostUserName { get; set; }

        /// <summary>
        /// 最后发贴时间
        /// </summary>
        public Nullable<System.DateTime> LastPostTime { get; set; }

        [DisplayName("点击数")]
        public int Hits { get; private set; }

        [DisplayName("回复数")]
        public int Child { get; private set; }

        /// <summary>
        /// 是否结贴
        /// </summary>
        public bool IsFinish { get; private set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLock { get; private set; }

        /// <summary>
        /// 置顶等级
        /// </summary>
        public int TopLevel { get; private set; }

        /// <summary>
        /// 是否精华
        /// </summary>
        public bool IsDigest { get; private set; }

        public static TopicView FromTopic(e_Topic topic)
        {
            return new TopicView
            {
                BoardId=topic.BoardId,
                TopicId = topic.TopicId,
                TopicType = (BoardType)topic.TopicType,
                Title = topic.Title,
                Keywords=topic.Keywords.Split(new char[]{','}).ToList<string>(),
                PostUserId = topic.PostUserId,
                PostUserName =topic.PostUserName,
                PostTime = topic.PostTime,
                LastPostUserId=topic.LastPostUserId,
                LastPostTime=topic.LastPostTime,
                LastPostUserName=topic.LastPostUserName,
                Hits = topic.Hits,
                Child=topic.Child,
                IsFinish = topic.IsFinish == (int)YesOrNo.Yes,
                IsLock = topic.IsLock == (int)YesOrNo.Yes,
                TopLevel = topic.TopLevel,
                IsDigest = topic.IsDigest == (int)YesOrNo.Yes,
            };
        }
    }
}