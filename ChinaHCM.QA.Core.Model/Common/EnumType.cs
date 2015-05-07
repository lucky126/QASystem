using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaHCM.QA.Core.Model
{
    /// <summary>
    /// 是/否
    /// </summary>
    public enum YesOrNo
    {
        /// <summary>
        /// 否
        /// </summary>
        No = 0,
        /// <summary>
        /// 是
        /// </summary>
        Yes = 1
    }

    /// <summary>
    /// 状态
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 1
    }

    /// <summary>
    /// 版区类型
    /// </summary>
    public enum BoardType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 问答
        /// </summary>
        QA = 1,
        /// <summary>
        /// 讨论
        /// </summary>
        Discuss = 2
    }

    /// <summary>
    /// 模式
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// 公开
        /// </summary>
        Public = 0,
        /// <summary>
        /// 私有
        /// </summary>
        Private = 1
    }

    /// <summary>
    /// 问答筛选类型
    /// </summary>
    public enum QAType
    {
        /// <summary>
        ///  精华
        /// </summary>
        Digest = 0,
        /// <summary>
        /// 已解决
        /// </summary>
        Solved = 1,
        /// <summary>
        /// 未解决
        /// </summary>
        Unsolved = 2,
        /// <summary>
        /// 抢答
        /// </summary>
        Zero = 3
    }

    /// <summary>
    /// 搜索类型
    /// </summary>
    public enum SearchType
    { 
        /// <summary>
        /// 标题和关键字
        /// </summary>
        TitleAndKeywords=0,
        /// <summary>
        /// 问题描述
        /// </summary>
        Content=1,
        /// <summary>
        /// 最佳回复
        /// </summary>
        Answer=2
    }

    /// <summary>
    /// 发帖时间搜索类型
    /// </summary>
    public enum PostTimeType
    { 
        /// <summary>
        /// 最近一周
        /// </summary>
        LastWeek=0,
        /// <summary>
        /// 最近一个月
        /// </summary>
        LastMonth=1,
        /// <summary>
        /// 最近一个季度
        /// </summary>
        LastQuarter = 2,
        /// <summary>
        /// 最近半年
        /// </summary>
        LastHalfYear=3
    }

    /// <summary>
    /// 置顶等级
    /// </summary>
    public enum TopLevel
    { 
        None =0,
        Lowest=1,
        Lower=2,
        Middle=3,
        Higher=4,
        Highest=5
    }

    /// <summary>
    /// 用户等级
    /// </summary>
    public enum UserLevel
    { 
        /// <summary>
        /// 注册用户
        /// </summary>
        User=0,
        /// <summary>
        /// 学员
        /// </summary>
        Student=1,
        /// <summary>
        /// 教师
        /// </summary>
        Teacher=2,
        /// <summary>
        /// 管理员
        /// </summary>
        Admin=3,
        /// <summary>
        /// 超管
        /// </summary>
        SuperAdmin=9
    }

    public enum Gender
    { 
        /// <summary>
        /// 男
        /// </summary>
        Male=0,
        /// <summary>
        /// 女
        /// </summary>
        Female=1,
    }
}    
