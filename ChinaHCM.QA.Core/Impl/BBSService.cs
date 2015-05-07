using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;

using ChinaHCM.QA.Core.Data;
using ChinaHCM.QA.Core.Model;
using ChinaHCM.QA.Component.Data;
using ChinaHCM.QA.Component.Tools;
using ChinaHCM.QA.Core.Model.BBS;

namespace ChinaHCM.QA.Core
{
    /// <summary>
    ///     帖子模块核心业务实现
    /// </summary>
    public class BBSService : CoreServiceBase, IBBSService
    {
        #region 属性

        #region 受保护的属性

        /// <summary>
        /// 获取或设置 帖子信息数据访问对象
        /// </summary>
        [Import]
        protected IBBSRepository BBSRepository { get; private set; }

        /// <summary>
        /// 获取或设置 用户信息核心业务访问对象
        /// </summary>
        [Import]
        protected IUserRepository UserRepository { get; private set; }

        /// <summary>
        /// 获取或设置 主贴信息核心业务访问对象
        /// </summary>  
        [Import]
        protected ITopicRepository TopicRepository { get; private set; }

        /// <summary>
        /// 获取或设置 版区信息核心业务访问对象
        /// </summary>
        [Import]
        protected IBoardRepository BoardRepository { get; private set; }

        #endregion

        #region 公共属性

        /// <summary>
        /// 获取 帖子信息查询数据集
        /// </summary>
        public IQueryable<e_BBS> BBSs
        {
            get { return BBSRepository.Entities; }
        }

        /// <summary>
        /// 获取 用户信息查询数据集
        /// </summary>
        public IQueryable<e_Users> Users
        {
            get { return UserRepository.Entities; }
        }

        /// <summary>
        /// 获取 主贴信息查询数据集
        /// </summary>
        public IQueryable<e_Topic> Topics
        {
            get { return TopicRepository.Entities; }
        }

