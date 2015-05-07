using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaHCM.QA.Core;
using ChinaHCM.QA.Site.Model;
using ChinaHCM.QA.Component.Tools;

namespace ChinaHCM.QA.Site
{
    /// <summary>
    ///     帖子模块站点业务契约
    /// </summary>
    public interface IBBSSiteContract : IBBSService
    {
        /// <summary>
        /// 根据主贴得到全部帖子信息
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        new IList<BBSView> GetBBSs(int topicId);

        /// <summary>
        /// 根据主贴得到全部帖子信息(分页)
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        new IList<BBSView> GetBBSs(int topicId, int pageIndex, int pageSize, out int recordCount);

        /// <summary>
        /// 得到指定帖子信息
        /// </summary>
        /// <param name="bbsId"></param>
        /// <returns></returns>
        new BBSView GetBBS(int bbsId);

        /// <summary>
        /// 判断是否允许回复QA
        /// </summary>
        /// <param name="topicId">主贴ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        new bool CanReplay(int topicId, int userId);

        /// <summary>
        ///     QA回复操作
        /// </summary>
        /// <param name="model">QA回复模型信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult SaveReply(ReplyMode model);

        /// <summary>
        /// 判断是否允许编辑QA
        /// </summary>
        /// <param name="topicId">主贴ID</param>
        /// <param name="bbsId">帖子ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        new bool CanEditReplay(int topicId, int bbsId, int userId);

        /// <summary>
        ///   QA回复修改操作
        /// </summary>
        /// <param name="model">QA回复模型信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult SaveReplyEdit(ReplyMode model);

        /// <summary>
        ///  更新隐藏标记
        /// </summary>
        /// <param name="bbsId">帖子ID</param>
        /// <returns>业务操作结果</returns>
        new OperationResult SetHidden(int bbsId);

        /// <summary>
        ///  删除帖子
        /// </summary>
        /// <param name="bbsId">帖子ID</param>
        /// <returns>业务操作结果</returns>
        new OperationResult Delete(int bbsId);

        /// <summary>
        ///  设置为正确答案
        /// </summary>
        /// <param name="bbsId">帖子ID</param>
        /// <returns>业务操作结果</returns>
        new OperationResult SetAnswer(int bbsId);

        /// <summary>
        ///  修改为正确答案
        /// </summary>
        /// <param name="bbsId">帖子ID</param>
        /// <param name="userId">当前用户ID</param>
        /// <returns>业务操作结果</returns>
        new OperationResult ChangeAnswer(int bbsId, int userId);

        /// <summary>
        ///  保存修改正确答案
        /// </summary>
        /// <param name="model">正确答案模型信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult EditAnswerSave(ReplyMode model);
    }
}
