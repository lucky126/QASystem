using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace ChinaHCM.QA.Site.Web.Models
{
    public class PagingInfo
    {
        /// <summary>
        /// 每页条数
        /// </summary>
        public static int PageSize
        {
            get { return int.Parse(ConfigurationManager.AppSettings["pageSize"]); }
        }
        /// <summary>
        /// 显示的数组链接个数
        /// </summary>
        public static int DigitalLinkNum
        {
            get { return int.Parse(ConfigurationManager.AppSettings["DigitalLinkNum"]); }
        }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get { return (int)Math.Ceiling((decimal)RecordCount / PageSize); }
        }
        public string ContinerId { get; set; }
        /// <summary>
        /// 内容容器IId
        /// </summary>
        public string ContinerID { get; set; }
        /// <summary>
        /// 分页控件Id
        /// </summary>
        public string PageDivID { get; set; }
        /// <summary>
        /// 分页控件css类名
        /// </summary>
        public string PageClass { get; set; }
        /// <summary>
        /// 分页控件导航信息css类名
        /// </summary>
        public string PageNavClass { get; set; }
        /// <summary>
        /// 分页控件链接信息css类名
        /// </summary>
        public string PageLinkClass { get; set; }
        /// <summary>
        /// 显示总记录数
        /// </summary>
        public bool ShowTotalRecord{ get; set; }
        /// <summary>
        /// 显示当前页
        /// </summary>
        public bool ShowCurrentPage { get; set; }
        /// <summary>
        /// 显示总页数
        /// </summary>
        public bool ShowTotalPage { get; set; }

        public PagingInfo()
        {
            PageDivID = "Pager";
            PageClass = "paging clearfix";
            PageNavClass = "pagingNav";
            PageLinkClass = "pagingLink";
            ShowTotalRecord = true;
            ShowCurrentPage = true;
            ShowTotalPage = true;
        }
    }
}