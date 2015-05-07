using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ChinaHCM.QA.Component.Tools
{
    public class Functions
    {
        #region 获取真实IP地址
        /// <summary>
        /// 获取真实IP地址
        /// </summary>
        /// <remarks>
        /// 修改：潘云峰 2012-03-13
        /// request.ServerVariables["HTTP_VIA"] 和 request.ServerVariables["HTTP_X_FORWARDED_FOR"]
        /// 有可能被伪造实施注入式攻击，因此，升级此方法。
        /// 由于只有“REMOTE_ADDR”（UserHostAddress），所以通过这个方法获得IP，
        /// 如果想知道代理IP的信息，可以用下面的带out参数的方法
        /// </remarks>
        /// <returns>真实IP</returns>
        public static string GetRealIP()
        {
            try
            {
                HttpRequest request = HttpContext.Current.Request;

                return request.UserHostAddress;
            }
            catch
            {
                return "0.0.0.0";
            }

        }
        /// <summary>
        /// 获取真实IP地址，注意out参数有可能是伪造的，无法避免
        /// </summary>
        /// <remarks>
        /// 创建：潘云峰 2012-03-13
        /// request.ServerVariables["HTTP_VIA"] 和 request.ServerVariables["HTTP_X_FORWARDED_FOR"]
        /// 有可能被伪造实施注入式攻击，因此，升级此方法。主要是去掉单引号
        /// </remarks>
        /// <param name="strDaiLiIP">代理IP的信息串，但是不代表其真实可信</param>
        /// <returns>IP，out 代理IP信息串</returns>
        public static string GetRealIP(out string strDaiLiIP)
        {
            strDaiLiIP = string.Empty;
            try
            {
                HttpRequest request = HttpContext.Current.Request;

                if (request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    if (request.ServerVariables["HTTP_VIA"] == null)
                    {
                        //不合常理，很可能是伪造的
                        return request.UserHostAddress;
                    }
                    string strXFor = Convert.ToString(request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
                    if (string.IsNullOrEmpty(strXFor))
                    {
                        return request.UserHostAddress;
                    }
                    else
                    {
                        string[] strXForList = strXFor.Split(',');

                        foreach (string strX in strXForList)
                        {
                            if (string.IsNullOrEmpty(strX) ||
                                strX.Equals(request.UserHostAddress))
                            {
                                //这也不符合规则，还是说明是伪造的
                                //根据规则，"HTTP_X_FORWARDED_FOR"不会包含“REMOTE_ADDR”（UserHostAddress）
                                return request.UserHostAddress;
                            }
                            else if (strX.IndexOf('\'') != -1)
                            {
                                //不符合规矩
                                return request.UserHostAddress;
                            }
                        }

                        strDaiLiIP = strXFor;   //这个代理IP串即时是伪造的，也伪造得比较真实
                        return request.UserHostAddress;
                    }
                }
                else
                {
                    return request.UserHostAddress;
                }
            }
            catch
            {
                return "0.0.0.0";
            }

        }
        #endregion
    }
}
