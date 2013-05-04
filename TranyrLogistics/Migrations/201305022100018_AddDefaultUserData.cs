namespace TranyrLogistics.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Web.Security;
    using WebMatrix.WebData;
    
    public partial class AddDefaultUserData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        EmailAddress = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            WebSecurity.InitializeDatabaseConnection("TranyrMembershipDb", "UserProfile", "UserId", "UserName", autoCreateTables: true);

            if (!Roles.RoleExists("Administrator"))
            {
                Roles.CreateRole("Administrator");
            }
            if (!Roles.RoleExists("Customer-Service"))
            {
                Roles.CreateRole("Customer-Service");
            }
            if (!Roles.RoleExists("Finance"))
            {
                Roles.CreateRole("Finance");
            }
            if (!Roles.RoleExists("Operator"))
            {
                Roles.CreateRole("Operator");
            }
            if (!Roles.RoleExists("Manager"))
            {
                Roles.CreateRole("Manager");
            }
            if (!Roles.RoleExists("Reporter"))
            {
                Roles.CreateRole("Reporter");
            }

            WebSecurity.CreateUserAndAccount(
                "administrator",
                "password",
                new { FirstName = "System", LastName = "Administrator", EmailAddress = "system@tranyr.com", IsActive = true, CreateDate = DateTime.Now }
            );

            Roles.AddUserToRole("administrator", "Administrator");
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserProfile");
        }
    }
}
