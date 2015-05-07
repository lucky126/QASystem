using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaHCM.QA.Core.Model
{
    /// <summary>
    /// 常量定义
    /// </summary>
    public class GlobalConst
    {
        /// <summary>
        /// 问答筛选类型文字表述
        /// </summary>
        public static string[] QATypeName = { "精华", "已解决", "未解决","抢答" };

        /// <summary>
        /// 发帖时间搜索类型
        /// </summary>
        public static string[] PostTimeTypeName = { "最近一周", "最近一个月", "最近三个月", "最近半年" };

        /// <summary>
        /// 搜索类型
        /// </summary>
        public static string[] SearchTypeName = { "标题和关键字", "问题描述", "最佳回复" };

        /// <summary>
        /// 版区类型文字表述
        /// </summary>
        public static string[] BoardTypeName = { "无", "问答", "讨论" };

        /// <summary>
        /// 模式文字表述
        /// </summary>
        public static string[] ModeName = { "所有人可见", "仅发帖人可见" };

        /// <summary>
        /// 用户等级文字描述
        /// </summary>
        public static string[] LevelName = { "注册用户", "学员", "教师", "管理员" };

         /// <summary>
        /// 性别文字描述
        /// </summary>
        public static string[] GenderName = { "男", "女" };        
    }
}
