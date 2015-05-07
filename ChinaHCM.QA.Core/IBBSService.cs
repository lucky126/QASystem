using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ChinaHCM.QA.Component.Data;
using ChinaHCM.QA.Core.Model;
using ChinaHCM.QA.Core.Model.BBS;
using ChinaHCM.QA.Component.Tools;

namespace ChinaHCM.QA.Core
{
    /// <summary>
    ///     帖子户模块核心业务契约
    /// </summary>
    public interface IBBSService
    {
        #region 属性

        /// <summary>
        /// 获取 帖子信息查询数据集
        /// </summary>
        IQueryable<e_BBS> BBSs { get; }

        #endregion

        /// <summary>
        /// 根据主贴得到全部帖子信息
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        IEnumerable<e_BBS> GetBBSs(int topicId);

        /// <summary>
        /// 根据主贴得到全部帖子信息(分页)
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        IEnumerable<e_BBS> GetBBSs(int topicId, int pageIndex, int pageSize, out int recordCount);

        /// <summary>
        /// 得到指定帖子信息
        /// </summary>
        /// <param name="bbsId"></param>
        /// <returns></returns>
        e_BBS GetBBS(int bbsId);

        /// <summary>
        /// 判断是否允许回复QA
        /// </summary>
        /// <param name="topicId">主贴ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        bool CanReplay(int topicId, int userId);

        /// <summary>
        ///  回帖
        /// </summary>
        /// <param name="replyInfo">回帖信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult SaveReply(ReplyInfo replyInfo);

        /// <summary>
        /// 判断是否允许编辑QA
        /// </summary>
        /// <param name="topicId">主贴ID</param>
        /// <param name="bbsId">帖子ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        bool CanEditReplay(int topicId, int bbsId, int userId);

        /// <summary>
        ///  回帖修改
        /// </summary>
        /// <param name="replyInfo">回帖信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult SaveReplyEdit(ReplyInfo replyInfo);

        /// <summary>
        ///  更新隐藏标记
        /// </summary>
        /// <param name="bbsId">帖子ID</param>
        /// <returns>业务操作结果</returns>
        OperationResult SetHidden(int bbsId);

        /// <summary>
        ///  删除帖子
        /// </summary>
        /// <param name="bbsId">帖子ID</param>
        /// <returns>业务操作结果</returns>
        OperationResult Delete(int bbsId);

        /// <summary>
        ///  设置为正确答案
        /// </summary>
        /// <param name="bbsId">帖子ID</param>
        /// <returns>业务操作结果</returns>
        OperationResult SetAnswer(int bbsId);

        /// <summary>
        ///  修改为正确答案
        /// </summary>
        /// <param name="bbsId">帖子ID</param>
        /// <param name="userId">当前用户ID</param>
        /// <returns>业务操作结果</returns>
        OperationResult ChangeAnswer(int bbsId, int userId);
    }
}