        /// <summary>
        /// 获取 版区信息查询数据集
        /// </summary>
        public IQueryable<e_Board> Boards
        {
            get { return BoardRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        /// 根据主贴得到全部帖子信息
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public virtual IEnumerable<e_BBS> GetBBSs(int topicId)
        {
            return BBSs.Where(b => b.TopicId == topicId && b.Status == (int)Status.Normal).OrderByDescending(b => b.PostTime);
        }

        /// <summary>
        /// 根据主贴得到全部帖子信息(分页)
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public virtual IEnumerable<e_BBS> GetBBSs(int topicId, int pageIndex, int pageSize, out int recordCount)
        {
            return BBSs.Where(b => b.TopicId == topicId && b.Status == (int)Status.Normal,
                                          pageIndex, pageSize, out recordCount,
                                          b => b.PostTime, true);
        }

        /// <summary>
        /// 得到指定帖子信息
        /// </summary>
        /// <param name="bbsId"></param>
        /// <returns></returns>
        public virtual e_BBS GetBBS(int bbsId)
        {
            if (BBSRepository.Entities.Count(b => b.BBSId == bbsId && b.Status == (int)Status.Normal) == 0)
                return null;

            return BBSRepository.GetByKey(bbsId);
        }

        /// <summary>
        /// 判断是否允许回复QA
        /// </summary>
        /// <param name="topicId">主贴ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public virtual bool CanReplay(int topicId, int userId)
        {
            PublicHelper.CheckArgument(topicId, "topicId");
            PublicHelper.CheckArgument(userId, "userId");

            bool CanReplay = true;

            //如果是发帖人，或者存在当前用户的回帖，或者锁定帖子，或者帖子完结，或者已经存在正确答案则不允许回复
            if (GetBBSs(topicId).Count(b => b.PostUserId == userId && b.IsTopic == (int)YesOrNo.No) > 0 ||
                GetBBSs(topicId).Count(b => b.PostUserId == userId && b.IsTopic == (int)YesOrNo.Yes) == 1 ||
                Topics.Count(t => t.TopicId == topicId && t.IsFinish == (int)YesOrNo.Yes) > 0 ||
                Topics.Count(t => t.TopicId == topicId && t.IsLock == (int)YesOrNo.Yes) > 0 ||
                GetBBSs(topicId).Count(b => b.IsAnswer == (int)YesOrNo.Yes) > 0)
            {
                CanReplay = false;
            }

            return CanReplay;
        }

        /// <summary>
        ///   回帖
        /// </summary>
        /// <param name="replyInfo">回帖信息</param>
        /// <returns>业务操作结果</returns>
        public virtual OperationResult SaveReply(ReplyInfo replyInfo)
        {
            if (Topics.Count(t => t.TopicId == replyInfo.TopicId && t.Status == (int)Status.Normal) == 0)
                return new OperationResult(OperationResultType.ParamError, "主贴不存在。");

            if (Users.Count(u => u.Status == (int)Status.Normal) == 0)
                return new OperationResult(OperationResultType.ParamError, "用户身份错误。");

            if (TopicRepository.GetByKey(replyInfo.TopicId).TopicType == (int)BoardType.QA &&
                GetBBSs(replyInfo.TopicId).Count(b => b.IsTopic == (int)YesOrNo.No && b.PostUserId == replyInfo.PostUserId) > 0)
                return new OperationResult(OperationResultType.Error, "您已经回复过该帖。");

            DateTime PostTime = DateTime.Now;

            e_Users user = Users.FirstOrDefault(u => u.UserId == replyInfo.PostUserId);

            e_BBS bbs = new e_BBS()
            {
                BoardId = replyInfo.BoardId,
                TopicId = replyInfo.TopicId,
                Content = replyInfo.ReplyContent,
                PostUserId = replyInfo.PostUserId,
                PostUserName = user.NickName,
                PostTime = PostTime,
                PostIp = replyInfo.PostIp,
                IsTopic = (int)YesOrNo.No,
                IsAnswer = (int)YesOrNo.No,
                DisplayMode = (int)Mode.Public,
                AnswerMode = (int)Mode.Public,
                Status = (int)Status.Normal
            };
            BBSRepository.Insert(bbs, false);

            e_Topic topic = Topics.FirstOrDefault(t => t.TopicId == replyInfo.TopicId);
            topic.Child++;
            topic.LastPostUserId = replyInfo.PostUserId;
            topic.LastPostTime = PostTime;
            topic.LastPostUserName = user.NickName;
            TopicRepository.Update(topic, false);

            e_Board board = Boards.FirstOrDefault(b => b.BoardId == replyInfo.BoardId);
            board.PostNum++;
            BoardRepository.Update(board, false);

            user.PostCnt++;
            UserRepository.Update(user);

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);

            return new OperationResult(OperationResultType.Success, "回复添加成功。", null);
        }

        /// <summary>
        /// 判断是否允许编辑QA
        /// </summary>
        /// <param name="topicId">主贴ID</param>
        /// <param name="bbsId">帖子ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public virtual bool CanEditReplay(int topicId, int bbsId, int userId)
        {
            PublicHelper.CheckArgument(topicId, "topicId");
            PublicHelper.CheckArgument(topicId, "bbsId");
            PublicHelper.CheckArgument(userId, "userId");

            bool CanEditReplay = true;

            //如果是不是发帖人，或者锁定帖子，或者帖子完结，或者已经存在正确答案则不允许回复
            if (GetBBS(bbsId).PostUserId != userId ||
                Topics.Count(t => t.TopicId == topicId && t.IsFinish == (int)YesOrNo.Yes) > 0 ||
                Topics.Count(t => t.TopicId == topicId && t.IsLock == (int)YesOrNo.Yes) > 0 ||
                GetBBSs(topicId).Count(b => b.IsAnswer == (int)YesOrNo.Yes) > 0)
            {
                CanEditReplay = false;
            }

            return CanEditReplay;
        }

        /// <summary>
        ///   回帖修改
        /// </summary>
        /// <param name="replyInfo">回帖信息</param>
        /// <returns>业务操作结果</returns>
        public virtual OperationResult SaveReplyEdit(ReplyInfo replyInfo)
        {
            if (Topics.Count(t => t.TopicId == replyInfo.TopicId && t.Status == (int)Status.Normal) == 0)
                return new OperationResult(OperationResultType.ParamError, "主贴不存在。");

            if (Users.Count(u => u.Status == (int)Status.Normal) == 0)
                return new OperationResult(OperationResultType.ParamError, "用户身份错误。");

            if(BBSs.Count(b=>b.BBSId==replyInfo.BBSId && b.PostUserId==replyInfo.PostUserId)==0)
                return new OperationResult(OperationResultType.Error, "您并没有回复该帖。");

            DateTime PostTime = DateTime.Now;

            e_Users user = Users.FirstOrDefault(u => u.UserId == replyInfo.PostUserId);

            e_BBS bbs = GetBBS(replyInfo.BBSId);
            bbs.Content = replyInfo.ReplyContent;
          
            BBSRepository.Update(bbs);          

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);

            return new OperationResult(OperationResultType.Success, "回复修改成功。", null);
        }

