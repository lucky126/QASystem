using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.Composition;

using ChinaHCM.QA.Core;
using ChinaHCM.QA.Site.Web.Models;
using ChinaHCM.QA.Component.Data;
using ChinaHCM.QA.Site.Model;
using ChinaHCM.QA.Component.Tools;
using ChinaHCM.QA.Core.Model;
using ChinaHCM.QA.Site.Web.Extensions;

namespace ChinaHCM.QA.Site.Web.Areas.Manager.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [AdminAuthorize]
    public class BBSController : Controller
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

            TopicView topic = TopicContract.GetTopic(topicId);

            if (topic.TopicType == BoardType.QA)
            {
                BBSs = this.BBSContract.GetBBSs(topicId);

                TopicBBS topicBBs = new TopicBBS(topic, BBSs, 0, true);

                PartialViewResult result = PartialView("PartialQAList", topicBBs);
                ViewBag.Keywords = topic.Keywords;

                return result;
            }
            else
            {
                BBSs = this.BBSContract.GetBBSs(topicId, pageIndex, PagingInfo.PageSize, out recordCount);

                TopicBBS topicBBs = new TopicBBS(topic, BBSs, 0, true);

                Func<int, UrlHelper, string> pageUrlAccessor =
               (currentPage, helper) => helper.Action("GetTopic", "Topic", new { PageIndex = currentPage, topicId = topicId }).ToString();

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
            TopicView topic = TopicContract.GetTopic(topicId);
            var user = (UserView)HttpContext.Session["Admin"];
            ReplyMode model = new ReplyMode()
            {
                Title = "发表回复",
                SubmitButtonValue = " 提交回复 ",
                IsShowCloseButton = false,
                CloseJSFunction = string.Empty,
                ActionName = "GetQAReplySave",
                PostUserId = user.UserId,
                TopicId = topicId,
                BoardId = topic.BoardId,
                BBSId = 0,
                ReplyContent = Request["ReplyContent"] ?? string.Empty
            };

            return View(model);
        }

        /// <summary>
        /// 保存QA回复
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetQAReplySave(ReplyMode model)
        {
            try
            {
                var user = (UserView)HttpContext.Session["Admin"];
                model.PostUserId = user.UserId;
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
        /// 帖子管理分部视图
        /// </summary>
        /// <param name="bbsId"></param>
        /// <returns></returns>
        public PartialViewResult BBSManager(int bbsId)
        {
            BBSView bbs = BBSContract.GetBBS(bbsId);

            TopicView topic = TopicContract.GetTopic(bbs.TopicId);

            ViewBag.TopicType = topic.TopicType;
            ViewBag.HasAnswer = topic.IsFinish;

            return PartialView(bbs);
        }

        /// <summary>
        /// 设置隐藏
        /// </summary>
        /// <param name="bbsId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetHidden(int bbsId)
        {
            OperationResult result = BBSContract.SetHidden(bbsId);
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
        /// <param name="bbsId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int bbsId)
        {
            OperationResult result = BBSContract.Delete(bbsId);
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
        /// 设置为正确答案
        /// </summary>
        /// <param name="bbsId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetAnswer(int bbsId)
        {
            OperationResult result = BBSContract.SetAnswer(bbsId);
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
        /// 修改为正确答案
        /// </summary>
        /// <param name="bbsId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeAnswer(int bbsId)
        {
            var user = (UserView)HttpContext.Session["Admin"];
            OperationResult result = BBSContract.ChangeAnswer(bbsId, user.UserId);
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
        /// 修改正确答案
        /// </summary>
        /// <param name="bbsId"></param>
        /// <returns></returns>
        public ActionResult GetEditAnswer(int bbsId)
        {
            BBSView bbs = BBSContract.GetBBS(bbsId);

            ReplyMode model = new ReplyMode()
            {
                Title = "修改答案",
                SubmitButtonValue = " 保存修改 ",
                IsShowCloseButton = true,
                CloseJSFunction = "Close()",
                ActionName = "GetEditAnswerSave",
                TopicId = bbs.TopicId,
                BoardId = bbs.BoardId,
                BBSId = bbsId,
                ReplyContent = bbs.Content
            };

            return View("GetQAReply", model);
        }

        /// <summary>
        /// 保存修改正确答案
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetEditAnswerSave(ReplyMode model)
        {
            try
            {
                var user = (UserView)HttpContext.Session["Admin"];
                model.PostUserId = user.UserId;
                OperationResult result = BBSContract.EditAnswerSave(model);
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
