using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;

using ChinaHCM.QA.Core;
using ChinaHCM.QA.Site.Model;
using ChinaHCM.QA.Core.Data;
using ChinaHCM.QA.Component.Tools;
using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Site
{
    /// <summary>
    ///  账户模块站点业务实现
    /// </summary>
    [Export(typeof(IBoardSiteContract))]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    internal class BoardSiteService : BoardService, IBoardSiteContract
    {
        /// <summary>
        /// 得到全部版区
        /// </summary>
        /// <returns></returns>
        public new IList<BoardView> GetBoards()
        {
            return base.GetBoards().Select(b => BoardView.FromBoard(b)).ToList<BoardView>();
        }

        /// <summary>
        /// 得到根版区
        /// </summary>
        /// <param name="onlyGetPublic">是否只得到公共版区</param>
        /// <returns></returns>
        public new IList<BoardView> GetParentBoards(bool onlyGetPublic)
        {
            return base.GetParentBoards(onlyGetPublic).Select(b => BoardView.FromBoard(b)).ToList<BoardView>();
        }

        /// <summary>
        /// 根据父版区ID得到子版区
        /// </summary>
        /// <param name="parentId">父版区ID</param>
        /// <returns></returns>
        public new IList<BoardView> GetBoardsByParentID(int parentId)
        {
            PublicHelper.CheckArgument(parentId, "parentId", true);

            return base.GetBoardsByParentID(parentId).Select(b => BoardView.FromBoard(b)).ToList<BoardView>();
        }

        /// <summary>
        /// 得到指定版区
        /// </summary>
        /// <param name="boardId">版区ID</param>
        /// <returns></returns>
        public new BoardView GetBoard(int boardId)
        {
            PublicHelper.CheckArgument(boardId, "boardId");

            if (base.GetBoard(boardId) == null)
                return null;

            return BoardView.FromBoard(base.GetBoard(boardId));
        }

        /// <summary>
        /// 判断是否可以访问版区
        /// </summary>
        /// <param name="boardId">版区ID</param>
        /// <param name="onlyGetPublic">是否只可以访问公共版区</param>
        /// <returns></returns>
        public new bool CanVisitedBoard(int boardId, bool onlyGetPublic)
        {
            PublicHelper.CheckArgument(boardId, "boardId");

            return base.CanVisitedBoard(boardId, onlyGetPublic);
        }

        /// <summary>
        ///  保存操作
        /// </summary>
        /// <param name="model">版区模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult SaveBoard(BoardModel model)
        {
            PublicHelper.CheckArgument(model, "model");

            BoardInfo boardInfo = new BoardInfo
            {
                BoardId = model.BoardId,
                Name=model.Name,
                ParentID = model.ParentID,
                IsPublic = model.IsPublic,
                MasterList = string.IsNullOrEmpty(model.MasterList) ? string.Empty : model.MasterList,
                BoardType = model.BoardType
            };

            OperationResult result = base.SaveBoard(boardInfo);

            if (result.ResultType == OperationResultType.Success)
            {
                result.AppendData = null;
            }

            return result;
        }
    }
}
