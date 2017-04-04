namespace SmithScreening.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Review : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Rating = c.Int(nullable: false),
                        ReviewTitle = c.String(),
                        ReviewComment = c.String(),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            
            
        }
        
        public override void Down()
        {
           
        }
    }
}
