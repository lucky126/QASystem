using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;
using System.Data.Objects.SqlClient;

using ChinaHCM.QA.Core.Data;
using ChinaHCM.QA.Component.Data;
using ChinaHCM.QA.Core.Model;
using ChinaHCM.QA.Component.Tools;
using System.ComponentModel;
using System.Data.Objects;

namespace ChinaHCM.QA.Core
{
    /// <summary>
    ///     主贴模块核心业务实现
    /// </summary>
    public class TopicService : CoreServiceBase, ITopicService
    {
        #region 属性

        #region 受保护的属性

        /// <summary>
        /// 获取或设置 主贴信息数据访问对象
        /// </summary>
        [Import]
        protected ITopicRepository TopicRepository { get; private set; }

        /// <summary>
        /// 获取或设置 帖子信息数据访问对象
        /// </summary>
        [Import]
        protected IBBSRepository BBSRepository { get; private set; }

        /// <summary>
        /// 获取或设置 版区信息核心业务访问对象
        /// </summary>
        [Import]
        protected IBoardRepository BoardRepository { get; private set; }

        /// <summary>
        /// 获取或设置 用户信息核心业务访问对象
        /// </summary>
        [Import]
        protected IUserRepository UserRepository { get; private set; }

        #endregion

        #region 公共属性

        /// <summary>
        /// 获取 主贴信息查询数据集
        /// </summary>
        public IQueryable<e_Topic> Topics
        {
            get { return TopicRepository.Entities; }
        }

        /// <summary>
        /// 获取 用户信息查询数据集
        /// </summary>
        public IQueryable<e_Users> Users
        {
            get { return UserRepository.Entities; }
        }

        /// <summary>
        /// 获取 版区信息查询数据集
        /// </summary>
        public IQueryable<e_Board> Boards
        {
            get { return BoardRepository.Entities; }
        }

