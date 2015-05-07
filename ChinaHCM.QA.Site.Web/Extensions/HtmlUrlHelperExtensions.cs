using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using ChinaHCM.QA.Site.Web.Models;
using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Site.Web.Extensions
{
    public static class HtmlUrlHelperExtensions
    {
        #region 置顶标记
        /// <summary>
        /// 置顶标记
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="topLevel">置顶排序</param>
        /// <returns></returns>
        public static MvcHtmlString GetTopLevl(this HtmlHelper helper, int topLevel)
        {
            StringBuilder _strBuilder = new StringBuilder();
            if (topLevel > 0)
            {
                TagBuilder tag = new TagBuilder("img");

                var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
                string imgUrl = urlHelper.Content("~/Images/top.gif");

                tag.MergeAttribute("src", imgUrl);
                tag.MergeAttribute("title", "置顶");
                tag.MergeAttribute("id", "TopImg");

                _strBuilder.Append(tag.ToString());
            }

            return MvcHtmlString.Create(_strBuilder.ToString());
        }
        #endregion

        #region 帖子精华标记
        /// <summary>
        /// 帖子精华标记
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="isDigest">是否精华</param>
        /// <returns></returns>
        public static MvcHtmlString GetDigest(this HtmlHelper helper, bool isDigest)
        {
            StringBuilder _strBuilder = new StringBuilder();
            if (isDigest)
            {
                TagBuilder tag = new TagBuilder("img");

                var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
                string imgUrl = urlHelper.Content("~/Images/star.gif");

                tag.MergeAttribute("src", imgUrl);
                tag.MergeAttribute("title", "精华");
                tag.MergeAttribute("id", "DigestImg");

                _strBuilder.Append(tag.ToString());
            }

            return MvcHtmlString.Create(_strBuilder.ToString());
        }
        #endregion

        #region 帖子完成标记
        /// <summary>
        /// 帖子完成标记
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="isFinish">是否完成</param>
        /// <returns></returns>
        public static MvcHtmlString TopicFinish(this HtmlHelper helper, bool isFinish)
        {
            StringBuilder _strBuilder = new StringBuilder();

            TagBuilder tag = new TagBuilder("img");

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            string imgUrl = urlHelper.Content(isFinish ? "~/Images/Resolved.gif" : "~/Images/Unresolved.gif");

            tag.MergeAttribute("src", imgUrl);
            tag.MergeAttribute("title", isFinish ? "已解决" : "未解决");
            tag.MergeAttribute("id", "FinishImg");

            _strBuilder.Append(tag.ToString());

            return MvcHtmlString.Create(_strBuilder.ToString());
        }
        #endregion

        #region 帖子锁定标记
        /// <summary>
        /// 帖子锁定标记
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="isLock">是否锁定</param>
        /// <returns></returns>
        public static MvcHtmlString GetLock(this HtmlHelper helper, bool isLock)
        {
            StringBuilder _strBuilder = new StringBuilder();
            if (isLock)
            {
                TagBuilder tag = new TagBuilder("img");

                var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
                string imgUrl = urlHelper.Content("~/Images/ico_lock.gif");

                tag.MergeAttribute("src", imgUrl);
                tag.MergeAttribute("title", "锁定");
                tag.MergeAttribute("id", "LockImg");

                _strBuilder.Append(tag.ToString());
            }

            return MvcHtmlString.Create(_strBuilder.ToString());
        }
        #endregion

        #region 帖子IP信息
        /// <summary>
        /// 帖子IP信息
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="ip">IP</param>
        /// <returns></returns>
        public static MvcHtmlString GetIP(this HtmlHelper helper, string ip)
        {
            StringBuilder _strBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(ip))
            {
                TagBuilder tag = new TagBuilder("img");

                var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
                string imgUrl = urlHelper.Content("~/Images/ico_IP.gif");

                tag.MergeAttribute("src", imgUrl);
                tag.MergeAttribute("title", string.Format("IP:{0}", ip));
                tag.MergeAttribute("id", "IPImg");

                _strBuilder.Append(tag.ToString());
            }

            return MvcHtmlString.Create(_strBuilder.ToString());
        }
        #endregion

        #region 讨论区楼层定义
        /// <summary>
        /// 讨论区楼层定义
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="iCnt"></param>
        /// <returns></returns>
        public static MvcHtmlString GetDiscussCount(this HtmlHelper helper, int iCnt)
        {
            string strTimestring = string.Format("{0}楼", iCnt);

            if (iCnt == 1)
                strTimestring = "楼主";

            if (iCnt == 2)
                strTimestring = "沙发";

            if (iCnt == 3)
                strTimestring = "板凳";

            return new MvcHtmlString(strTimestring);
        }
        #endregion

        #region 积分等级定义
        /// <summary>
        /// 积分等级定义
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="iCnt"></param>
        /// <returns></returns>
        public static MvcHtmlString GetGradeDesc(this HtmlHelper helper, int iCnt)
        {
            Dictionary<int, string> dicGradeDesc = new Dictionary<int, string>();
            dicGradeDesc.Add(0, "婴儿");
            dicGradeDesc.Add(10, "幼儿");
            dicGradeDesc.Add(50, "小学");
            dicGradeDesc.Add(100, "初中");
            dicGradeDesc.Add(500, "高中");
            dicGradeDesc.Add(1000, "本科");
            dicGradeDesc.Add(5000, "硕士");
            dicGradeDesc.Add(10000, "博士");
            dicGradeDesc.Add(50000, "博士后");

            string _gradeDesc = dicGradeDesc[0];

            foreach (KeyValuePair<int, string> val in dicGradeDesc)
            {
                if (iCnt > val.Key)
                    _gradeDesc = val.Value;
                else
                    break;
            }

            return new MvcHtmlString(_gradeDesc);
        }
        #endregion

        #region 格式化时间
        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="date">时间</param>
        /// <returns></returns>
        public static MvcHtmlString FormatAllDateTime(this HtmlHelper helper, DateTime date)
        {
            return FormatDateTime(helper, date, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="date">时间</param>
        /// <returns></returns>
        public static MvcHtmlString FormatDateTime(this HtmlHelper helper, DateTime date)
        {
            return FormatDateTime(helper, date, "yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="date">时间</param>
        /// <returns></returns>
        public static MvcHtmlString FormatDateTime(this HtmlHelper helper, DateTime date, string stringFormat)
        {
            return new MvcHtmlString(date.ToString(stringFormat));
        }

        /// <summary>
        /// 格式化QA时间
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="date">时间</param>
        /// <returns></returns>
        public static MvcHtmlString DateTimeLeaveString(this HtmlHelper helper, DateTime date)
        {
            TimeSpan ts = DateTime.Now - date;
            string strTimestring = date.ToString("yyyy-MM-dd HH:mm");

            if (ts.TotalSeconds < 60)
                strTimestring = string.Format("{0}秒前", ts.Seconds);

            if (ts.TotalMinutes < 60)
                strTimestring = string.Format("{0}分钟前", ts.Minutes);

            if (ts.TotalHours < 24)
                strTimestring = string.Format("{0}小时前", ts.Hours);

            if (ts.TotalDays < 5)
                strTimestring = string.Format("{0}天前", ts.Days);

            return new MvcHtmlString(strTimestring);
        }
        #endregion

        #region 页面导航
        /// <summary>
        /// 页面导航
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="pageUrlAccessor"></param>
        /// <returns></returns>
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrlAccessor)
        {
            //是否显示导航
            bool IsShowPageNav;

            if (pagingInfo.RecordCount < PagingInfo.PageSize)
                IsShowPageNav = false;
            else
                IsShowPageNav = true;

            StringBuilder _strBuilder = new StringBuilder();
            _strBuilder.AppendFormat("<div id=\"{0}\" class=\"{1}\">", pagingInfo.PageDivID, pagingInfo.PageClass);

            _strBuilder.AppendFormat("<div class=\"{0}\">", pagingInfo.PageNavClass);
            if (pagingInfo.ShowTotalRecord) _strBuilder.AppendFormat("共{0}条", pagingInfo.RecordCount);
            if (pagingInfo.ShowCurrentPage && IsShowPageNav) _strBuilder.AppendFormat("/每页{0}条 ", PagingInfo.PageSize);
            if (pagingInfo.ShowTotalPage && IsShowPageNav) _strBuilder.AppendFormat("第{0}页/共{1}页 ", pagingInfo.PageIndex, pagingInfo.PageCount);
            _strBuilder.Append("</div>");

            _strBuilder.AppendFormat("<div class=\"{0}\">", pagingInfo.PageLinkClass);

            //首页链接
            if (pagingInfo.PageIndex > 1)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrlAccessor(1));
                tag.InnerHtml = "首页";
                _strBuilder.Append(tag.ToString());
            }
            else if (IsShowPageNav)
                _strBuilder.Append("<span>首页</span>");

            //上一页
            if (pagingInfo.PageIndex > 1)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrlAccessor(pagingInfo.PageIndex - 1));
                tag.InnerHtml = "上一页";
                _strBuilder.Append(tag.ToString());
            }
            else if (IsShowPageNav)
                _strBuilder.Append("<span>上一页</span>");

            //数字导航开始
            if (IsShowPageNav)
            {
                int _startPage, _endPage;
                //总页数少于要显示的页数，页码全部显示
                if (PagingInfo.DigitalLinkNum >= pagingInfo.PageCount) { _startPage = 1; _endPage = pagingInfo.PageCount; }
                else//显示指定数量的页码
                {
                    int _forward = (int)Math.Ceiling(PagingInfo.DigitalLinkNum / 2.0);
                    if (pagingInfo.PageIndex > _forward)//起始页码大于1
                    {
                        _endPage = pagingInfo.PageIndex + PagingInfo.DigitalLinkNum - _forward;
                        if (_endPage > pagingInfo.PageCount)//结束页码大于总页码结束页码为最后一页
                        {
                            _startPage = pagingInfo.PageCount - PagingInfo.DigitalLinkNum;
                            _endPage = pagingInfo.PageCount;

                        }
                        else _startPage = pagingInfo.PageIndex - _forward;
                    }
                    else//起始页码从1开始
                    {
                        _startPage = 1;
                        _endPage = PagingInfo.DigitalLinkNum;
                    }
                }
                //向上…
                if (_startPage > 1)
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.MergeAttribute("href", pageUrlAccessor(_startPage - 1));
                    tag.InnerHtml = "...";
                    _strBuilder.Append(tag.ToString());
                }
                //数字
                for (int i = _startPage; i <= _endPage; i++)
                {
                    TagBuilder tag;
                    if (i == pagingInfo.PageIndex)
                    {
                        tag = new TagBuilder("span");
                        tag.InnerHtml = i.ToString();
                        tag.AddCssClass("selected");
                    }
                    else
                    {
                        tag = new TagBuilder("a");
                        tag.MergeAttribute("href", pageUrlAccessor(i));
                        tag.InnerHtml = i.ToString();
                    }
                    _strBuilder.Append(tag.ToString());
                }
                //向下…
                if (_endPage < pagingInfo.PageCount)
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.MergeAttribute("href", pageUrlAccessor(_endPage + 1));
                    tag.InnerHtml = "...";
                    _strBuilder.Append(tag.ToString());
                }
                ////数字导航结束
            }

            //下一页和尾页
            if (pagingInfo.PageIndex < pagingInfo.PageCount)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrlAccessor(pagingInfo.PageIndex + 1));
                tag.InnerHtml = "下一页";
                _strBuilder.Append(tag.ToString());

                TagBuilder tagLast = new TagBuilder("a");
                tagLast.MergeAttribute("href", pageUrlAccessor(pagingInfo.PageCount));
                tagLast.InnerHtml = "尾页";
                _strBuilder.Append(tagLast.ToString());
            }
            else if (IsShowPageNav)
                _strBuilder.Append("<span>下一页</span><span>尾页</span>");

            _strBuilder.Append("</div>");

            _strBuilder.Append("</div>");

            return MvcHtmlString.Create(_strBuilder.ToString());
        }

        /// <summary>
        /// Ajax页面导航
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="pageUrlAccessor"></param>
        /// <returns></returns>
        public static MvcHtmlString AjaxPageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrlAccessor)
        {
            //是否显示导航
            bool IsShowPageNav;

            if (pagingInfo.RecordCount < PagingInfo.PageSize)
                IsShowPageNav = false;
            else
                IsShowPageNav = true;

            StringBuilder _strBuilder = new StringBuilder();
            _strBuilder.AppendFormat("<div id=\"{0}\" class=\"{1}\">", pagingInfo.PageDivID, pagingInfo.PageClass);

            _strBuilder.AppendFormat("<div class=\"{0}\">", pagingInfo.PageNavClass);
            if (pagingInfo.ShowTotalRecord) _strBuilder.AppendFormat("共{0}条", pagingInfo.RecordCount);
            if (pagingInfo.ShowCurrentPage && IsShowPageNav) _strBuilder.AppendFormat("/每页{0}条 ", PagingInfo.PageSize);
            if (pagingInfo.ShowTotalPage && IsShowPageNav) _strBuilder.AppendFormat("第{0}页/共{1}页 ", pagingInfo.PageIndex, pagingInfo.PageCount);
            _strBuilder.Append("</div>");

            _strBuilder.AppendFormat("<div class=\"{0}\">", pagingInfo.PageLinkClass);

            //首页链接
            if (pagingInfo.PageIndex > 1)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrlAccessor(1));
                tag.InnerHtml = "首页";
                _strBuilder.Append(tag.ToString());
            }
            else if (IsShowPageNav)
                _strBuilder.Append("<span>首页</span>");

            //上一页
            if (pagingInfo.PageIndex > 1)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrlAccessor(pagingInfo.PageIndex - 1));
                tag.InnerHtml = "上一页";
                _strBuilder.Append(tag.ToString());
            }
            else if (IsShowPageNav)
                _strBuilder.Append("<span>上一页</span>");

            //数字导航开始
            if (IsShowPageNav)
            {
                int _startPage, _endPage;
                //总页数少于要显示的页数，页码全部显示
                if (PagingInfo.DigitalLinkNum >= pagingInfo.PageCount) { _startPage = 1; _endPage = pagingInfo.PageCount; }
                else//显示指定数量的页码
                {
                    int _forward = (int)Math.Ceiling(PagingInfo.DigitalLinkNum / 2.0);
                    if (pagingInfo.PageIndex > _forward)//起始页码大于1
                    {
                        _endPage = pagingInfo.PageIndex + PagingInfo.DigitalLinkNum - _forward;
                        if (_endPage > pagingInfo.PageCount)//结束页码大于总页码结束页码为最后一页
                        {
                            _startPage = pagingInfo.PageCount - PagingInfo.DigitalLinkNum;
                            _endPage = pagingInfo.PageCount;

                        }
                        else _startPage = pagingInfo.PageIndex - _forward;
                    }
                    else//起始页码从1开始
                    {
                        _startPage = 1;
                        _endPage = PagingInfo.DigitalLinkNum;
                    }
                }
                //向上…
                if (_startPage > 1)
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.MergeAttribute("href", pageUrlAccessor(_startPage - 1));
                    tag.InnerHtml = "...";
                    _strBuilder.Append(tag.ToString());
                }
                //数字
                for (int i = _startPage; i <= _endPage; i++)
                {
                    TagBuilder tag;
                    if (i == pagingInfo.PageIndex)
                    {
                        tag = new TagBuilder("span");
                        tag.InnerHtml = i.ToString();
                        tag.AddCssClass("selected");
                    }
                    else
                    {
                        tag = new TagBuilder("a");
                        tag.MergeAttribute("href", pageUrlAccessor(i));
                        tag.InnerHtml = i.ToString();
                    }
                    _strBuilder.Append(tag.ToString());
                }
                //向下…
                if (_endPage < pagingInfo.PageCount)
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.MergeAttribute("href", pageUrlAccessor(_endPage + 1));
                    tag.InnerHtml = "...";
                    _strBuilder.Append(tag.ToString());
                }
                ////数字导航结束
            }

            //下一页和尾页
            if (pagingInfo.PageIndex < pagingInfo.PageCount)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrlAccessor(pagingInfo.PageIndex + 1));
                tag.InnerHtml = "下一页";
                _strBuilder.Append(tag.ToString());

                TagBuilder tagLast = new TagBuilder("a");
                tagLast.MergeAttribute("href", pageUrlAccessor(pagingInfo.PageCount));
                tagLast.InnerHtml = "尾页";
                _strBuilder.Append(tagLast.ToString());
            }
            else if (IsShowPageNav)
                _strBuilder.Append("<span>下一页</span><span>尾页</span>");

            _strBuilder.Append("</div>");

            _strBuilder.Append("<script type=\"text/javascript\">$(\"#" + pagingInfo.PageDivID + " a\").click(function () {$.post($(this).attr(\"href\"), function (data) {$(\"#" + pagingInfo.ContinerID + "\").html(data);});return false; });</script>");

            _strBuilder.Append("</div>");

            return MvcHtmlString.Create(_strBuilder.ToString());
        }
        #endregion

        #region 判定是否
        /// <summary>
        /// 判定是否
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="iCnt"></param>
        /// <returns></returns>
        public static MvcHtmlString GetYesOrNo(this HtmlHelper helper, bool yesOrNo)
        {
            if (yesOrNo)
                return new MvcHtmlString("<span class=\"YesOrNo\">是</span>");
            else
                return new MvcHtmlString(""); ;
        }
        #endregion        
    }
}
