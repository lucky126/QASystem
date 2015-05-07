using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.ComponentModel.Composition;
using System.Data.Common;

using ChinaHCM.QA.Core.Model;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ChinaHCM.QA.Component.Data
{
    /// <summary>
    /// EF数据上下文
    /// </summary>
    [Export("EF", typeof(DbContext))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class BBSDbContext : DbContext
    {
        public BBSDbContext()
            : base("name=BBS")
        {            
        }

        public BBSDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString) { }

        public BBSDbContext(DbConnection existingConnection)
            : base(existingConnection, true) { }

        public DbSet<e_BBS> BBS { get; set; }
        public DbSet<e_Board> Board { get; set; }
        public DbSet<e_Topic> Topic { get; set; }
        public DbSet<e_Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //移除一对多的级联删除约定，想要级联删除可以在 EntityTypeConfiguration<TEntity>的实现类中进行控制
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //多对多启用级联删除约定，不想级联删除可以在删除前判断关联的数据进行拦截
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }
    }
}
