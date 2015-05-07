using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;

using ChinaHCM.QA.Core;
using ChinaHCM.QA.Site.Model;
using ChinaHCM.QA.Component.Tools;
using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Site
{
    /// <summary>
    ///  主贴模块站点业务实现
    /// </summary>
    [Export(typeof(ITopicSiteContract))]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    internal class TopicSiteService : TopicService, ITopicSiteContract
    {
         /// <summary>
        /// 根据版区获得主贴置顶信息
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public new IList<TopicView> GetTopTopics(int boardId)
        {
            PublicHelper.CheckArgument(boardId, "boardId");

            return base.GetTopTopics(boardId).Select(t => TopicView.FromTopic(t)).ToList<TopicView>();
        }

        /// <summary>
        /// 根据版区获得主贴信息
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public new IList<TopicView> GetTopics(int boardId, int pageIndex, int pageSize, out int recordCount)
        {
            PublicHelper.CheckArgument(boardId, "boardId");
            PublicHelper.CheckArgument(pageIndex, "pageIndex");
            PublicHelper.CheckArgument(pageSize, "pageSize");

            return base.GetTopics(boardId, pageIndex, pageSize, out recordCount).Select(t => TopicView.FromTopic(t)).ToList<TopicView>();
        }

        /// <summary>
        /// 根据版区和帖子分类类型获得主贴信息
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="qaType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public new IList<TopicView> GetTopicsByType(int boardId, QAType qaType, int pageIndex, int pageSize, out int recordCount)
        {
            PublicHelper.CheckArgument(boardId, "boardId");
            PublicHelper.CheckArgument(qaType, "qaType");
            PublicHelper.CheckArgument(pageIndex, "pageIndex");
            PublicHelper.CheckArgument(pageSize, "pageSize");

            return base.GetTopicsByType(boardId, qaType, pageIndex, pageSize, out recordCount).Select(t => TopicView.FromTopic(t)).ToList<TopicView>();
        }

        /// <summary>
        /// 得到指定帖子信息
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public new TopicView GetTopic(int topicId)
        {
            PublicHelper.CheckArgument(topicId, "topicId");

            if (base.GetTopic(topicId) == null)
                return null;

            return TopicView.FromTopic(base.GetTopic(topicId));
        }

        /// <summary>
        ///  发帖操作
        /// </summary>
        /// <param name="model">发帖模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult SaveTopic(NewTopicModel model)
        {
            PublicHelper.CheckArgument(model, "model");

            NewTopicInfo topicInfo = new NewTopicInfo
            {
                BoardId = model.BoardId,
                PostUserId = model.PostUserId,
                PostIp = HttpContext.Current.Request.UserHostAddress,
                Title = model.Title,
                Content = model.Content,
                Keywords = model.Keywords
            };

            OperationResult result = base.SaveTopic(topicInfo);

            if (result.ResultType == OperationResultType.Success)
            {
                result.AppendData = null;
            }

            return result;
        }

        /// <summary>
        ///  搜索操作
        /// </summary>
        /// <param name="model">搜索模型信息</param>
        /// <param name="model"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns>业务操作结果</returns>
        public IEnumerable<TopicView> Search(AdvanceSearchModel model, int pageIndex, int pageSize, out int recordCount)
        {
            PublicHelper.CheckArgument(model, "model");
            PublicHelper.CheckArgument(pageIndex, "pageIndex");
            PublicHelper.CheckArgument(pageSize, "pageSize");

            SearchInfo searchInfo = new SearchInfo
            {
                SearchContext = model.SearchContext,
                SearchScope = model.SearchScope,
                SearchType = model.SearchType,
                PostTime = model.PostTime
            };

            return base.Search(searchInfo, pageIndex, pageSize, out recordCount).Select(t => TopicView.FromTopic(t));
        }

        /// <summary>
        ///  更新帖子点击次数
        /// </summary>
        /// <param name="TopicId">主贴ID</param>
        /// <returns>业务操作结果</returns>
        public new OperationResult SaveHits(int TopicId)
        {
            PublicHelper.CheckArgument(TopicId, "TopicId");

            OperationResult result = base.SaveHits(TopicId);

            if (result.ResultType == OperationResultType.Success)
            {
                result.AppendData = null;
            }

            return result;
        }

        /// <summary>
        ///  更新精华标记
        /// </summary>
        /// <param name="TopicId">主贴ID</param>
        /// <returns>业务操作结果</returns>
        public new OperationResult SetDegist(int TopicId)
        {
            PublicHelper.CheckArgument(TopicId, "TopicId");

            OperationResult result = base.SetDegist(TopicId);

            if (result.ResultType == OperationResultType.Success)
            {
                result.AppendData = null;
            }

            return result;
        }

        /// <summary>
        ///  更新锁定标记
        /// </summary>
        /// <param name="TopicId">主贴ID</param>
        /// <returns>业务操作结果</returns>
        public new OperationResult SetLock(int TopicId)
        {
            PublicHelper.CheckArgument(TopicId, "TopicId");

            OperationResult result = base.SetLock(TopicId);

            if (result.ResultType == OperationResultType.Success)
            {
                result.AppendData = null;
            }

            return result;
        }

        /// <summary>
        ///  更新置顶等级
        /// </summary>
        /// <param name="TopicId">主贴ID</param>
        /// <param name="topLevel">置顶等级</param>
        /// <returns>业务操作结果</returns>
        public new OperationResult SetTop(int TopicId, TopLevel topLevel)
        {
            PublicHelper.CheckArgument(TopicId, "TopicId");
            PublicHelper.CheckArgument(topLevel, "topLevel");

            OperationResult result = base.SetTop(TopicId, topLevel);

            if (result.ResultType == OperationResultType.Success)
            {
                result.AppendData = null;
            }

            return result;
        }

        /// <summary>
        ///  删除帖子
        /// </summary>
        /// <param name="topicIds">主贴ID串</param>
        /// <returns>业务操作结果</returns>
        public new OperationResult Delete(IEnumerable<int> topicIds)
        {
            PublicHelper.CheckArgument(topicIds, "topicIds");

            OperationResult result = base.Delete(topicIds);

            if (result.ResultType == OperationResultType.Success)
            {
                result.AppendData = null;
            }

            return result;
        }

        /// <summary>
        ///  录入正确答案
        /// </summary>
        /// <param name="model">正确答案模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult SaveAnswer(AnswerModel model)
        {
            PublicHelper.CheckArgument(model, "model");

            AnswerInfo info = new AnswerInfo
            {
                BoardId = model.BoardId,
                PostUserId = model.PostUserId,
                PostIp = HttpContext.Current.Request.UserHostAddress,
                TopicId=model.TopicId,
                ReplyContent=model.ReplyContent,
                AnswerMode = model.AnswerMode
            };

            OperationResult result = base.SaveAnswer(info);

            if (result.ResultType == OperationResultType.Success)
            {
                result.AppendData = null;
            }

            return result;
        }
    }
}