        /// <summary>
        ///  更新隐藏标记
        /// </summary>
        /// <param name="bbsId">帖子ID</param>
        /// <returns>业务操作结果</returns>
        public OperationResult SetHidden(int bbsId)
        {
            if (GetBBS(bbsId) == null)
                return new OperationResult(OperationResultType.Error, "帖子不存在。");

            e_BBS bbs = GetBBS(bbsId);
            bbs.DisplayMode = (bbs.DisplayMode == (int)Mode.Private) ? (int)Mode.Public : (int)Mode.Private;
            BBSRepository.Update(bbs);

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);

            return new OperationResult(OperationResultType.Success, "隐藏标记更新成功。", null);
        }

        /// <summary>
        ///  删除帖子
        /// </summary>
        /// <param name="bbsId">帖子ID</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Delete(int bbsId)
        {
            if (GetBBS(bbsId) == null)
                return new OperationResult(OperationResultType.Error, "帖子不存在。");

            e_BBS bbs = GetBBS(bbsId);
            bbs.Status = (int)Status.Delete;
            BBSRepository.Update(bbs);

            //e_Topic topic = TopicRepository.Entities.SingleOrDefault(t => t.TopicId == bbs.TopicId);
            e_Board board = BoardRepository.Entities.SingleOrDefault(b => b.BoardId == bbs.BoardId);
            board.PostNum--;
            BoardRepository.Update(board);

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);

            return new OperationResult(OperationResultType.Success, "帖子删除更新成功。", null);
        }

        /// <summary>
        ///  设置为正确答案
        /// </summary>
        /// <param name="bbsId">帖子ID</param>
        /// <returns>业务操作结果</returns>
        public OperationResult SetAnswer(int bbsId)
        {
            if (GetBBS(bbsId) == null)
                return new OperationResult(OperationResultType.Error, "帖子不存在。");

            e_BBS bbs = GetBBS(bbsId);
            bbs.IsAnswer = (int)YesOrNo.Yes;
            BBSRepository.Update(bbs, false);

            e_Topic topic = TopicRepository.Entities.SingleOrDefault(t => t.TopicId == bbs.TopicId);
            topic.IsFinish = (int)YesOrNo.Yes;
            topic.IsLock = (int)YesOrNo.Yes;
            TopicRepository.Update(topic);         

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);

            return new OperationResult(OperationResultType.Success, "帖子删除更新成功。", null);
        }

        /// <summary>
        ///  修改为正确答案
        /// </summary>
        /// <param name="bbsId">帖子ID</param>
        /// <param name="userId">当前用户ID</param>
        /// <returns>业务操作结果</returns>
        public OperationResult ChangeAnswer(int bbsId, int userId)
        {
            if (GetBBS(bbsId) == null)
                return new OperationResult(OperationResultType.Error, "帖子不存在。");

            e_BBS bbs = GetBBS(bbsId);

            e_BBS oldAnswer = BBSRepository.Entities.SingleOrDefault(b => b.IsAnswer == (int)YesOrNo.Yes && b.TopicId==bbs.TopicId);
            if (oldAnswer.PostUserId == userId)
            {
                BBSRepository.Delete(oldAnswer, false);
            }
            else
            {
                oldAnswer.IsAnswer = (int)YesOrNo.No;
                BBSRepository.Update(oldAnswer, false);
            }

            e_Board board = BoardRepository.Entities.SingleOrDefault(b => b.BoardId == bbs.BoardId);
            if (oldAnswer.PostUserId == userId)
            {
                board.PostNum--;
                BoardRepository.Update(board, false);
            }

            bbs.IsAnswer = (int)YesOrNo.Yes;
            BBSRepository.Update(bbs);

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);

            return new OperationResult(OperationResultType.Success, "帖子删除更新成功。", null);
        }
    }
}
