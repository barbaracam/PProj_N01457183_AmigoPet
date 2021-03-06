namespace amigopet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedesign2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pets", "PetHasPic", c => c.Boolean(nullable: false));
            DropColumn("dbo.Pets", "PetWithPic");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pets", "PetWithPic", c => c.Boolean(nullable: false));
            DropColumn("dbo.Pets", "PetHasPic");
        }
    }
}
