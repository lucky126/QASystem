using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Collections.Generic;
using System.ComponentModel.Composition;

using ChinaHCM.QA.Site.Model;
using ChinaHCM.QA.Site.Web.Models;
using ChinaHCM.QA.Component.Tools;
using ChinaHCM.QA.Site.Web.Controllers;
using ChinaHCM.QA.Site.Web.Extensions;
using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Site.Web.Areas.Manager.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountController : BaseController
    {
        #region 属性

        [Import(RequiredCreationPolicy = CreationPolicy.NonShared)]
        public IAccountSiteContract AccountContract { get; private set; }

        #endregion

        /// <summary>
        ///  后台登录
        /// </summary>
        /// <returns></returns>
        public ViewResult Login()
        {
            string returnUrl = Request.Params["returnUrl"];
            returnUrl = returnUrl ?? Url.Action("Index", "Board");
            LoginModel model = new LoginModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="model">登录模型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            try
            {
                OperationResult result = AccountContract.AdminLogin(model);
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
        /// 注销
        /// </summary>
        /// <returns></returns>
        [AdminAuthorize]
        public ActionResult Logout()
        {
            string returnUrl = Request.Params["returnUrl"];
            returnUrl = returnUrl ?? Url.Action("Login");
            AccountContract.AdminLogout();
            return Redirect(returnUrl);
        }

        /// <summary>
        /// 得到用户昵称
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AdminAuthorize]
        public PartialViewResult GetNickName(int userId)
        {
            UserView user = AccountContract.GetUser(userId);

            return PartialView("PartialNickName", user);
        }

        /// <summary>
        /// 返回用户列表
        /// </summary>
        /// <param name="isAll">是否全部</param>
        /// <param name="userList"></param>
        /// <returns></returns>
        [AdminAuthorize]
        public PartialViewResult GetUser(bool isAll, string userList = "")
        {
            IEnumerable<UserView> model = AccountContract.GetUserList(UserLevel.Teacher);

            if (!isAll)
            {
                if (string.IsNullOrEmpty(userList))
                {
                    model = new List<UserView>();
                }
                else
                {
                    var _list = userList.Split(new char[] { ',' }).Select(s => Convert.ToInt32(s)).ToList<int>();
                    model = AccountContract.GetUserList(UserLevel.Teacher).Where(u => _list.Contains(u.UserId));
                }
            }

            return PartialView("UserList", model);
        }

        /// <summary>
        /// 管理员管理列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [AdminAuthorize]
        public ViewResult AdminManager(int pageIndex = 1)
        {
            int recordCount;

            IEnumerable<UserView> model = AccountContract.GetAdmin(pageIndex, PagingInfo.PageSize, out recordCount);

            Func<int, UrlHelper, string> pageUrlAccessor =
              (currentPage, helper) => helper.Action("AdminManager", "Account", new { PageIndex = currentPage }).ToString();

            ViewBag.TitleName = "管理员管理";
            ViewBag.UserType = "Admin";

            return RenderUserList(model, recordCount, pageIndex, pageUrlAccessor);
        }

        /// <summary>
        /// 用户管理列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [AdminAuthorize]
        public ViewResult UserManager(int pageIndex = 1)
        {
            int recordCount;

            IEnumerable<UserView> model = AccountContract.GetNormalUsers(pageIndex, PagingInfo.PageSize, out recordCount);

            Func<int, UrlHelper, string> pageUrlAccessor =
              (currentPage, helper) => helper.Action("UserManager", "Account", new { PageIndex = currentPage }).ToString();

            ViewBag.TitleName = "用户管理";
            ViewBag.UserType = "User";

            return RenderUserList(model, recordCount, pageIndex, pageUrlAccessor);
        }

        /// <summary>
        /// 得到用户分页分部视图
        /// </summary>
        /// <param name="users"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageUrlAccessor"></param>
        /// <returns></returns>
        [AdminAuthorize]
        private ViewResult RenderUserList(IEnumerable<UserView> users, int recordCount, int pageIndex, Func<int, UrlHelper, string> pageUrlAccessor)
        {
            ViewResult result = View("UserManager", users);
            var _usr = (UserView)HttpContext.Session["Admin"];

            ViewBag.CurrentUser = _usr;
            ViewBag.RecordCount = recordCount;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageUrlAccessor = pageUrlAccessor;
            return result;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminAuthorize]
        public ActionResult Delete(string[] del)
        {
            //得到需要删除的TopicID
            IEnumerable<int> model = del.Where(d => string.Compare(d, "false", true) != 0).Select(d => Convert.ToInt32(d)).ToList<int>();
            //提交操作
            OperationResult result = AccountContract.Delete(model);
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
        /// 查阅用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AdminAuthorize]
        public ActionResult GetUserInfo(int userId)
        {
            UserView model = AccountContract.GetUser(userId);

            if (model == null)
            {
                Error _e = new Error
                {
                    Title = "错误",
                    Details = "指定的用户不存在",
                    Cause = Server.UrlEncode("您访问的用户可能已经被删除"),
                    Solution = Server.UrlEncode("<li>返回<a href='" + Url.Action("Index", "Board") + "'>网站首页</a></li>")
                };
                return RedirectToAction("Error", "Prompt", _e);
            }

            return View(model);
        }

        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [AdminAuthorize]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]   //清除缓存
        public JsonResult CheckUserName(string LoginName, int UserId)
        {
            bool valid = false;
            if (!AccountContract.IsExistUserName(LoginName, UserId))
                valid = true;

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 判断昵称是否存在
        /// </summary>
        /// <param name="NickName"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [AdminAuthorize]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]   //清除缓存
        public JsonResult CheckNickName(string NickName, int UserId)
        {
            bool valid = false;
            if (!AccountContract.IsExistNickName(NickName, UserId))
                valid = true;

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新增管理员
        /// </summary>
        /// <returns></returns>
        [AdminAuthorize]
        public ActionResult AddUser()
        {
            UserModel model = new UserModel()
            {
                IsNewUser = true,
                Level = UserLevel.Teacher
            };

            return View("EditUser", model);
        }

        /// <summary>
        /// 修改管理员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AdminAuthorize]
        public ActionResult EditUser(int userId = 0)
        {
            UserView _user = AccountContract.GetUser(userId);

            if (_user == null)
            {
                Error _e = new Error
                {
                    Title = "错误",
                    Details = "指定的用户不存在",
                    Cause = Server.UrlEncode("您访问的用户可能已经被删除"),
                    Solution = Server.UrlEncode("<li>返回<a href='" + Url.Action("Index", "Board") + "'>网站首页</a></li>")
                };
                return RedirectToAction("Error", "Prompt", _e);
            }

            UserModel model = new UserModel()
            {
                UserId = _user.UserId,
                LoginName = _user.LoginName,
                Password = _user.Password,
                NickName = _user.NickName,
                RealName = _user.RealName,
                Level = _user.Level,
                Gender = (Gender)GlobalConst.GenderName.Select((str, index) => new { str, index }).Single(s => s.str == _user.Gender).index,
                UserEmail = _user.UserEmail,
                IsNewUser = false
            };

            return View(model);
        }


        /// <summary>
        /// 保存管理员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AdminAuthorize]
        [HttpPost]
        public ActionResult EditUser(UserModel model)
        {
            try
            {
                OperationResult result = AccountContract.EditUser(model);
                string msg = result.Message ?? result.ResultType.ToDescription();
                if (result.ResultType == OperationResultType.Success)
                {
                    return RedirectToAction("AdminManager");
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
        /// 判断密码是否正确
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [AdminAuthorize]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]   //清除缓存
        public JsonResult CheckPassword(string Password, int UserId)
        {
            bool valid = false;
            if (AccountContract.IsRightPassword(Password, UserId))
                valid = true;

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改密码视图
        /// </summary>
        /// <returns></returns>
        [AdminAuthorize]
        public ViewResult ChangePassword()
        {
            var _user = (UserView)HttpContext.Session["Admin"];

            ChangePasswordModel model = new ChangePasswordModel()
            {
                UserId = _user.UserId
            };
            return View(model);
        }

        /// <summary>
        /// 保存修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AdminAuthorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            try
            {
                OperationResult result = AccountContract.ChangePassword(model);
                string msg = result.Message ?? result.ResultType.ToDescription();
                if (result.ResultType == OperationResultType.Success)
                {
                    return RedirectToAction("main","Board");
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
    }
}
