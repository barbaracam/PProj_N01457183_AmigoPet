namespace amigopet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedatabase6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PetWalkers", "PetWalker_PetWalkerID", c => c.Int());
            AddColumn("dbo.PetWalkers", "Pet_PetID", c => c.Int());
            CreateIndex("dbo.PetWalkers", "PetWalker_PetWalkerID");
            CreateIndex("dbo.PetWalkers", "Pet_PetID");
            AddForeignKey("dbo.PetWalkers", "PetWalker_PetWalkerID", "dbo.PetWalkers", "PetWalkerID");
            AddForeignKey("dbo.PetWalkers", "Pet_PetID", "dbo.Pets", "PetID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PetWalkers", "Pet_PetID", "dbo.Pets");
            DropForeignKey("dbo.PetWalkers", "PetWalker_PetWalkerID", "dbo.PetWalkers");
            DropIndex("dbo.PetWalkers", new[] { "Pet_PetID" });
            DropIndex("dbo.PetWalkers", new[] { "PetWalker_PetWalkerID" });
            DropColumn("dbo.PetWalkers", "Pet_PetID");
            DropColumn("dbo.PetWalkers", "PetWalker_PetWalkerID");
        }
    }
}
