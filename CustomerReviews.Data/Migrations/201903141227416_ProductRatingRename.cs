namespace CustomerReviews.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ProductRatingRename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomerReview", "ProductRating", c => c.Int(nullable: false));
            DropColumn("dbo.CustomerReview", "Rating");
        }
    }
}
