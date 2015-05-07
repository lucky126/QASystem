using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaHCM.QA.Component.Data;
using ChinaHCM.QA.Core.Model;
using ChinaHCM.QA.Component.Tools;

namespace ChinaHCM.QA.Core
{
    /// <summary>
    ///     主贴模块核心业务契约
    /// </summary>
    public interface ITopicService
    {
        #region 属性

        /// <summary>
        /// 获取 主贴信息查询数据集
        /// </summary>
        IQueryable<e_Topic> Topics { get; }

        #endregion

         /// <summary>
        /// 根据版区获得主贴置顶信息
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        IEnumerable<e_Topic> GetTopTopics(int boardId);

        /// <summary>
        /// 根据版区获得主贴信息
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        IEnumerable<e_Topic> GetTopics(int boardId, int pageIndex, int pageSize, out int recordCount);

        /// <summary>
        /// 根据版区和帖子分类类型获得主贴信息
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="qaType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        IEnumerable<e_Topic> GetTopicsByType(int boardId, QAType qaType, int pageIndex, int pageSize, out int recordCount);

        /// <summary>
        /// 得到指定帖子信息
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        e_Topic GetTopic(int topicId);

        /// <summary>
        ///  发帖操作
        /// </summary>
        /// <param name="model">发帖模型信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult SaveTopic(NewTopicInfo model);

        /// <summary>
        ///  搜索操作
        /// </summary>
        /// <param name="model">搜索模型信息</param>
        /// <param name="model"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns>业务操作结果</returns>
        IEnumerable<e_Topic> Search(SearchInfo model, int pageIndex, int pageSize, out int recordCount);

        /// <summary>
        ///  更新帖子点击次数
        /// </summary>
        /// <param name="TopicId">主贴ID</param>
        /// <returns>业务操作结果</returns>
        OperationResult SaveHits(int TopicId);

        /// <summary>
        ///  更新精华标记
        /// </summary>
        /// <param name="TopicId">主贴ID</param>
        /// <returns>业务操作结果</returns>
        OperationResult SetDegist(int TopicId);

        /// <summary>
        ///  更新锁定标记
        /// </summary>
        /// <param name="TopicId">主贴ID</param>
        /// <returns>业务操作结果</returns>
        OperationResult SetLock(int TopicId);

        /// <summary>
        ///  更新置顶等级
        /// </summary>
        /// <param name="TopicId">主贴ID</param>
        /// <param name="topLevel">置顶等级</param>
        /// <returns>业务操作结果</returns>
        OperationResult SetTop(int TopicId, TopLevel topLevel);

        /// <summary>
        ///  删除帖子
        /// </summary>
        /// <param name="topicIds">主贴ID串</param>
        /// <returns>业务操作结果</returns>
        OperationResult Delete(IEnumerable<int> topicIds);

        /// <summary>
        ///  录入正确答案
        /// </summary>
        /// <param name="model">正确答案模型信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult SaveAnswer(AnswerInfo model);
    }
}
