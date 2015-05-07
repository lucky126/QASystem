using System;
using System.Collections.Generic;
using System.Linq;

using ChinaHCM.QA.Core;
using ChinaHCM.QA.Component.Tools;
using ChinaHCM.QA.Site.Model;
using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Site
{
    /// <summary>
    ///     账户模块站点业务契约
    /// </summary>
    public interface IAccountSiteContract : IUserService
    {
        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        new bool IsExistUserName(string userName);
        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        new bool IsExistUserName(string userName, int userId);

        /// <summary>
        /// 判断昵称是否存在
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        new bool IsExistNickName(string nickName);
        /// <summary>
        /// 判断昵称是否存在
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        new bool IsExistNickName(string nickName, int userId);

        /// <summary>
        /// 判断密码是否正确
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        new bool IsRightPassword(string Password, int userId);

        /// <summary>
        ///   用户登录
        /// </summary>
        /// <param name="model">登录模型信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult Login(LoginModel model);

        /// <summary>
        ///   管理员登录
        /// </summary>
        /// <param name="model">登录模型信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult AdminLogin(LoginModel model);

        /// <summary>
        ///   用户退出
        /// </summary>
        void Logout();

        /// <summary>
        ///   用户退出
        /// </summary>
        void AdminLogout();

        /// <summary>
        ///  用户注册
        /// </summary>
        /// <param name="model">注册模型信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult Register(RegisterModel model);

        /// <summary>
        /// 得到用户信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        new UserView GetUser(int userId);

        /// <summary>
        /// 得到所有指定类型用户列表
        /// </summary>
        /// <param name="userLevel">用户类型</param>
        /// <returns></returns>
        IList<UserView> GetUserList(UserLevel userLevel);

        /// <summary>
        /// 获得管理员用户（分页）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        new IList<UserView> GetAdmin(int pageIndex, int pageSize, out int recordCount);

        /// <summary>
        /// 获得管理员用户（分页）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        new IList<UserView> GetNormalUsers(int pageIndex, int pageSize, out int recordCount);

        /// <summary>
        ///  删除用户
        /// </summary>
        /// <param name="userIds">用户D串</param>
        /// <returns>业务操作结果</returns>
        new OperationResult Delete(IEnumerable<int> userIds);

        /// <summary>
        ///  用户编辑
        /// </summary>
        /// <param name="model">用户模型信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult EditUser(UserModel model);

        /// <summary>
        ///  密码修改
        /// </summary>
        /// <param name="model">密码修改模型信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult ChangePassword(ChangePasswordModel model);
    }
}
