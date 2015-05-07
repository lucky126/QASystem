using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.ComponentModel.Composition;

using ChinaHCM.QA.Site.Model;
using ChinaHCM.QA.Site.Web.Models;
using ChinaHCM.QA.Component.Tools;
using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Site.Web.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
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
        /// 得到页面导航
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public PartialViewResult Nav(int boardId, int topicId = 0, string lastNav = "")
        {
            //得到版区信息
            BoardView board = boardId > 0 ? BoardContract.GetBoard(boardId) : null;

            //得到主贴信息
            TopicView topic = null;
            if (topicId > 0)
                topic = TopicContract.GetTopic(topicId);

            //得到导航信息
            NavModel model = new NavModel(BoardContract.GetBoards(), board, topic, lastNav);

            //返回生成导航
            return PartialView("PartialNav", model);
        }

        /// <summary>
        /// 根据版区得到该版区所有主贴信息（分类别，分页）
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="qatype"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult GetTopics(int boardId, int qatype, int pageIndex = 1)
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
                (currentPage, helper) => helper.RouteUrl("BoardPage", new { PageIndex = currentPage, boardId = boardId }).ToString();

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

            //如果没有登录只显示公开版区
            bool bOnlyGetPublic = !Request.IsAuthenticated;

            if (!BoardContract.CanVisitedBoard(topic.BoardId, bOnlyGetPublic))
            {
                Error _e = new Error
                {
                    Title = "无权访问",
                    Details = "您无权访问该版区",
                    Cause = Server.UrlEncode("<li>您可能未登录</li><li>您的账户权限不够</li>"),
                    Solution = Server.UrlEncode("<a href=\"javascript:void(0)\" onclick=\"show('tologin')\">点击此处</a> 完成登录")
                };
                return RedirectToAction("Error", "Prompt", _e);
            }

            ViewBag.PageIndex = pageIndex;

            return View("Topic", topic);
        }

        /// <summary>
        /// 发帖
        /// </summary>
        /// <param name="boardId">版区ID</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult NewTopic(int boardId)
        {
            BoardView board = BoardContract.GetBoard(boardId);

            if (board == null)
            {
                Error _e = new Error
                {
                    Title = "错误",
                    Details = "指定的版区不存在",
                    Cause = Server.UrlEncode("您访问的版区可能已经被删除"),
                    Solution = Server.UrlEncode("<li>返回<a href='" + Url.Action("Index", "Board") + "'>网站首页</a></li>")
                };
                return RedirectToAction("Error", "Prompt", _e);
            }

            NewTopicModel model = new NewTopicModel()
            {
                BoardId = boardId,
                PostUserId = Convert.ToInt32(User.Identity.Name)
            };

            return View(model);
        }

        /// <summary>
        /// 发帖验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult NewTopic(NewTopicModel model)
        {
            try
            {
                model.PostUserId = Convert.ToInt32(User.Identity.Name);
                OperationResult result = TopicContract.SaveTopic(model);
                string msg = result.Message ?? result.ResultType.ToDescription();
                if (result.ResultType == OperationResultType.Success)
                {
                    return RedirectToActionPermanent("GetBoard", "Board", new { boardId = model.BoardId });
                }
                ModelState.AddModelError("", msg);
                return View(model);
            }
            catch (DbEntityValidationException dbEx)
            {
                ModelState.AddModelError("", dbEx.Message);
                return View(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public ActionResult SearchInfo(int boardId)
        {
            AdvanceSearchModel model = new AdvanceSearchModel() { BoardId = boardId };

            IList<BoardView> boards = BoardContract.GetBoards();

            List<SelectListItem> _searchScope = new List<SelectListItem>();
            _searchScope.Add(new SelectListItem { Text = "请选择", Value = "0" });
            foreach (BoardView _board in boards)
            {
                string _depthName = string.Empty;
                int _depth = 0;
                while (_depth < _board.Depth)
                {
                    _depthName = string.Format("──{0}", _depthName);
                    _depth++;
                }
                _searchScope.Add(new SelectListItem { Text = string.Format("├{0}{1}", _depthName, _board.Name), Value = _board.BoardId.ToString(), Selected = boardId == _board.BoardId });
            }
            ViewData["SearchScope"] = _searchScope;

            return View(model);
        }

        /// <summary>
        /// 搜索结果
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult SearchResult(AdvanceSearchModel model, int pageIndex = 1)
        {
            try
            {
                int recordCount;

                IEnumerable<TopicView> _topics = TopicContract.Search(model, pageIndex, PagingInfo.PageSize, out recordCount);

                Func<int, UrlHelper, string> pageUrlAccessor =
               (currentPage, helper) => helper.Action("SearchResult",
                   new
                   {
                       pageIndex = currentPage,
                       SearchContext = model.SearchContext,
                       SearchScope = model.SearchScope,
                       SearchType = model.SearchType,
                       PostTime = model.PostTime,
                       BoardId = model.BoardId
                   }).ToString();

                ViewBag.RecordCount = recordCount;
                ViewBag.PageIndex = pageIndex;
                ViewBag.PageUrlAccessor = pageUrlAccessor;

                SearchResult result = new SearchResult() { Search = model, topics = _topics };

                if (_topics != null)
                {                   
                    return View(result);
                }
                else
                {
                    ModelState.AddModelError("", "查询出错");
                    return View(result);
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                ModelState.AddModelError("", dbEx.Message);
                return View(new SearchResult() { Search = model, topics = null });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(new SearchResult() { Search = model, topics = null });
            }
        }

        /// <summary>
        /// 快速搜索结果
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BasicSearchResult(BasicSearchModel model)
        {
            AdvanceSearchModel advModel = new AdvanceSearchModel()
            {
                SearchContext = model.SearchContext,
                BoardId = model.BoardId,
                SearchScope = model.BoardId,
                SearchType = SearchType.TitleAndKeywords,
                PostTime = PostTimeType.LastWeek
            };
            return RedirectToAction("SearchResult", advModel);
        }


        /// <summary>
        ///  保存帖子点击次数
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveHits(int topicId)
        {
            OperationResult result = TopicContract.SaveHits(topicId);
            string msg = result.Message ?? result.ResultType.ToDescription();
            if (result.ResultType == OperationResultType.Success)
            {
                JsonReturn data = new JsonReturn() { Flag = 0, ErrorMessage = string.Empty };
                return Json(data);
            }

            JsonReturn datas = new JsonReturn() { Flag = 0, ErrorMessage = msg };
            return Json(datas);
        }

        #endregion
    }
}
