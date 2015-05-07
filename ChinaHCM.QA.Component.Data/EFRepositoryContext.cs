using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.Composition;

namespace ChinaHCM.QA.Component.Data
{
    /// <summary>
    ///     数据单元操作类
    /// </summary>
    [Export(typeof(IUnitOfWork))]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    public class EFRepositoryContext : UnitOfWorkContextBase
    {
        /// <summary>
        ///     获取 当前使用的数据访问上下文对象
        /// </summary>
        protected override DbContext Context
        {
            get
            {
                //bool secondCachingEnabled = ConfigurationManager.AppSettings["EntityFrameworkCachingEnabled"].CastTo(false);
                //return secondCachingEnabled ? EFCachingDbContext.Value : EFDbContext.Value;
                return BBSDbContext;
            }
        }

        //检测是否是同一上下文用的测试变量
        public string ID = Guid.NewGuid().ToString();

        [Import("EF", typeof(DbContext))]
        public BBSDbContext BBSDbContext { get; set; }


        //[Import("EFCaching", typeof(DbContext))]
        //private Lazy<EFCachingDbContext> EFCachingDbContext { get; set; }
    }
}
