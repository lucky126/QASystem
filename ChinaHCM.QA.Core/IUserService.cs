using System;
using System.Collections.Generic;
using System.Linq;

using ChinaHCM.QA.Core.Model;
using ChinaHCM.QA.Component.Tools;

namespace ChinaHCM.QA.Core
{
    /// <summary>
    ///  账户模块核心业务契约
    /// </summary>
    public interface IUserService
    {
        #region 属性

        /// <summary>
        /// 获取 用户信息查询数据集
        /// </summary>
        IQueryable<e_Users> Users { get; }

        #endregion

        /// <summary>
        /// 获得全部用户
        /// </summary>
        /// <returns></returns>
        IEnumerable<e_Users> GetUsers();

        /// <summary>
        /// 获得全部用户（分页）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        IEnumerable<e_Users> GetUsers(int pageIndex, int pageSize, out int recordCount);

        /// <summary>
        /// 获得管理员用户（分页）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        IEnumerable<e_Users> GetAdmin(int pageIndex, int pageSize, out int recordCount);

        /// <summary>
        /// 获得普通用户（分页）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        IEnumerable<e_Users> GetNormalUsers(int pageIndex, int pageSize, out int recordCount);

        /// <summary>
        /// 获得指定用户
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        e_Users GetUser(int userid);

        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool IsExistUserName(string userName);
        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool IsExistUserName(string userName, int userId);

        /// <summary>
        /// 判断昵称是否存在
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        bool IsExistNickName(string nickName);
        /// <summary>
        /// 判断昵称是否存在
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool IsExistNickName(string nickName, int userId);

        /// <summary>
        /// 判断密码是否正确
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool IsRightPassword(string Password, int userId);

        /// <summary>
        ///   用户登录
        /// </summary>
        /// <param name="loginInfo">登录信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult Login(LoginInfo loginInfo);

        /// <summary>
        ///  管理员登录
        /// </summary>
        /// <param name="loginInfo">登录信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult AdminLogin(LoginInfo loginInfo);

        /// <summary>
        ///  用户注册
        /// </summary>
        /// <param name="model">注册模型信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult Register(UserInfo model);

        /// <summary>
        ///  删除用户
        /// </summary>
        /// <param name="userIds">用户ID串</param>
        /// <returns>业务操作结果</returns>
        OperationResult Delete(IEnumerable<int> userIds);

        /// <summary>
        ///  用户维护
        /// </summary>
        /// <param name="model">用户模型信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult EditUser(UserInfo model);

        /// <summary>
        ///  密码修改
        /// </summary>
        /// <param name="model">用户模型信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult ChangePassword(UserInfo model);
    }
}
