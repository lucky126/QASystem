using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.Composition;

using ChinaHCM.QA.Site.Web.Models;
using ChinaHCM.QA.Site.Model;
using ChinaHCM.QA.Component.Tools;
using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Site.Web.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class BBSController : BaseController
    {
        #region 属性

        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        public IBoardSiteContract BoardContract { get; private set; }
        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        public IBBSSiteContract BBSContract { get; private set; }
        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        public ITopicSiteContract TopicContract { get; private set; }

        #endregion

        #region 视图功能

        /// <summary>
        /// 根据主贴得到帖子列表
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public PartialViewResult GetBBSs(int topicId, int pageIndex)
        {
            int recordCount = 0;

            IList<BBSView> BBSs = new List<BBSView>();
            
            //得到主贴信息
            TopicView topic = TopicContract.GetTopic(topicId); 
            //判断当前用户
            int _currentUserId = Request.IsAuthenticated ? Convert.ToInt32(User.Identity.Name) : 0; 

            if (topic.TopicType == BoardType.QA)
            {
                BBSs = this.BBSContract.GetBBSs(topicId);
                
                TopicBBS topicBBs = new TopicBBS(topic, BBSs, _currentUserId);
                
                PartialViewResult result = PartialView("PartialQAList", topicBBs);
                ViewBag.Keywords = topic.Keywords;

                return result;
            }
            else
            {
                BBSs = this.BBSContract.GetBBSs(topicId, pageIndex, PagingInfo.PageSize, out recordCount);

                TopicBBS topicBBs = new TopicBBS(topic, BBSs, _currentUserId);

                Func<int, UrlHelper, string> pageUrlAccessor =
               (currentPage, helper) => helper.RouteUrl("TopicPage", new { PageIndex = currentPage, topicId = topicId }).ToString();

                return RenderBBSList(topicBBs, recordCount, pageIndex, pageUrlAccessor);
            }
        }

        /// <summary>
        /// 等到帖子分页分部视图
        /// </summary>
        /// <param name="topicBBs"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageUrlAccessor"></param>
        /// <returns></returns>
        private PartialViewResult RenderBBSList(TopicBBS topicBBs, int recordCount, int pageIndex,
            Func<int, UrlHelper, string> pageUrlAccessor)
        {
            PartialViewResult result = PartialView("PartialDiscussList", topicBBs);

            ViewBag.RecordCount = recordCount;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageUrlAccessor = pageUrlAccessor;

            return result;
        }

        /// <summary>
        /// 添加QA回复
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public ActionResult GetQAReply(int topicId)
        {
            //是否可以回复，首先必须登录
            bool IsCanReply = Request.IsAuthenticated;

            TopicView topic = TopicContract.GetTopic(topicId);

            if (!BoardContract.CanVisitedBoard(topic.BoardId, !Request.IsAuthenticated))
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

            //如果是QA则必须满足若干条件
            if (topic.TopicType == BoardType.QA)
            {
                IsCanReply = IsCanReply && BBSContract.CanReplay(topicId, Convert.ToInt32(User.Identity.Name));
            }

            if (IsCanReply)
            {
                ReplyMode model = new ReplyMode()
                {
                    Title = "发表回复",
                    SubmitButtonValue = " 提交回复 ",
                    IsShowCloseButton = false,
                    CloseJSFunction = string.Empty,
                    ActionName = "GetQAReplySave",
                    PostUserId = Convert.ToInt32(User.Identity.Name),
                    TopicId = topicId,
                    BoardId = topic.BoardId,
                    BBSId=0,
                    ReplyContent = Request["ReplyContent"] ?? string.Empty
                };

                return View(model);
            }
            else
                return new EmptyResult();
        }

        /// <summary>
        /// 保存QA回复
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult GetQAReplySave(ReplyMode model)
        {
            try
            {
                model.PostUserId = Convert.ToInt32(User.Identity.Name);
                OperationResult result = BBSContract.SaveReply(model);
                string msg = result.Message ?? result.ResultType.ToDescription();
                if (result.ResultType == OperationResultType.Success)
                {
                    return RedirectToActionPermanent("GetTopic", "Topic", new { topicId = model.TopicId });
                }
                ModelState.AddModelError("", msg);
                return RedirectToActionPermanent("GetTopic", "Topic", new { topicId = model.TopicId });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return RedirectToActionPermanent("GetTopic", "Topic", new { topicId = model.TopicId });
            }
        }

        /// <summary>
        /// 修改QA回复
        /// </summary>
        /// <param name="bbsId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult GetEditReply(int bbsId)
        {
            //是否可以修改，首先必须登录
            bool IsCanEdit = Request.IsAuthenticated;

            BBSView bbs = BBSContract.GetBBS(bbsId);

            if (!BoardContract.CanVisitedBoard(bbs.BoardId, !Request.IsAuthenticated))
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

            //判断编辑许可
            IsCanEdit = IsCanEdit && BBSContract.CanEditReplay(bbs.TopicId, bbsId, Convert.ToInt32(User.Identity.Name));

            if (IsCanEdit)
            {
                ReplyMode model = new ReplyMode()
                {
                    Title = "修改回复",
                    SubmitButtonValue = " 保存修改 ",
                    IsShowCloseButton = true,
                    CloseJSFunction = "Close()",
                    ActionName = "GetQAReplyEdit",
                    PostUserId = Convert.ToInt32(User.Identity.Name),
                    TopicId = bbs.TopicId,
                    BoardId = bbs.BoardId,
                    BBSId=bbsId,
                    ReplyContent = bbs.Content
                };

                return View("GetQAReply", model);
            }
            else
                return new EmptyResult();
        }

        /// <summary>
        /// 保存QA回复修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult GetQAReplyEdit(ReplyMode model)
        {
            try
            {
                model.PostUserId = Convert.ToInt32(User.Identity.Name);
                OperationResult result = BBSContract.SaveReplyEdit(model);
                string msg = result.Message ?? result.ResultType.ToDescription();
                if (result.ResultType == OperationResultType.Success)
                {
                    return RedirectToActionPermanent("GetTopic", "Topic", new { topicId = model.TopicId });
                }
                ModelState.AddModelError("", msg);
                return RedirectToActionPermanent("GetTopic", "Topic", new { topicId = model.TopicId });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return RedirectToActionPermanent("GetTopic", "Topic", new { topicId = model.TopicId });
            }
        }

        #endregion
    }
}
