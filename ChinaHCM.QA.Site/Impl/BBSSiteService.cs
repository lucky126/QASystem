using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;

using ChinaHCM.QA.Core;
using ChinaHCM.QA.Site.Model;
using ChinaHCM.QA.Component.Tools;
using ChinaHCM.QA.Core.Model.BBS;

namespace ChinaHCM.QA.Site
{
    /// <summary>
    ///  帖子模块站点业务实现
    /// </summary>
    [Export(typeof(IBBSSiteContract))]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    internal class BBSSiteService : BBSService, IBBSSiteContract
    {
        /// <summary>
        /// 根据主贴得到全部帖子信息
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public new IList<BBSView> GetBBSs(int topicId)
        {            
            PublicHelper.CheckArgument(topicId, "topicId");

            return base.GetBBSs(topicId).Select(b => BBSView.FromBBS(b)).ToList<BBSView>();
        }

        /// <summary>
        /// 根据主贴得到全部帖子信息(分页)
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public new IList<BBSView> GetBBSs(int topicId, int pageIndex, int pageSize, out int recordCount)
        {
            PublicHelper.CheckArgument(topicId, "topicId");
            PublicHelper.CheckArgument(pageIndex, "pageIndex");
            PublicHelper.CheckArgument(pageSize, "pageSize");

            return base.GetBBSs(topicId, pageIndex, pageSize, out recordCount).Select(b => BBSView.FromBBS(b)).ToList<BBSView>();
        }

        /// <summary>
        /// 得到指定帖子信息
        /// </summary>
        /// <param name="bbsId"></param>
        /// <returns></returns>
        public new BBSView GetBBS(int bbsId)
        {
            PublicHelper.CheckArgument(bbsId, "bbsId");

            return BBSView.FromBBS(base.GetBBS(bbsId));
        }

        /// <summary>
        /// 判断是否允许回复QA
        /// </summary>
        /// <param name="topicId">主贴ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public new bool CanReplay(int topicId, int userId)
        {
            PublicHelper.CheckArgument(topicId, "topicId");
            PublicHelper.CheckArgument(userId, "userId");

            return base.CanReplay(topicId, userId);
        }

        /// <summary>
        ///    QA回复操作
        /// </summary>
        /// <param name="model">QA回复模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult SaveReply(ReplyMode model)
        {
            PublicHelper.CheckArgument(model, "model");

            ReplyInfo replyInfo = new ReplyInfo
            {
                BBSId=model.BBSId,
                TopicId = model.TopicId,
                BoardId=model.BoardId,
                PostUserId = model.PostUserId,
                PostIp = HttpContext.Current.Request.UserHostAddress,
                ReplyContent = model.ReplyContent
            };

            OperationResult result = base.SaveReply(replyInfo);

            if (result.ResultType == OperationResultType.Success)
            {
                result.AppendData = null;
            }

            return result;
        }

        /// <summary>
        /// 判断是否允许编辑QA
        /// </summary>
        /// <param name="topicId">主贴ID</param>
        /// <param name="bbsId">帖子ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public new bool CanEditReplay(int topicId, int bbsId, int userId)
        {
            PublicHelper.CheckArgument(topicId, "topicId");
            PublicHelper.CheckArgument(topicId, "bbsId");
            PublicHelper.CheckArgument(userId, "userId");

            return base.CanEditReplay(topicId, bbsId, userId);
        }

        /// <summary>
        ///    QA回复修改操作
        /// </summary>
        /// <param name="model">QA回复模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult SaveReplyEdit(ReplyMode model)
        {
            PublicHelper.CheckArgument(model, "model");

            ReplyInfo replyInfo = new ReplyInfo
            {
                BBSId=model.BBSId,
                TopicId = model.TopicId,
                BoardId = model.BoardId,
                PostUserId = model.PostUserId,
                PostIp = HttpContext.Current.Request.UserHostAddress,
                ReplyContent = model.ReplyContent
            };

            OperationResult result = base.SaveReplyEdit(replyInfo);

            if (result.ResultType == OperationResultType.Success)
            {
                result.AppendData = null;
            }

            return result;
        }

        /// <summary>
        ///  更新隐藏标记
        /// </summary>
        /// <param name="bbsId">帖子ID</param>
        /// <returns>业务操作结果</returns>
        public new OperationResult SetHidden(int bbsId)
        {
            PublicHelper.CheckArgument(bbsId, "bbsId");

            OperationResult result = base.SetHidden(bbsId);

            if (result.ResultType == OperationResultType.Success)
            {
                result.AppendData = null;
            }

            return result;
        }

        /// <summary>
        ///  删除帖子
        /// </summary>
        /// <param name="bbsId">帖子ID</param>
        /// <returns>业务操作结果</returns>
        public new OperationResult Delete(int bbsId)
        {
            PublicHelper.CheckArgument(bbsId, "bbsId");

            OperationResult result = base.Delete(bbsId);

            if (result.ResultType == OperationResultType.Success)
            {
                result.AppendData = null;
            }

            return result;
        }

        /// <summary>
        ///  设置为正确答案
        /// </summary>
        /// <param name="bbsId">帖子ID</param>
        /// <returns>业务操作结果</returns>
        public new OperationResult SetAnswer(int bbsId)
        {
            PublicHelper.CheckArgument(bbsId, "bbsId");

            OperationResult result = base.SetAnswer(bbsId);

            if (result.ResultType == OperationResultType.Success)
            {
                result.AppendData = null;
            }

            return result;
        }

        /// <summary>
        ///  修改为正确答案
        /// </summary>
        /// <param name="bbsId">帖子ID</param>
        /// <param name="userId">当前用户ID</param>
        /// <returns>业务操作结果</returns>
        public new OperationResult ChangeAnswer(int bbsId, int userId)
        {
            PublicHelper.CheckArgument(bbsId, "bbsId");

            OperationResult result = base.ChangeAnswer(bbsId, userId);

            if (result.ResultType == OperationResultType.Success)
            {
                result.AppendData = null;
            }

            return result;
        }

        /// <summary>
        ///  保存修改正确答案
        /// </summary>
        /// <param name="model">正确答案模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult EditAnswerSave(ReplyMode model)
        {
            PublicHelper.CheckArgument(model, "model");

            ReplyInfo replyInfo = new ReplyInfo
            {
                BBSId = model.BBSId,
                TopicId = model.TopicId,
                BoardId = model.BoardId,
                PostUserId = model.PostUserId,
                PostIp = HttpContext.Current.Request.UserHostAddress,
                ReplyContent = model.ReplyContent
            };

            OperationResult result = base.SaveReplyEdit(replyInfo);

            if (result.ResultType == OperationResultType.Success)
            {
                result.AppendData = null;
            }

            return result;
        }
    }
}
