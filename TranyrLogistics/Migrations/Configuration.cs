namespace TranyrLogistics.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<TranyrLogistics.Models.TranyrMembershipDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TranyrLogistics.Models.TranyrMembershipDb context)
        {
            
        }
    }
}
