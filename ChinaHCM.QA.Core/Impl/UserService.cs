using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;
using System.Data.Objects.SqlClient;

using ChinaHCM.QA.Core.Data;
using ChinaHCM.QA.Core.Model;
using ChinaHCM.QA.Component.Tools;

namespace ChinaHCM.QA.Core
{
    /// <summary>
    ///     账户模块核心业务实现
    /// </summary>
    public class UserService : CoreServiceBase, IUserService
    {
        #region 属性

        #region 受保护的属性

        /// <summary>
        /// 获取或设置 用户信息数据访问对象
        /// </summary>
        [Import]
        protected IUserRepository UserRepository { get; private set; }

        /// <summary>
        /// 获取或设置 版区信息数据访问对象
        /// </summary>
        [Import]
        protected IBoardRepository BoardRepository { get; private set; }

        #endregion

        #region 公共属性

        /// <summary>
        /// 获取 用户信息查询数据集
        /// </summary>
        public IQueryable<e_Users> Users
        {
            get { return UserRepository.Entities; }
        }

        /// <summary>
        /// 获取 版区信息查询数据集
        /// </summary>
        public IQueryable<e_Board> Boards
        {
            get { return BoardRepository.Entities; }
        }

        #endregion

        #endregion

        /// <summary>
        /// 获得全部用户
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<e_Users> GetUsers()
        {
            return Users;
        }

        /// <summary>
        /// 获得全部用户（分页）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public virtual IEnumerable<e_Users> GetUsers(int pageIndex, int pageSize, out int recordCount)
        {
            return Users.Where(u => true, pageIndex, pageSize, out recordCount, u => u.UserId);
        }

        /// <summary>
        /// 获得管理员用户（分页）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public virtual IEnumerable<e_Users> GetAdmin(int pageIndex, int pageSize, out int recordCount)
        {
            return Users.Where(u => u.Level > (int)UserLevel.Student && u.Level < (int)UserLevel.SuperAdmin && u.Status == (int)Status.Normal, pageIndex, pageSize, out recordCount, u => u.UserId);
        }

        /// <summary>
        /// 获得普通用户（分页）
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public virtual IEnumerable<e_Users> GetNormalUsers(int pageIndex, int pageSize, out int recordCount)
        {
            return Users.Where(u => u.Level <= (int)UserLevel.Student && u.Status == (int)Status.Normal, pageIndex, pageSize, out recordCount, u => u.UserId);
        }

        /// <summary>
        /// 获得指定用户
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public virtual e_Users GetUser(int userid)
        {
            if (UserRepository.Entities.Count(u => u.UserId == userid && u.Status == (int)Status.Normal) == 0)
                return null;

            return UserRepository.GetByKey(userid);
        }

        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsExistUserName(string userName)
        {
            return Users.Count(u => string.Compare(u.LoginName, userName, true) == 0) > 0;
        }
        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsExistUserName(string userName, int userId)
        {
            return Users.Count(u => string.Compare(u.LoginName, userName, true) == 0 && u.UserId != userId) > 0;
        }

        /// <summary>
        /// 判断昵称是否存在
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public bool IsExistNickName(string nickName)
        {
            return Users.Count(u => string.Compare(u.NickName, nickName, true) == 0) > 0;
        }
        /// <summary>
        /// 判断昵称是否存在
        /// </summary>
        /// <param name="nickName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsExistNickName(string nickName, int userId)
        {
            return Users.Count(u => string.Compare(u.NickName, nickName, true) == 0 && u.UserId != userId) > 0;
        }

        /// <summary>
        /// 判断密码是否正确
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsRightPassword(string Password, int userId)
        {
            return Users.Count(u => string.Compare(u.Password, Password, false) == 0 && u.UserId == userId) == 1;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginInfo">登录信息</param>
        /// <returns>业务操作结果</returns>
        public virtual OperationResult Login(LoginInfo loginInfo)
        {
            var _user = Users.SingleOrDefault(m => m.LoginName == loginInfo.LoginName && m.Level <= (int)UserLevel.Student && m.Status == (int)Status.Normal);
            if (_user == null)
            {
                return new OperationResult(OperationResultType.QueryNull, "指定账号的用户不存在。");
            }
            if (_user.Password != loginInfo.Password)
            {
                return new OperationResult(OperationResultType.Warning, "登录密码不正确。");
            }
            _user.LoginCnt++;
            _user.LastLoginTime = DateTime.Now;
            UserRepository.Update(_user);

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);
            return new OperationResult(OperationResultType.Success, "登录成功。", _user);
        }

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="loginInfo">登录信息</param>
        /// <returns>业务操作结果</returns>
        public virtual OperationResult AdminLogin(LoginInfo loginInfo)
        {
            var _user = Users.SingleOrDefault(m => m.LoginName == loginInfo.LoginName && m.Level > (int)UserLevel.Student && m.Status == (int)Status.Normal);
            if (_user == null)
            {
                return new OperationResult(OperationResultType.QueryNull, "指定账号的用户不存在。");
            }
            if (_user.Password != loginInfo.Password)
            {
                return new OperationResult(OperationResultType.Warning, "登录密码不正确。");
            }
            _user.LoginCnt++;
            _user.LastLoginTime = DateTime.Now;
            UserRepository.Update(_user);

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);
            return new OperationResult(OperationResultType.Success, "登录成功。", _user);
        }

