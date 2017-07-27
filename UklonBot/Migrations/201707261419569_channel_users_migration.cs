namespace UklonBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class channel_users_migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChannelUsers",
                c => new
                    {
                        ProviderId = c.String(nullable: false, maxLength: 128),
                        Provider = c.String(),
                        PhoneNumber = c.String(),
                        IsPhoneNumberConfirmed = c.Boolean(nullable: false),
                        City = c.Int(nullable: false),
                        UklonUserToken = c.String(),
                        UklonTokenExpirationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProviderId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ChannelUsers");
        }
    }
}
