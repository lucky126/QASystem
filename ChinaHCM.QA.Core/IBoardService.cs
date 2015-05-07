using System;
using System.Collections.Generic;
using System.Linq;

using ChinaHCM.QA.Core.Model;
using ChinaHCM.QA.Component.Tools;

namespace ChinaHCM.QA.Core
{
    /// <summary>
    ///     版区模块核心业务契约
    /// </summary>
    public interface IBoardService
    {
        #region 属性

        /// <summary>
        /// 获取 版区信息查询数据集
        /// </summary>
        IQueryable<e_Board> Boards { get; }

        #endregion

        /// <summary>
        /// 得到全部版区
        /// </summary>
        /// <returns></returns>
        IEnumerable<e_Board> GetBoards();

        /// <summary>
        /// 得到根版区
        /// </summary>
        /// <param name="onlyGetPublic">是否只得到公共版区</param>
        /// <returns></returns>
        IEnumerable<e_Board> GetParentBoards(bool onlyGetPublic);

        /// <summary>
        /// 根据父版区ID得到子版区
        /// </summary>
        /// <param name="parentId">父版区ID</param>
        /// <returns></returns>
        IEnumerable<e_Board> GetBoardsByParentID(int parentId);

        /// <summary>
        /// 得到指定版区
        /// </summary>
        /// <param name="boardId">版区ID</param>
        /// <returns></returns>
        e_Board GetBoard(int boardId);

        /// <summary>
        /// 判断是否可以访问版区
        /// </summary>
        /// <param name="boardId">版区ID</param>
        /// <param name="onlyGetPublic">是否只可以访问公共版区</param>
        /// <returns></returns>
        bool CanVisitedBoard(int boardId, bool onlyGetPublic);

        /// <summary>
        ///  保存操作
        /// </summary>
        /// <param name="model">版区模型信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult SaveBoard(BoardInfo model);
    }
}
