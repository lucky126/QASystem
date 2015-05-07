using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition;

using ChinaHCM.QA.Component.Data;
using ChinaHCM.QA.Core.Model;

namespace ChinaHCM.QA.Core.Data
{
    /// <summary>
    ///   仓储操作层实现——用户信息
    /// </summary>
    [Export(typeof(IUserRepository))]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    public class UserRepository : EFRepositoryBase<e_Users>, IUserRepository
    {
    }
}
