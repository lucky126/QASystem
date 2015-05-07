using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;

using ChinaHCM.QA.Core.Data;
using ChinaHCM.QA.Core.Model;
using ChinaHCM.QA.Component.Data;
using ChinaHCM.QA.Component.Tools;

namespace ChinaHCM.QA.Core
{
    /// <summary>
    ///     版区模块核心业务实现
    /// </summary>
    public abstract class BoardService : CoreServiceBase, IBoardService
    {
        #region 属性

        #region 受保护的属性

        /// <summary>
        /// 获取或设置 版区信息数据访问对象
        /// </summary>
        [Import]
        protected IBoardRepository BoardRepository { get; private set; }

        #endregion

        #region 公共属性

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
        /// 得到全部版区
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<e_Board> GetBoards()
        {
            return Boards.OrderBy(p => p.Order);
        }

        /// <summary>
        /// 得到根版区
        /// </summary>
        /// <param name="onlyGetPublic">是否只得到公共版区</param>
        /// <returns></returns>
        public virtual IEnumerable<e_Board> GetParentBoards(bool onlyGetPublic)
        {
            if (onlyGetPublic)
                return Boards.Where(b => b.ParentID == 0 && b.IsPublic == (int)YesOrNo.Yes).OrderBy(p => p.Order);
            else
                return Boards.Where(b => b.ParentID == 0).OrderBy(p => p.Order);
        }

        /// <summary>
        /// 根据父版区ID得到子版区
        /// </summary>
        /// <param name="parentId">父版区ID</param>
        /// <returns></returns>
        public virtual IEnumerable<e_Board> GetBoardsByParentID(int parentId)
        {
            return Boards.Where(b => b.ParentID == parentId).OrderBy(b => b.Order);
        }

        /// <summary>
        /// 得到指定版区
        /// </summary>
        /// <param name="boardId">版区ID</param>
        /// <returns></returns>
        public virtual e_Board GetBoard(int boardId)
        {            
            return BoardRepository.GetByKey(boardId);
        }

        /// <summary>
        /// 判断是否可以访问版区
        /// </summary>
        /// <param name="boardId">版区ID</param>
        /// <param name="onlyGetPublic">是否只可以访问公共版区</param>
        /// <returns></returns>
        public virtual bool CanVisitedBoard(int boardId, bool onlyGetPublic)
        {
            e_Board board = GetBoard(boardId);
            if (board == null)
                return false;
            //如果只能访问公共版区的情况下，且当前版区不是公共版区，则返回不许可；否则返回许可
            if (onlyGetPublic && board.IsPublic == (int)YesOrNo.No)
                return false;
            else
                return true;
        }

        /// <summary>
        ///  保存操作
        /// </summary>
        /// <param name="model">版区模型信息</param>
        /// <returns>业务操作结果</returns>
        public OperationResult SaveBoard(BoardInfo model)
        {
            if (model.BoardId != 0 && Boards.Count(b => b.BoardId == model.BoardId) == 0)
                return new OperationResult(OperationResultType.Error, "版区不存在。");

            e_Board board;

            if (model.BoardId != 0)
            {
                board = BoardRepository.GetByKey(model.BoardId);
                board.Name = model.Name;
                board.IsPublic = model.IsPublic ? (int)YesOrNo.Yes : (int)YesOrNo.No;
                board.Master = model.MasterList;
                board.BoardType = (int)model.BoardType;

                BoardRepository.Update(board);
            }
            else
            {
                e_Board _parent;
                if (model.ParentID > 0)
                {
                    _parent = BoardRepository.GetByKey(model.ParentID);
                }
                else
                {
                    _parent = new e_Board()
                    {
                        Depth = -1,
                        RootID = 0
                    };
                }
                //新增的版区顺序号取所有同级中最大的加一
                int _maxOrder;
                if (BoardRepository.Entities.Count(m => m.ParentID == model.ParentID) > 0)
                {
                    _maxOrder = BoardRepository.Entities.Where(m => m.ParentID == model.ParentID).Max(m => m.Order);
                }
                else
                {
                    _maxOrder = _parent.Order;
                }
                //为此,所有比此顺序号大的均需要加一处理
                //IEnumerable<e_Board> boards = BoardRepository.Entities.Where(m => m.Order > _maxOrder);

                BoardRepository.DBSqlCommand("UPDATE e_Board SET [Order]=[Order]+1 WHERE [Order]>{0}", _maxOrder);
                
                //调整排序
                //if (boards.Count() > 0)
                //{
                //    foreach (e_Board b in boards.ToList<e_Board>())
                //    {
                //        b.Order++;
                //        BoardRepository.Update(b, false);
                //    }
                //}
                //更新父版区状态
                if (model.ParentID > 0)
                {
                    _parent.IsLeaf = (int)YesOrNo.No;
                    BoardRepository.Update(_parent, false);
                }
                //新增版区
                board = new e_Board()
                {
                    Name = model.Name,
                    IsPublic = model.IsPublic ? (int)YesOrNo.Yes : (int)YesOrNo.No,
                    Master = model.MasterList,
                    Depth = _parent.Depth + 1,
                    ParentID = model.ParentID,
                    RootID = _parent.RootID,
                    IsLeaf = (int)YesOrNo.Yes,
                    BoardType = (int)model.BoardType,
                    AddTime = DateTime.Now,
                    TopicNum = 0,
                    PostNum = 0,
                    ParentStr = string.Format("{0},{1}", _parent.ParentStr, model.ParentID),
                    Order = _maxOrder + 1
                };
                BoardRepository.Insert(board);                
            }

            //LoginLog loginLog = new LoginLog { IpAddress = loginInfo.IpAddress, Member = member };
            //LoginLogRepository.Insert(loginLog);

            return new OperationResult(OperationResultType.Success, "版区维护成功。", null);
        }
    }
}
