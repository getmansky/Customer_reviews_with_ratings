namespace CustomerReviews.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Ratings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerReviewRatingEntity",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    AuthorNickname = c.String(),
                    Rating = c.Int(nullable: false),
                    ReviewId = c.String(maxLength: 128),
                    CreatedDate = c.DateTime(nullable: false),
                    ModifiedDate = c.DateTime(),
                    CreatedBy = c.String(maxLength: 64),
                    ModifiedBy = c.String(maxLength: 64),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerReview", t => t.ReviewId)
                .Index(t => t.ReviewId);

            AddColumn("dbo.CustomerReview", "Rating", c => c.Int(nullable: false));
        }
    }
}
