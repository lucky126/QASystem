//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChinaHCM.QA.Core.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class e_Users
    {
        public e_Users()
        {
            this.e_BBS = new HashSet<e_BBS>();
            this.e_Topic = new HashSet<e_Topic>();
        }
    
        public int UserId { get; set; }
        public int Level { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
        public string RealName { get; set; }
        public string Gender { get; set; }
        public string UserEmail { get; set; }
        public string UserSign { get; set; }
        public string UserQuesion { get; set; }
        public string UserAnswer { get; set; }
        public System.DateTime AddTime { get; set; }
        public string AddIp { get; set; }
        public int TopicCnt { get; set; }
        public int PostCnt { get; set; }
        public int DelCnt { get; set; }
        public int LoginCnt { get; set; }
        public int Grade { get; set; }
        public System.DateTime LastLoginTime { get; set; }
        public int Status { get; set; }
    
        public virtual ICollection<e_BBS> e_BBS { get; set; }
        public virtual ICollection<e_Topic> e_Topic { get; set; }
    }
}
