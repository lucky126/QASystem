using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.Composition;

using ChinaHCM.QA.Site.Model;
using ChinaHCM.QA.Core.Model;
using ChinaHCM.QA.Site.Web.Controllers;
using ChinaHCM.QA.Component.Tools;
using ChinaHCM.QA.Site.Web.Models;
using ChinaHCM.QA.Site.Web.Extensions;

namespace ChinaHCM.QA.Site.Web.Areas.Manager.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [AdminAuthorize]
    public class TopicController : BaseController
    {
        #region 属性

        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        public ITopicSiteContract TopicContract { get; private set; }
        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        public IBoardSiteContract BoardContract { get; private set; }

        #endregion

        #region 视图功能

        /// <summary>
        /// 根据版区得到该版区所有主贴信息（分类别，分页）
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="qatype"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult GetTopics(int boardId, int qatype = 2, int pageIndex = 1)
        {
            int recordCount;

            BoardView board = BoardContract.GetBoard(boardId);

            IList<TopicView> topics;

            if (board.BoardType == BoardType.QA)
            {
                topics = TopicContract.GetTopicsByType(boardId, (QAType)qatype, pageIndex, PagingInfo.PageSize, out recordCount);
            }
            else
            {
                topics = TopicContract.GetTopics(boardId, pageIndex, PagingInfo.PageSize, out recordCount);
            }

            Func<int, UrlHelper, string> pageUrlAccessor =
                (currentPage, helper) => helper.Action("GetBoard", "Board", new { PageIndex = currentPage, boardId = boardId, qatype = qatype }).ToString();

            return RenderTopicList(topics, recordCount, pageIndex, board.BoardType, pageUrlAccessor);
        }

        /// <summary>
        /// 得到版区主贴分页分部视图
        /// </summary>
        /// <param name="topics"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="boardType"></param>
        /// <param name="pageUrlAccessor"></param>
        /// <returns></returns>
        private PartialViewResult RenderTopicList(IList<TopicView> topics, int recordCount, int pageIndex, BoardType boardType,
            Func<int, UrlHelper, string> pageUrlAccessor)
        {
            PartialViewResult result = PartialView("PartialDiscussList", topics);

            if (boardType == BoardType.QA)
            {
                result = PartialView("PartialQAList", topics);
            }

            ViewBag.RecordCount = recordCount;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageUrlAccessor = pageUrlAccessor;
            return result;
        }

        /// <summary>
        /// 得到主贴信息
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult GetTopic(int topicId, int pageIndex = 1)
        {
            TopicView topic = TopicContract.GetTopic(topicId);

            if (topic == null)
            {
                Error _e = new Error
                {
                    Title = "错误",
                    Details = "指定的主题不存在",
                    Cause = Server.UrlEncode("您访问的主题可能已经被删除"),
                    Solution = Server.UrlEncode("<li>返回<a href='" + Url.Action("Index", "Board") + "'>网站首页</a></li>")
                };
                return RedirectToAction("Error", "Prompt", _e);
            }

            ViewBag.PageIndex = pageIndex;

            return View("Topic", topic);
        }

        /// <summary>
        /// 主贴管理分部视图
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public PartialViewResult TopicManager(int topicId)
        {
            TopicView topic = TopicContract.GetTopic(topicId);

            return PartialView(topic);
        }

        /// <summary>
        /// 设置精华
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetDegist(int topicId)
        {
            OperationResult result = TopicContract.SetDegist(topicId);
            string msg = result.Message ?? result.ResultType.ToDescription();
            if (result.ResultType == OperationResultType.Success)
            {
                JsonReturn data = new JsonReturn() { Flag = 0, ErrorMessage = string.Empty };
                return Json(data);
            }
            JsonReturn datas = new JsonReturn() { Flag = -1, ErrorMessage = msg };
            return Json(datas);
        }

        /// <summary>
        /// 设置锁定
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetLock(int topicId)
        {
            OperationResult result = TopicContract.SetLock(topicId);
            string msg = result.Message ?? result.ResultType.ToDescription();
            if (result.ResultType == OperationResultType.Success)
            {
                JsonReturn data = new JsonReturn() { Flag = 0, ErrorMessage = string.Empty };
                return Json(data);
            }
            JsonReturn datas = new JsonReturn() { Flag = -1, ErrorMessage = msg };
            return Json(datas);
        }

        /// <summary>
        /// 置顶列表
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public ViewResult ListTop(int boardId, int topicId)
        {
            IEnumerable<int> tops = TopicContract.GetTopTopics(boardId).Select(t => t.TopLevel);
            TopicView topic = TopicContract.GetTopic(topicId);

            //置顶等级下拉框
            List<SelectListItem> _topList = new List<SelectListItem>();
            _topList.Add(new SelectListItem { Text = "不置顶", Value = "0" });
            foreach (int value in Enum.GetValues(typeof(TopLevel)))
            {
                if (value > 0)
                {
                    string _name = string.Format("{0}{1}", value, tops.Count(i => i == value) > 0 ? "(已设置)" : string.Empty);
                    _topList.Add(new SelectListItem { Text = _name, Value = value.ToString(), Selected = topic.TopLevel == value });
                }
            }

            ViewData["TopLevel"] = _topList;

            TopLevelModel model = new TopLevelModel() { TopicId = topicId };

            return View(model);
        }

        /// <summary>
        /// 设置置顶
        /// </summary>
        /// <param name="TopLevel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetTop(TopLevelModel model)
        {
            OperationResult result = TopicContract.SetTop(model.TopicId, model.TopLevel);
            string msg = result.Message ?? result.ResultType.ToDescription();
            if (result.ResultType == OperationResultType.Success)
            {
                JsonReturn data = new JsonReturn() { Flag = 0, ErrorMessage = string.Empty };
                return Json(data);
            }
            JsonReturn datas = new JsonReturn() { Flag = -1, ErrorMessage = msg };
            return Json(datas);
        }

        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string[] del)
        {
            //得到需要删除的TopicID
            IEnumerable<int> model = del.Where(d => string.Compare(d, "false", true) != 0).Select(d => Convert.ToInt32(d)).ToList<int>();
            //提交操作
            OperationResult result = TopicContract.Delete(model);
            string msg = result.Message ?? result.ResultType.ToDescription();
            if (result.ResultType == OperationResultType.Success)
            {
                JsonReturn data = new JsonReturn() { Flag = 0, ErrorMessage = string.Empty };
                return Json(data);
            }
            JsonReturn datas = new JsonReturn() { Flag = -1, ErrorMessage = msg };
            return Json(datas);
        }

        /// <summary>
        /// 录入正确答案
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public ViewResult GetAnswer(int topicId)
        {
            TopicView topic = TopicContract.GetTopic(topicId);

            AnswerModel model = new AnswerModel()
            {
                TopicId = topicId,
                BoardId = topic.BoardId
            };

            List<SelectListItem> _List = new List<SelectListItem>();
            foreach (int value in Enum.GetValues(typeof(Mode)))
            {
                _List.Add(new SelectListItem { Text = GlobalConst.ModeName[value], Value = value.ToString() });
            }

            ViewData["AnswerMode"] = _List;

            return View(model);
        }

        /// <summary>
        /// 保存正确答案
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAnswerSave(AnswerModel model)
        {
            try
            {
                var user = (UserView)HttpContext.Session["Admin"];
                model.PostUserId = user.UserId;
                OperationResult result = TopicContract.SaveAnswer(model);
                string msg = result.Message ?? result.ResultType.ToDescription();
                if (result.ResultType == OperationResultType.Success)
                {
                    JsonReturn data = new JsonReturn() { Flag = 0, ErrorMessage = string.Empty };
                    return Json(data);
                }
                else
                {
                    JsonReturn datas = new JsonReturn() { Flag = -1, ErrorMessage = msg };
                    return Json(datas);
                }
            }
            catch (Exception e)
            {
                JsonReturn datas = new JsonReturn() { Flag = -1, ErrorMessage = e.Message };
                return Json(datas);
            }
        }
        #endregion
    }
}
