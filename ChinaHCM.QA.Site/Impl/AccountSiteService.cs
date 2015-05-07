using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.ComponentModel.Composition;

using ChinaHCM.QA.Core;
using ChinaHCM.QA.Component.Tools;
using ChinaHCM.QA.Core.Model;
using ChinaHCM.QA.Site.Model;
using ChinaHCM.QA.Core.Data;

namespace ChinaHCM.QA.Site
{
    /// <summary>
    ///  账户模块站点业务实现
    /// </summary>
    [Export(typeof(IAccountSiteContract))]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    internal class AccountSiteService : UserService, IAccountSiteContract
    {
        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public new bool IsExistUserName(string userName)
        {
            return base.IsExistUserName(userName);
        }
        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public new bool IsExistUserName(string userName, int userId)
        {
            return base.IsExistUserName(userName, userId);
        }

        /// <summary>
        /// 判断昵称是否存在
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public new bool IsExistNickName(string nickName)
        {
            return base.IsExistNickName(nickName);
        }
        /// <summary>
        /// 判断昵称是否存在
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public new bool IsExistNickName(string nickName, int userId)
        {
            return base.IsExistNickName(nickName, userId);
        }

        /// <summary>
        /// 判断密码是否正确
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public new bool IsRightPassword(string Password, int userId)
        {
            return base.IsRightPassword(Password, userId);
        }

        /// <summary>
        ///   用户登录
        /// </summary>
        /// <param name="model">登录模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Login(LoginModel model)
        {
            PublicHelper.CheckArgument(model, "model");
            LoginInfo loginInfo = new LoginInfo
            {
                LoginName = model.UserName,
                Password = model.Password,
                IpAddress = HttpContext.Current.Request.UserHostAddress
            };
            OperationResult result = base.Login(loginInfo);
            if (result.ResultType == OperationResultType.Success)
            {
                e_Users member = (e_Users)result.AppendData;

                DateTime expiration = model.IsRememberLogin
                    ? DateTime.Now.AddDays(7)
                    : DateTime.Now.Add(FormsAuthentication.Timeout);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, member.UserId.ToString(), DateTime.Now, expiration,
                    true, member.NickName, FormsAuthentication.FormsCookiePath);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
             
                if (model.IsRememberLogin)
                {
                    cookie.Expires = DateTime.Now.AddDays(7);
                }

                HttpContext.Current.Response.Cookies.Set(cookie);
                result.AppendData = null;
            }
            return result;
        }

        /// <summary>
        ///   管理员登录
        /// </summary>
        /// <param name="model">登录模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult AdminLogin(LoginModel model)
        {
            PublicHelper.CheckArgument(model, "model");
            LoginInfo loginInfo = new LoginInfo
            {
                LoginName = model.UserName,
                Password = model.Password,
                IpAddress = HttpContext.Current.Request.UserHostAddress
            };
            OperationResult result = base.AdminLogin(loginInfo);
            if (result.ResultType == OperationResultType.Success)
            {
                e_Users member = (e_Users)result.AppendData;

                HttpContext.Current.Session["Admin"] = UserView.FromUser(member);

                result.AppendData = null;
            }
            return result;
        }

        /// <summary>
        ///     用户退出
        /// </summary>
        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        ///     用户退出
        /// </summary>
        public void AdminLogout()
        {
            FormsAuthentication.SignOut();

            if (HttpContext.Current.Session["Admin"] != null)
            {
                HttpContext.Current.Session.Remove("Admin");
            }
        }

        /// <summary>
        ///  用户注册
        /// </summary>
        /// <param name="model">注册模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Register(RegisterModel model)
        {
            PublicHelper.CheckArgument(model, "model");

            UserInfo registerInfo = new UserInfo
            {
                LoginName = model.LoginName,
                Password = model.Password,
                NickName = model.NickName,
                RealName = model.RealName,
                Gender = GlobalConst.GenderName[(int)model.Gender],
                UserEmail = model.UserEmail,
                UserQuesion = model.UserQuesion,
                UserAnswer = model.UserAnswer,
                IpAddress = HttpContext.Current.Request.UserHostAddress
            };
            OperationResult result = base.Register(registerInfo);

            if (result.ResultType == OperationResultType.Success)
            {

            }
            return result;
        }

        /// <summary>
        /// 得到用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public new UserView GetUser(int userId)
        {
            PublicHelper.CheckArgument(userId, "userId");

            return UserView.FromUser(base.GetUser(userId));
        }

        /// <summary>
        /// 得到所有指定类型用户列表
        /// </summary>
        /// <param name="userLevel">用户类型</param>
        /// <returns></returns>
        public IList<UserView> GetUserList(UserLevel userLevel)
        {
            return base.GetUsers().Where(u => u.Level == (int)userLevel).Select(b => UserView.FromUser(b)).ToList<UserView>();
        }

        /// <summary>
        /// 获得管理员用户（分页）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public new IList<UserView> GetAdmin(int pageIndex, int pageSize, out int recordCount)
        {
            return base.GetAdmin(pageIndex, pageSize, out recordCount).Select(b => UserView.FromUser(b)).ToList<UserView>();
        }

        /// <summary>
        /// 获得管理员用户（分页）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public new IList<UserView> GetNormalUsers(int pageIndex, int pageSize, out int recordCount)
        {
            return base.GetNormalUsers(pageIndex, pageSize, out recordCount).Select(b => UserView.FromUser(b)).ToList<UserView>();
        }

        /// <summary>
        ///  删除用户
        /// </summary>
        /// <param name="userIds">用户D串</param>
        /// <returns>业务操作结果</returns>
        public new OperationResult Delete(IEnumerable<int> userIds)
        {
            PublicHelper.CheckArgument(userIds, "userIds");

            OperationResult result = base.Delete(userIds);

            if (result.ResultType == OperationResultType.Success)
            {
                result.AppendData = null;
            }

            return result;
        }

        /// <summary>
        ///  用户编辑
        /// </summary>
        /// <param name="model">用户模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult EditUser(UserModel model)
        {
            PublicHelper.CheckArgument(model, "model");

            UserInfo userInfo = new UserInfo
            {
                UserId = model.UserId,
                Level = model.Level,
                LoginName = model.LoginName,
                Password = model.Password,
                NickName = model.NickName,
                RealName = model.RealName,
                Gender = GlobalConst.GenderName[(int)model.Gender],
                UserEmail = model.UserEmail,
                IpAddress = HttpContext.Current.Request.UserHostAddress
            };
            OperationResult result = base.EditUser(userInfo);

            return result;
        }

        /// <summary>
        ///  密码修改
        /// </summary>
        /// <param name="model">密码修改模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult ChangePassword(ChangePasswordModel model)
        {
            PublicHelper.CheckArgument(model, "model");

            UserInfo userInfo = new UserInfo
            {
                UserId = model.UserId,
                Password = model.NewPassword
            };
            OperationResult result = base.ChangePassword(userInfo);

            return result;
        }
    }
}
