using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaHCM.QA.Site.Model
{
    /// <summary>
    /// 导航页面模型
    /// </summary>
    public class NavModel
    {
        /// <summary>
        /// 最后导航文字
        /// </summary>
        public string LastNav { get; set; }
        /// <summary>
        /// 主贴信息
        /// </summary>
        public TopicView Topic { get; private set; }
        /// <summary>
        /// 所属版区列表
        /// </summary>
        public IList<BoardView> Boards { get; private set; }
        private IList<BoardView> _boards;

        /// <summary>
        /// 构造函数（用户生成版区列表）
        /// </summary>
        /// <param name="boards">全部版区列表（需要考虑缓存问题）</param>
        /// <param name="board">当前版区</param>
        /// <param name="topic">当前帖子</param>
        public NavModel(IList<BoardView> boards, BoardView board, TopicView topic, string lastNav = "")
        {
            LastNav = lastNav;
            //赋值主贴（可能为空）
            Topic = topic;
            //建立临时存储版区列表，并且将当前版区添加
            List<BoardView> RetultBoards = new List<BoardView>();
            if (board != null)
            {
                RetultBoards.Add(board);

                //从全部版区列表中得到与当前版区同根的版区信息
                _boards = boards.Where(b => b.RootID == board.RootID).ToList<BoardView>();
                //得到当前版区的父版区
                BoardView parent = _boards.FirstOrDefault(b => b.BoardId == board.ParentID);
                //循环填充所有父版区直到父版区不存在
                while (parent != null)
                {
                    RetultBoards.Add(parent);
                    parent = GetParent(parent.ParentID);
                }
            }
            //返回版区列表
            Boards = RetultBoards.OrderBy(b => b.Depth).ToList<BoardView>();
        }

        /// <summary>
        /// 得到父版区
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        private BoardView GetParent(int boardId)
        {
            return _boards.FirstOrDefault(b => b.BoardId == boardId);
        }

        /// <summary>
        /// 得到子版区
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        private IEnumerable<BoardView> GetChild(int boardId)
        {
            return _boards.Where(b => b.ParentID == boardId);
        }
    }
}
