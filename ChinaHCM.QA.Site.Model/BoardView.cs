using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

using ChinaHCM.QA.Component.Data;
using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Site.Model
{
    public class BoardView
    {
        /// <summary>
        /// 版区ID
        /// </summary>
        public int BoardId { get; private set; }

        /// <summary>
        /// 版区名称
        /// </summary>
        [DisplayName("版区名称")]
        public string Name { get; private set; }

        /// <summary>
        /// 全局排序
        /// </summary>
        public int Order { get; private set; }

        /// <summary>
        /// 父版区ID
        /// </summary>
        public int ParentID { get; private set; }

        /// <summary>
        /// 根版区ID
        /// </summary>
        public int RootID { get; private set; }

        /// <summary>
        /// 深度
        /// </summary>
        public int Depth { get; private set; }

        /// <summary>
        /// 是否叶子节点
        /// </summary>
        public bool IsLeaf { get; private set; }

        /// <summary>
        /// 是否公共版区
        /// </summary>
        public bool IsPublic { get; private set; }

        /// <summary>
        /// 版区类型
        /// </summary>
        public BoardType BoardType { get; set; }

        /// <summary>
        /// 管理员列表
        /// </summary>
        [DisplayName("管理员")]
        public IEnumerable<int> Master { get; private set; }
        /// <summary>
        /// 管理员列表字符串
        /// </summary>
        public string MasterList { get; private set; }

        /// <summary>
        /// 发帖数
        /// </summary>
        [DisplayName("发帖数")]
        public int TopicNum { get; set; }

        /// <summary>
        /// 帖子数
        /// </summary>
        [DisplayName("帖子数")]
        public int PostNum { get; set; }

        /// <summary>
        /// 拥有的是否是分类
        /// </summary>
        public bool IsHaveSubBoard { get; set; }

        /// <summary>
        /// 是否可以删除
        /// </summary>
        public bool CanDelete { get; set; }

        public static BoardView FromBoard(e_Board board)
        {
            return new BoardView
            {
                BoardId = board.BoardId,
                Name = board.Name,
                Order = board.Order,
                Depth = board.Depth,
                ParentID = board.ParentID,
                RootID = board.RootID,
                IsLeaf = board.IsLeaf == Convert.ToInt32(YesOrNo.Yes),
                IsPublic = board.IsPublic == Convert.ToInt32(YesOrNo.Yes),
                BoardType = (BoardType)board.BoardType,
                MasterList = board.Master,
                Master = string.IsNullOrEmpty(board.Master) ? new List<int>() : board.Master.Split(new char[] { ',' }).Select(s => Convert.ToInt32(s)).ToList<int>(),
                TopicNum = board.TopicNum,
                PostNum = board.PostNum,
                IsHaveSubBoard = board.Depth > 0,
                CanDelete = board.IsLeaf == Convert.ToInt32(YesOrNo.Yes) && board.PostNum==0 && board.TopicNum==0
            };
        }
    }
}