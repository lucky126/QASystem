using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using ChinaHCM.QA.Site.Model;
using ChinaHCM.QA.Site.Web.Models;
using ChinaHCM.QA.Component.Tools;

namespace ChinaHCM.QA.Site.Web.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountController : BaseController
    {
        #region 属性

        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        public IAccountSiteContract AccountContract { get; private set; }

        #endregion

        #region 视图功能
        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]   //清除缓存
        public JsonResult CheckUserName(string userName)
        {
            bool valid = false;
            if (!AccountContract.IsExistUserName(userName))
                valid = true;

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 判断昵称是否存在
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]   //清除缓存
        public JsonResult CheckNickName(string nickName)
        {
            bool valid = false;
            if (!AccountContract.IsExistNickName(nickName))
                valid = true;

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ViewResult Login()
        {
            string returnUrl = Request.Params["returnUrl"];
            returnUrl = returnUrl ?? Url.RouteUrl("Home");
            LoginModel model = new LoginModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]        
        public JsonResult Login(LoginModel model)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    OperationResult result = AccountContract.Login(model);
                    string msg = result.Message ?? result.ResultType.ToDescription();
                    if (result.ResultType == OperationResultType.Success)
                    {
                        JsonReturn data = new JsonReturn() { Flag = 0, ErrorMessage = string.Empty, ReturlUrl = model.ReturnUrl };
                        return Json(data);
                    }
                    ModelState.AddModelError("", msg);
                    JsonReturn data2 = new JsonReturn() { Flag = 1, ErrorMessage = msg, ReturlUrl = string.Empty };
                    return Json(data2);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                    JsonReturn data = new JsonReturn() { Flag = 1, ErrorMessage = e.Message, ReturlUrl = string.Empty };
                    return Json(data);
                }
            }
            else
            {
                return Json(null);
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Logout()
        {
            string returnUrl = Request.Params["returnUrl"];
            returnUrl = returnUrl ?? Url.RouteUrl("Home");
            if (Request.IsAuthenticated)
            {
                AccountContract.Logout();
            }

            Notice _n = new Notice { Title = "成功退出", Details = "您已经成功退出！", DwellTime = 5, NavigationName = "网站首页", NavigationUrl = returnUrl };
            return RedirectToAction("Notice", "Prompt", _n);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public ViewResult Register()
        {
            string returnUrl = Request.Params["returnUrl"];
            returnUrl = returnUrl ?? Url.RouteUrl("Home");
            RegisterModel model = new RegisterModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        /// <summary>
        /// 注册验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken()]
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            try
            {
                OperationResult result = AccountContract.Register(model);
                string msg = result.Message ?? result.ResultType.ToDescription();
                if (result.ResultType == OperationResultType.Success)
                {
                    return Redirect(model.ReturnUrl);
                }
                ModelState.AddModelError("", msg);
                return View(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);
            }
        }

        /// <summary>
        /// 得到用户昵称
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public PartialViewResult GetNickName(int userId)
        {
            UserView user = AccountContract.GetUser(userId);

            return PartialView("PartialNickName", user);
        }
        
        /// <summary>
        /// 显示用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult GetUser(int userId)
        {
            UserView user = AccountContract.GetUser(userId);

            return View("User",user);
        }
        #endregion
    }
}
