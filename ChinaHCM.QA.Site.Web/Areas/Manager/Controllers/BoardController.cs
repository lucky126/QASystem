using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity.Validation;

using ChinaHCM.QA.Site.Model;
using ChinaHCM.QA.Site.Web.Models;
using ChinaHCM.QA.Component.Tools;
using ChinaHCM.QA.Site.Web.Controllers;
using ChinaHCM.QA.Core.Model;
using ChinaHCM.QA.Site.Web.Extensions;

namespace ChinaHCM.QA.Site.Web.Areas.Manager.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [AdminAuthorize]
    public class BoardController : BaseController
    {
        #region 属性

        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        public IBoardSiteContract BoardContract { get; private set; }

        #endregion

        #region 视图功能

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 顶部框架
        /// </summary>
        /// <returns></returns>
        public ActionResult Top()
        {
            return View();
        }

        /// <summary>
        /// 导航框架
        /// </summary>
        /// <returns></returns>
        public ActionResult Nav()
        {
            var user = (UserView)HttpContext.Session["Admin"];

            ViewBag.IsAdmin = user.IsAdmin;

            return View();
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Main()
        {
            return View();
        }

        /// <summary>
        /// 版区列表
        /// </summary>
        /// <returns></returns>
        public ViewResult BoardManager(int parentId = 0)
        {
            IList<BoardView> boards = BoardContract.GetBoardsByParentID(parentId);

            ViewBag.IsRoot = parentId == 0;
            if (parentId > 0)
            {
                BoardView _board = BoardContract.GetBoard(parentId);
                ViewBag.IsSub = _board.IsHaveSubBoard;
                ViewBag.BoardName = _board.Name;
                ViewBag.ParentId = _board.ParentID;
            }
            else
            {
                ViewBag.IsSub = false;
                ViewBag.BoardName = "系统";
                ViewBag.ParentId = 0;
            }
            return View(boards);
        }

        /// <summary>
        /// 得到版区名称
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public ActionResult GetBoardName(int boardId = 0)
        {
            if (boardId == 0)
                return Content("根版区");
            else
            {
                BoardView board = BoardContract.GetBoard(boardId);
                return View(board);
            }
        }

        /// <summary>
        /// 新增/编辑版区
        /// </summary>
        /// <param name="boardId">当前版区ID</param>
        /// <param name="parentId">父版区ID</param>
        /// <returns></returns>
        [HttpGet]
        public ViewResult EditBoard(int boardId = 0, int parentId = 0)
        {
            if (boardId == 0)
            {
                BoardModel model = new BoardModel();
                model.ParentID = parentId;

                if (parentId != 0)
                {
                    BoardView _parent = BoardContract.GetBoard(parentId);
                    model.BoardType = _parent.BoardType;
                    model.IsPublic = _parent.IsPublic;
                }
                else
                {
                    model.BoardType = BoardType.Discuss;
                }
                return View(model);
            }
            else
            {
                BoardView board = BoardContract.GetBoard(boardId);

                BoardModel model = new BoardModel()
                {
                    BoardId = board.BoardId,
                    ParentID = parentId,
                    Name = board.Name,
                    IsPublic = board.IsPublic,
                    MasterList = board.MasterList,
                    Master=board.Master,
                    BoardType = board.BoardType
                };

                if (board != null)
                    return View(model);
                else
                    return View();
            }
        }        

        /// <summary>
        /// 维护版区
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditBoard(BoardModel model)
        {
            try
            {
                OperationResult result = BoardContract.SaveBoard(model);
                string msg = result.Message ?? result.ResultType.ToDescription();
                if (result.ResultType == OperationResultType.Success)
                {
                    return RedirectToActionPermanent("BoardManager", "Board", new { parentId = model.ParentID });
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
        /// 删除版区
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public ActionResult DeleteBoard(int boardId)
        {
            return null;
        }


        /// <summary>
        /// 得到版区列表
        /// </summary>
        /// <returns></returns>
        [NonAction]
        private IEnumerable<SelectListItem> GetBoardList()
        {
            var user = (UserView)HttpContext.Session["Admin"];

            IEnumerable<BoardView> boards = BoardContract.GetBoards();

            List<SelectListItem> _boardList = new List<SelectListItem>();
            _boardList.Add(new SelectListItem { Text = "请选择", Value = "0" });
            foreach (BoardView _board in boards)
            {
                if (!user.IsAdmin && _board.Master.Count(m => m == user.UserId) == 0)
                    continue;

                string _depthName = string.Empty;
                int _depth = 0;
                while (_depth < _board.Depth)
                {
                    _depthName = string.Format("──{0}", _depthName);
                    _depth++;
                }
                
                _boardList.Add(new SelectListItem { Text = string.Format("├{0}{1}", _depthName, _board.Name), Value = _board.BoardId.ToString() });
            }

            return _boardList;
        }

        /// <summary>
        /// 帖子管理第一步--版区列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectBoard()
        {
            ViewData["boardId"] = GetBoardList();
            BoardList model = new BoardList();
            return View(model);
        }

        /// <summary>
        /// 帖子管理第一步--版区列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SelectBoard(BoardList model)
        {
            ViewData["boardId"] = GetBoardList();

            if (model.BoardId == 0)
            {
                ModelState.AddModelError("", "请选择版区");
                return View(model);
            }

            return RedirectToAction("GetBoard", new { boardId = model.BoardId });
        }

        /// <summary>
        /// 显示版区
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="qatype"></param>
        /// <returns></returns>
        public ActionResult GetBoard(int boardId, int pageIndex = 1, int qatype = 2)
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

            var user = (UserView)HttpContext.Session["Admin"];
            if (!user.IsAdmin && board.Master.Count(m => m == user.UserId) == 0)
            {
                Error _e = new Error
                {
                    Title = "错误",
                    Details = "您没有权利维护该版区的内容",
                    Cause = Server.UrlEncode("您的权限不够，或者身份错误"),
                    Solution = Server.UrlEncode("<li>重新<a href='" + Url.Action("Login", "Account") + "'>登录</a></li>")
                };
                return RedirectToAction("Error", "Prompt", _e);
            }

            ViewBag.PageIndex = pageIndex;
            ViewData["QAType"] = qatype;
            return View("Board", board);
        }

        #endregion
    }
}
