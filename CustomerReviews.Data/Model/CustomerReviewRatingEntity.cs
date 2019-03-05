using System;
using System.ComponentModel.DataAnnotations;
using CustomerReviews.Core.Model;
using VirtoCommerce.Platform.Core.Common;

namespace CustomerReviews.Data.Model
{
    public class CustomerReviewRatingEntity : AuditableEntity
    {
        public string AuthorNickname { get; set; }

        [Required]
        [Range(0, 5)]
        public int Rating { get; set; }

        public string ReviewId { get; set; }
        public CustomerReviewEntity Review { get; set; }

        public virtual CustomerReviewRating ToModel(CustomerReviewRating rating)
        {
            if (rating == null)
                throw new ArgumentNullException(nameof(rating));

            rating.Id = Id;
            rating.CreatedBy = CreatedBy;
            rating.CreatedDate = CreatedDate;
            rating.ModifiedBy = ModifiedBy;
            rating.ModifiedDate = ModifiedDate;

            rating.AuthorNickname = AuthorNickname;
            rating.Rating = Rating;

            return rating;
        }

        public virtual CustomerReviewRatingEntity FromModel(CustomerReviewRating rating, PrimaryKeyResolvingMap pkMap)
        {
            if (rating == null)
                throw new ArgumentNullException(nameof(rating));

            Id = rating.Id;
            CreatedBy = rating.CreatedBy;
            CreatedDate = rating.CreatedDate;
            ModifiedBy = rating.ModifiedBy;
            ModifiedDate = rating.ModifiedDate;

            AuthorNickname = rating.AuthorNickname;
            Rating = rating.Rating;

            return this;
        }

        public virtual void Patch(CustomerReviewRatingEntity target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            target.AuthorNickname = AuthorNickname;
            target.Rating = Rating;
        }
    }
}
