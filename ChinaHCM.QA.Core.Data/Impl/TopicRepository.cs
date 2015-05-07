using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;

using ChinaHCM.QA.Component.Data;
using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Core.Data
{
    /// <summary>
    ///   仓储操作层实现——主贴信息
    /// </summary>
    [Export(typeof(ITopicRepository))]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    public class TopicRepository : EFRepositoryBase<e_Topic>, ITopicRepository
    {
        /*
        public void LoadBBS(e_Topic entity)
        {
            var entry = EFContext.DbContext.Entry(entity);
            entry.Collection(t=>t.e_BBS).Load();
        }
         * */
    }
}
