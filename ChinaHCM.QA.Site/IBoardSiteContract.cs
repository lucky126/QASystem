using System;
using System.Collections.Generic;
using System.Linq;

using ChinaHCM.QA.Core;
using ChinaHCM.QA.Site.Model;
using ChinaHCM.QA.Component.Tools;

namespace ChinaHCM.QA.Site
{
    /// <summary>
    ///     版区模块站点业务契约
    /// </summary>
    public interface IBoardSiteContract : IBoardService
    {
        /// <summary>
        /// 得到全部版区
        /// </summary>
        /// <returns></returns>
        new IList<BoardView> GetBoards();

        /// <summary>
        /// 得到根版区
        /// </summary>
        /// <param name="onlyGetPublic">是否只得到公共版区</param>
        /// <returns></returns>
        new IList<BoardView> GetParentBoards(bool onlyGetPublic);

        /// <summary>
        /// 根据父版区ID得到子版区
        /// </summary>
        /// <param name="parentId">父版区ID</param>
        /// <returns></returns>
        new IList<BoardView> GetBoardsByParentID(int parentId);

        /// <summary>
        /// 得到指定版区
        /// </summary>
        /// <param name="boardId">版区ID</param>
        /// <returns></returns>
        new BoardView GetBoard(int boardId);

        /// <summary>
        /// 判断是否可以访问版区
        /// </summary>
        /// <param name="boardId">版区ID</param>
        /// <param name="onlyGetPublic">是否只可以访问公共版区</param>
        /// <returns></returns>
        new bool CanVisitedBoard(int boardId, bool onlyGetPublicbOnlyGetPublic);

        /// <summary>
        ///  保存操作
        /// </summary>
        /// <param name="model">版区模型信息</param>
        /// <returns>业务操作结果</returns>
        OperationResult SaveBoard(BoardModel model);
    }
}
