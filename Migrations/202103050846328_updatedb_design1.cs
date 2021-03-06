namespace amigopet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedb_design1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pets", "PetWithPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Pets", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pets", "PicExtension");
            DropColumn("dbo.Pets", "PetWithPic");
        }
    }
}
