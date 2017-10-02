namespace WpfApp1
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MomSQLedm : DbContext
    {
        public MomSQLedm()
            : base("name=MomSQLedm")
        {
        }

      

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
