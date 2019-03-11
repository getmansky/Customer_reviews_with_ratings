using System;
using System.Linq;
using CustomerReviews.Core.Model;
using CustomerReviews.Core.Services;
using CustomerReviews.Data.Model;
using CustomerReviews.Data.Repositories;
using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace CustomerReviews.Data.Services
{
    public class CustomerReviewService : ServiceBase, ICustomerReviewService
    {
        private readonly Func<ICustomerReviewRepository> _repositoryFactory;

        public CustomerReviewService(Func<ICustomerReviewRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public CustomerReview[] GetByIds(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                return repository.GetByIds(ids).Select(x => x.ToModel(AbstractTypeFactory<CustomerReview>.TryCreateInstance())).ToArray();
            }
        }

        public void SaveCustomerReviews(CustomerReview[] items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _repositoryFactory())
            {
                using (var changeTracker = GetChangeTracker(repository))
                {
                    var alreadyExistEntities = repository.GetByIds(items.Where(m => !m.IsTransient()).Select(x => x.Id).ToArray());
                    foreach (var derivativeContract in items)
                    {
                        var sourceEntity = AbstractTypeFactory<CustomerReviewEntity>.TryCreateInstance().FromModel(derivativeContract, pkMap);
                        var targetEntity = alreadyExistEntities.FirstOrDefault(x => x.Id == sourceEntity.Id);
                        if (targetEntity != null)
                        {
                            changeTracker.Attach(targetEntity);
                            sourceEntity.Patch(targetEntity);
                        }
                        else
                        {
                            repository.Add(sourceEntity);
                        }
                    }

                    CommitChanges(repository);
                    pkMap.ResolvePrimaryKeys();
                }
            }
        }

        public void DeleteCustomerReviews(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                repository.DeleteCustomerReviews(ids);
                CommitChanges(repository);
            }
        }

        public void Rate(string reviewId, CustomerReviewRating rating)
        {
            if (reviewId == null)
                throw new ArgumentNullException(nameof(reviewId));

            using (var repository = _repositoryFactory())
            {
                var review = repository.GetByIds(new string[] { reviewId }).FirstOrDefault();

                if (review == null)
                    throw new ArgumentException(nameof(reviewId));

                var pkMap = new PrimaryKeyResolvingMap();

                var sourceEntity = AbstractTypeFactory<CustomerReviewRatingEntity>
                       .TryCreateInstance()
                       .FromModel(rating, pkMap);

                var existingEntity = review.Ratings.FirstOrDefault(r => r.Id == rating.Id);

                if (existingEntity != null)
                    sourceEntity.Patch(existingEntity);
                else
                    review.Ratings.Add(sourceEntity);

                CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();
            }
        }


        public GenericSearchResult<CustomerReviewRating> GetRatings(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            using (var repository = _repositoryFactory())
            {
                var ratings = repository.GetByIds(new string[] { id })
                    .FirstOrDefault()
                    ?.Ratings
                    .Select(r => r.ToModel(new CustomerReviewRating()))
                    .ToList();

                var result = new GenericSearchResult<CustomerReviewRating>();
                if (ratings != null)
                {
                    result.TotalCount = ratings.Count;
                    result.Results = ratings;
                }

                return result;
            }
        }
    }
}
