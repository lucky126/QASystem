using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.Composition;

using ChinaHCM.QA.Site.Model;
using ChinaHCM.QA.Site.Web.Models;

namespace ChinaHCM.QA.Site.Web.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class BoardController : BaseController
    {
        #region 属性

        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        public IBoardSiteContract BoardContract { get; private set; }

        #endregion

        #region 视图功能

        /// <summary>
        /// 显示所有父版区列表
        /// </summary> 
        public ActionResult Index()
        {
            //如果没有登录只显示公开版区
            bool bOnlyGetPublic = !Request.IsAuthenticated;

            IList<BoardView> boards = BoardContract.GetParentBoards(bOnlyGetPublic);

            if (boards == null || boards.Count==0)
            {
                Error _e = new Error
                {
                    Title = "错误",
                    Details = "没有任何版区",
                    Cause = Server.UrlEncode("<li>版区未设置</li><li>您访问的版区可能已经被删除</li>"),
                    Solution = Server.UrlEncode("暂时无法访问")
                };
                return RedirectToAction("Error", "Prompt", _e);
            }

            ViewBag.Title = "你问我答系统";

            return View(boards);
        }
        
        /// <summary>
        /// 根据父版区得到子版区列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult GetChild(int parentId)
        {
            IList<BoardView> boards = BoardContract.GetBoardsByParentID(parentId);

            return PartialView("PartialChildBoard", boards);
        }

         /// <summary>
        /// 显示版区名称
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        [OutputCache(Duration = 3600)]
        public ContentResult GetBoardName(int boardId)
        {
            string boardName = string.Empty;

            if (boardId == 0)
                boardName = "全部";
            else
            {
                BoardView board = BoardContract.GetBoard(boardId);
                if (board != null)
                    boardName = board.Name;
            }
            return Content(boardName);
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
            //如果没有登录只显示公开版区
            bool bOnlyGetPublic = !Request.IsAuthenticated;

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

            if (!BoardContract.CanVisitedBoard(boardId, bOnlyGetPublic))
            {
                Error _e = new Error { 
                    Title = "访问限制", 
                    Details = "您无权访问该版区", 
                    Cause = Server.UrlEncode("<li>您可能未登录</li><li>您的账户权限不够</li>"), 
                    Solution = Server.UrlEncode("<a href=\"javascript:void(0)\" onclick=\"show('tologin')\">点击此处</a> 完成登录") };
                return RedirectToAction("Error", "Prompt", _e);
            }           

            ViewBag.PageIndex = pageIndex;
            ViewData["QAType"] = qatype;
            return View("Board", board);
        }

        #endregion
    }
}
