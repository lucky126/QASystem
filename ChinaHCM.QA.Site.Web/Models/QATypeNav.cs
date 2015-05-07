using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaHCM.QA.Component.Data;

using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Site.Web.Models
{
    public class QATypeNav
    {
        public int BoardId { get; set; }
        public QAType QAType { get; set; }
    }
}