        /// <summary>
        ///  用户注册
        /// </summary>
        /// <param name="model">注册模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Register(UserInfo model)
        {
            if (Users.Count(m => m.LoginName == model.LoginName) > 0)
            {
                return new OperationResult(OperationResultType.Error, "用户名已经存在。");
            }
            if (Users.Count(m => m.NickName == model.NickName) > 0)
            {
                return new OperationResult(OperationResultType.Error, "用户昵称已经存在。");
            }
            e_Users _user = new e_Users()
            {
                LoginName = model.LoginName,
                Password = model.Password,
                NickName = model.NickName,
                RealName = model.RealName,
                Gender = model.Gender,
                UserEmail = model.UserEmail,
                UserQuesion = model.UserQuesion,
                UserAnswer = model.UserAnswer,
                AddTime = DateTime.Now,
                LastLoginTime = DateTime.Now,
                AddIp = model.IpAddress
            };
            UserRepository.Insert(_user);

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);
            return new OperationResult(OperationResultType.Success, "登录成功。", _user);
        }

        /// <summary>
        ///  删除用户
        /// </summary>
        /// <param name="userIds">用户ID串</param>
        /// <returns>业务操作结果</returns>
        public OperationResult Delete(IEnumerable<int> userIds)
        {
            foreach (int userId in userIds)
            {
                e_Users user = GetUser(userId);
                user.Status = (int)Status.Delete;

                List<int> bids = new List<int>();

                foreach (e_Board b in Boards)
                {
                    if (!string.IsNullOrEmpty(b.Master))
                    {
                        if (b.Master.Split(new char[] { ',' }).ToList<string>().Select(s => Convert.ToInt32(s)).Count(s => s == userId) > 0)
                        {
                            bids.Add(b.BoardId);
                        }
                    }
                }

                if (bids.Count > 0)
                {
                    //Foreach时候确保数据库事务需要将IQueryable转换成为List
                    foreach (e_Board board in Boards.Where(b => bids.Contains(b.BoardId)).ToList<e_Board>())
                    {
                        board.Master = string.Format(",{0},", board.Master).Replace(string.Format("{0},", userId), "").Trim(new char[] { ',' });
                        BoardRepository.Update(board, false);
                    }
                }

                UserRepository.Update(user);
            }

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);

            return new OperationResult(OperationResultType.Success, "删除成功。", null);
        }

        /// <summary>
        ///  用户维护
        /// </summary>
        /// <param name="model">用户模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult EditUser(UserInfo model)
        {
            if ((model.UserId == 0 && Users.Count(m => m.LoginName == model.LoginName) > 0) ||
                (model.UserId > 0 && Users.Count(m => m.LoginName == model.LoginName && m.UserId != model.UserId) > 0))
            {
                return new OperationResult(OperationResultType.Error, "用户名已经存在。");
            }
            if ((model.UserId == 0 && Users.Count(m => m.NickName == model.NickName) > 0) ||
                (model.UserId > 0 && Users.Count(m => m.NickName == model.NickName && m.UserId != model.UserId) > 0))
            {
                return new OperationResult(OperationResultType.Error, "用户昵称已经存在。");
            }

            e_Users _user;
            if (model.UserId == 0)
            {
                _user = new e_Users()
                {
                    LoginName = model.LoginName,
                    Password = model.Password,
                    Level = (int)model.Level,
                    NickName = model.NickName,
                    RealName = model.RealName,
                    Gender = model.Gender,
                    UserEmail = model.UserEmail,
                    UserQuesion = string.Empty,
                    UserAnswer = string.Empty,
                    AddTime = DateTime.Now,
                    LastLoginTime = DateTime.Now,
                    AddIp = model.IpAddress
                };
                UserRepository.Insert(_user);
            }
            else
            {
                _user = GetUser(model.UserId);

                _user.LoginName = model.LoginName;
                _user.Password = model.Password;
                _user.Level = (int)model.Level;
                _user.NickName = model.NickName;
                _user.RealName = model.RealName;
                _user.Gender = model.Gender;
                _user.UserEmail = model.UserEmail;

                UserRepository.Update(_user);
            }

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);

            return new OperationResult(OperationResultType.Success, "用户编辑成功。", _user);
        }


        /// <summary>
        ///  密码修改
        /// </summary>
        /// <param name="model">用户模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult ChangePassword(UserInfo model)
        {
            var _user = GetUser(model.UserId);

            _user.Password = model.Password;

            UserRepository.Update(_user);

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);

            return new OperationResult(OperationResultType.Success, "密码修改成功。", _user);
        }
    }
}
