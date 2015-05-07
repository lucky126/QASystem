using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;

using ChinaHCM.QA.Component.Data;
using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Core.Data
{
    /// <summary>
    ///   仓储操作层实现——版区信息
    /// </summary>
    [Export(typeof(IBoardRepository))]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    public class BoardRepository : EFRepositoryBase<e_Board>, IBoardRepository
    {
    }
}
