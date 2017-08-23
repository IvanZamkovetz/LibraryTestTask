namespace Library.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class magazines : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Magazines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Name = c.String(),
                        PublishingDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            //AddColumn("dbo.Books", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "timestamp"));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Books", "RowVersion");
            DropTable("dbo.Magazines");
        }
    }
}
