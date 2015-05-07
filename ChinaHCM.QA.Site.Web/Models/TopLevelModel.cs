using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Site.Web.Models
{
    public class TopLevelModel
    {
        public int TopicId { get; set; }
        public TopLevel TopLevel { get; set; }
    }
}