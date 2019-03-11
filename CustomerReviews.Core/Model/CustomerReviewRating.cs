using VirtoCommerce.Platform.Core.Common;

namespace CustomerReviews.Core.Model
{
    public class CustomerReviewRating : AuditableEntity
    {
        public int Rating { get; set; }
        public string AuthorNickname { get; set; }
        public string ReviewId { get; set; }
    }
}