        /// <summary>
        /// 获取 帖子信息查询数据集
        /// </summary>
        public IQueryable<e_BBS> BBSs
        {
            get { return BBSRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        /// 根据版区获得主贴置顶信息
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public virtual IEnumerable<e_Topic> GetTopTopics(int boardId)
        {
            return Topics.Where(t => t.BoardId == boardId && t.Status == (int)Status.Normal && t.TopLevel > 0).OrderByDescending(t => t.TopLevel);
        }

        /// <summary>
        /// 根据版区获得主贴信息
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public virtual IEnumerable<e_Topic> GetTopics(int boardId, int pageIndex, int pageSize, out int recordCount)
        {
            return Topics.Where(t => t.BoardId == boardId && t.Status == (int)Status.Normal,
                                            pageIndex, pageSize, out recordCount,
                                            t => t.PostTime, false).OrderByDescending(t => t.TopicType);
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
        public virtual IEnumerable<e_Topic> GetTopicsByType(int boardId, QAType qaType, int pageIndex, int pageSize, out int recordCount)
        {
            PropertySortCondition[] sortConditions = new[] {
                new PropertySortCondition("TopLevel",ListSortDirection.Descending),
                new PropertySortCondition("PostTime",ListSortDirection.Descending)
            };

            switch (qaType)
            {
                case QAType.Digest:
                    return Topics.Where(t => t.BoardId == boardId && t.Status == (int)Status.Normal && t.IsDigest == (int)YesOrNo.Yes,
                                         pageIndex, pageSize, out recordCount,
                                         sortConditions);
                case QAType.Unsolved:
                    return Topics.Where(t => t.BoardId == boardId && t.Status == (int)Status.Normal && t.IsFinish == (int)YesOrNo.No,
                                         pageIndex, pageSize, out recordCount,
                                         sortConditions);
                case QAType.Solved:
                    return Topics.Where(t => t.BoardId == boardId && t.Status == (int)Status.Normal && t.IsFinish == (int)YesOrNo.Yes,
                                         pageIndex, pageSize, out recordCount,
                                         sortConditions);
                case QAType.Zero:
                    return Topics.Where(t => t.BoardId == boardId && t.Status == (int)Status.Normal && t.Child == 0,
                                         pageIndex, pageSize, out recordCount,
                                         sortConditions);
                default:
                    recordCount = 0;
                    return null;
            }
        }

        /// <summary>
        /// 得到指定帖子信息
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public virtual e_Topic GetTopic(int topicId)
        {
            if (TopicRepository.Entities.Count(t => t.TopicId == topicId && t.Status == (int)Status.Normal) == 0)
                return null;

            return TopicRepository.GetByKey(topicId);
        }

        /// <summary>
        ///  发帖操作
        /// </summary>
        /// <param name="model">发帖模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult SaveTopic(NewTopicInfo model)
        {
            if (Users.Count(u => u.Status == (int)Status.Normal) == 0)
                return new OperationResult(OperationResultType.Error, "用户身份错误。");

            if (Boards.Count(b => b.BoardId == model.BoardId) == 0)
                return new OperationResult(OperationResultType.Error, "版区不存在。");

            DateTime PostTime = DateTime.Now;

            e_Users user = Users.FirstOrDefault(u => u.UserId == model.PostUserId);
            e_Board board = Boards.FirstOrDefault(b => b.BoardId == model.BoardId);

            e_Topic topic = new e_Topic()
            {
                BoardId = model.BoardId,
                TopicType = board.BoardType,
                Title = model.Title,
                Keywords = model.Keywords == null ? string.Empty : model.Keywords,
                PostUserId = model.PostUserId,
                PostTime = PostTime,
                PostIp = model.PostIp,
                PostUserName = user.NickName,
                Status = (int)Status.Normal,
                IsFinish = (int)YesOrNo.No,
                IsLock = (int)YesOrNo.No,
                IsDigest = (int)YesOrNo.No,
                TopLevel = 0,
                Child = 0,
                Hits = 0
            };
            TopicRepository.Insert(topic,false);

            e_BBS bbs = new e_BBS()
            {
                BoardId = model.BoardId,
                TopicId = topic.TopicId,
                Content = model.Content,
                PostUserId = model.PostUserId,
                PostUserName = user.NickName,
                PostTime = PostTime,
                PostIp = model.PostIp,
                IsTopic = (int)YesOrNo.Yes,
                IsAnswer = (int)YesOrNo.No,
                DisplayMode = (int)Mode.Public,
                AnswerMode = (int)Mode.Public,
                Status = (int)Status.Normal
            };
            BBSRepository.Insert(bbs,false);

            board.TopicNum++;
            BoardRepository.Update(board,false);

            user.TopicCnt++;
            UserRepository.Update(user);

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);

            return new OperationResult(OperationResultType.Success, "回复添加成功。", null);
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
        public IEnumerable<e_Topic> Search(SearchInfo model, int pageIndex, int pageSize, out int recordCount)
        {
            recordCount = 0;

            if (model.SearchScope > 0 && Boards.Count(b => b.BoardId == model.SearchScope) == 0)
                return null;             

            //依据版区确定搜索范围            
            IQueryable<e_BBS> _bbs = BBSs.Where(b => b.Status == (int)Status.Normal);
            IQueryable<e_Topic> _topics = Topics.Where(t => t.Status == (int)Status.Normal);
            _topics = model.SearchScope > 0 ? _topics.Where(t => t.BoardId == model.SearchScope) : _topics;
            //依据类型确定
            switch (model.SearchType)
            {
                case SearchType.TitleAndKeywords:
                    _topics = _topics.Where(t => t.Title.IndexOf(model.SearchContext) >= 0 || t.Keywords.IndexOf(model.SearchContext) >= 0);
                    break;
                case SearchType.Content:
                    _bbs = _bbs.Where(b => b.Content.IndexOf(model.SearchContext) >= 0);
                    _topics = _topics.Where(t => t.Title == model.SearchContext && _bbs.Select(b => b.TopicId).Contains(t.TopicId));
                    break;
                case SearchType.Answer:
                    _bbs = _bbs.Where(b => b.Content.IndexOf(model.SearchContext) >= 0 && b.IsAnswer == (int)YesOrNo.Yes);
                    _topics = _topics.Where(t => t.Title == model.SearchContext && _bbs.Select(b => b.TopicId).Contains(t.TopicId));
                    break;
                default:
                    break;
            }
            //依据时间确定
           // _topics = _topics.Where(t => EntityFunctions.DiffDays(t.PostTime, DateTime.Now) <= 7);
            switch (model.PostTime)
            {
                case PostTimeType.LastWeek:
                    _topics = _topics.Where(t => SqlFunctions.DateDiff("dd", t.PostTime, DateTime.Now) <= 7);
                    break;
                case PostTimeType.LastMonth:
                    _topics = _topics.Where(t => SqlFunctions.DateDiff("mm", t.PostTime, DateTime.Now) <= 1);
                    break;
                case PostTimeType.LastQuarter:
                    _topics = _topics.Where(t => SqlFunctions.DateDiff("mm", t.PostTime, DateTime.Now) <= 3);
                    break;
                case PostTimeType.LastHalfYear:
                    _topics = _topics.Where(t => SqlFunctions.DateDiff("mm", t.PostTime, DateTime.Now) <= 6);
                    break;
                default:
                    break;
            }

            return _topics.Where(t => true, pageIndex, pageSize, out recordCount, t => t.PostTime, false);
        }


        /// <summary>
        ///  更新帖子点击次数
        /// </summary>
        /// <param name="TopicId">主贴ID</param>
        /// <returns>业务操作结果</returns>
        public OperationResult SaveHits(int TopicId)
        {
            if (GetTopic(TopicId) == null)
                return new OperationResult(OperationResultType.Error, "帖子不存在。");

            e_Topic topic = GetTopic(TopicId);
            topic.Hits++;
            TopicRepository.Update(topic);

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);

            return new OperationResult(OperationResultType.Success, "回复添加成功。", null);
        }

        /// <summary>
        ///  更新精华标记
        /// </summary>
        /// <param name="TopicId">主贴ID</param>
        /// <returns>业务操作结果</returns>
        public OperationResult SetDegist(int TopicId)
        {
            if (GetTopic(TopicId) == null)
                return new OperationResult(OperationResultType.Error, "帖子不存在。");

            e_Topic topic = GetTopic(TopicId);
            topic.IsDigest = (topic.IsDigest == (int)YesOrNo.Yes) ? (int)YesOrNo.No : (int)YesOrNo.Yes;
            TopicRepository.Update(topic);

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);

            return new OperationResult(OperationResultType.Success, "精华标记更新成功。", null);
        }

        /// <summary>
        ///  更新锁定标记
        /// </summary>
        /// <param name="TopicId">主贴ID</param>
        /// <returns>业务操作结果</returns>
        public OperationResult SetLock(int TopicId)
        {
            if (GetTopic(TopicId) == null)
                return new OperationResult(OperationResultType.Error, "帖子不存在。");

            e_Topic topic = GetTopic(TopicId);
            topic.IsLock = (topic.IsLock == (int)YesOrNo.Yes) ? (int)YesOrNo.No : (int)YesOrNo.Yes;
            TopicRepository.Update(topic);

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);

            return new OperationResult(OperationResultType.Success, "锁定标记更新成功。", null);
        }

