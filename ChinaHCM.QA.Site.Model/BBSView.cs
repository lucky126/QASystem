using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

using ChinaHCM.QA.Component.Data;
using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Site.Model
{
    public class BBSView
    {
        public int BoardId { get; private set; }

        public int TopicId { get; private set; }

        public int BBSId { get; private set; }

        [DisplayName("内容")]
        public string Content { get;  set; }

        public int PostUserId { get; private set; }

        [DisplayName("发帖人")]
        public string PostUserName { get; private set; }

        [DisplayName("发帖时间")]
        public DateTime PostTime { get; private set; }

        [DisplayName("发帖IP")]
        public string PostIp { get; private set; }

        public bool IsTopic { get; private set; }

        public bool IsAnswer { get; private set; }

        public Mode DisplayMode { get; private set; }

        public Mode AnswerMode { get; private set; }

        public static BBSView FromBBS(e_BBS bbs)
        {
            return new BBSView
            {
                BoardId=bbs.BoardId,
                TopicId = bbs.TopicId,
                BBSId = bbs.BBSId,
                Content = bbs.Content,
                PostUserId = bbs.PostUserId,
                PostUserName = bbs.PostUserName,
                PostTime = bbs.PostTime,
                PostIp = bbs.PostIp,
                IsTopic = bbs.IsTopic == (int)YesOrNo.Yes,
                IsAnswer = bbs.IsAnswer == (int)YesOrNo.Yes,
                DisplayMode = (Mode)bbs.DisplayMode,
                AnswerMode = (Mode)bbs.AnswerMode
            };
        }
    }
}