        /// <summary>
        ///  更新置顶等级
        /// </summary>
        /// <param name="TopicId">主贴ID</param>
        /// <param name="topLevel">置顶等级</param>
        /// <returns>业务操作结果</returns>
        public OperationResult SetTop(int TopicId, TopLevel topLevel)
        {
            if (GetTopic(TopicId) == null)
                return new OperationResult(OperationResultType.Error, "帖子不存在。");

            e_Topic topic = GetTopic(TopicId);
            topic.TopLevel = (int)topLevel;
            TopicRepository.Update(topic);

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);

            return new OperationResult(OperationResultType.Success, "置顶标记更新成功。", null);
        }

        /// <summary>
        ///  删除帖子
        /// </summary>
        /// <param name="topicIds">主贴ID串</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Delete( IEnumerable<int> topicIds)
        {
            foreach (int TopicId in topicIds)
            {
                e_Topic topic = GetTopic(TopicId);
                topic.Status = (int)Status.Delete;
                TopicRepository.Update(topic);
            }

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);

            return new OperationResult(OperationResultType.Success, "删除成功。", null);
        }

        /// <summary>
        ///  录入正确答案
        /// </summary>
        /// <param name="model">正确答案模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult SaveAnswer(AnswerInfo model)
        {
            if (Users.Count(u => u.Status == (int)Status.Normal) == 0)
                return new OperationResult(OperationResultType.Error, "用户身份错误。");

            if (Boards.Count(b => b.BoardId == model.BoardId) == 0)
                return new OperationResult(OperationResultType.Error, "版区不存在。");

            DateTime PostTime = DateTime.Now;

            e_Users user = Users.FirstOrDefault(u => u.UserId == model.PostUserId);
            e_Board board = Boards.FirstOrDefault(b => b.BoardId == model.BoardId);

            e_Topic topic = GetTopic(model.TopicId);
            topic.IsFinish = (int)YesOrNo.Yes;
            topic.IsLock = (int)YesOrNo.Yes;
            topic.LastPostTime = PostTime;
            topic.LastPostUserId = model.PostUserId;
            topic.LastPostUserName = user.NickName;
            TopicRepository.Update(topic, false);

            e_BBS bbs = new e_BBS()
            {
                BoardId = model.BoardId,
                TopicId = topic.TopicId,
                Content = model.ReplyContent,
                PostUserId = model.PostUserId,
                PostUserName = user.NickName,
                PostTime = PostTime,
                PostIp = model.PostIp,
                IsTopic = (int)YesOrNo.No,
                IsAnswer = (int)YesOrNo.Yes,
                DisplayMode = (int)Mode.Public,
                AnswerMode = (int)model.AnswerMode,
                Status = (int)Status.Normal
            };
            BBSRepository.Insert(bbs, false);

            board.PostNum++;
            BoardRepository.Update(board, false);

            user.PostCnt++;
            UserRepository.Update(user);

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);

            return new OperationResult(OperationResultType.Success, "正确答案添加成功。", null);
        }
    }
}
