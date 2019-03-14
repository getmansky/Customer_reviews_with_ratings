using System;
using System.Data.Entity;
using CustomerReviews.Core.Model;
using CustomerReviews.Core.Services;
using CustomerReviews.Data.Migrations;
using CustomerReviews.Data.Repositories;
using CustomerReviews.Data.Services;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Testing.Bases;
using Xunit;

namespace CustomerReviews.Test
{
    public class CustomerReviewsTests : FunctionalTestBase
    {
        private const string ProductId = "testProductId";
        private const string CustomerReviewId = "testId";
        private const string ReviewRatingId = "testRateId";

        public CustomerReviewsTests()
        {
            // TODO Do not use real database. Use InMemoryDb instead.
            ConnectionString = "VirtoCommerce";
        }

        [Fact]
        public void CanDoCRUDandSearch()
        {
            // Read non-existing item
            var getByIdsResult = CustomerReviewService.GetByIds(new[] { CustomerReviewId });
            Assert.NotNull(getByIdsResult);
            Assert.Empty(getByIdsResult);

            // Create
            var item = new CustomerReview
            {
                Id = CustomerReviewId,
                ProductId = ProductId,
                CreatedDate = DateTime.Now,
                CreatedBy = "initial data seed",
                AuthorNickname = "John Doe",
                Content = "Liked that",
                Rating = 4
            };

            CustomerReviewService.SaveCustomerReviews(new[] { item });

            getByIdsResult = CustomerReviewService.GetByIds(new[] { CustomerReviewId });
            Assert.Single(getByIdsResult);

            item = getByIdsResult[0];
            Assert.Equal(CustomerReviewId, item.Id);

            // Update
            var updatedContent = "Updated content";
            Assert.NotEqual(updatedContent, item.Content);

            item.Content = updatedContent;
            CustomerReviewService.SaveCustomerReviews(new[] { item });
            getByIdsResult = CustomerReviewService.GetByIds(new[] { CustomerReviewId });
            Assert.Single(getByIdsResult);

            item = getByIdsResult[0];
            Assert.Equal(updatedContent, item.Content);

            // Rate
            var rating = new CustomerReviewRating
            {
                Rating = 4,
                AuthorNickname = "john",
                CreatedBy = "initial data seed",
                CreatedDate = DateTime.Now,
                Id = ReviewRatingId,
                ReviewId = CustomerReviewId
            };

            Assert.Throws<ArgumentNullException>(() => CustomerReviewService.Rate(null, rating));

            CustomerReviewService.Rate(CustomerReviewId, rating);
            var gotRating = CustomerReviewService.GetRatings(CustomerReviewId);

            Assert.Equal(1, gotRating.TotalCount);
            Assert.Single(gotRating.Results);

            // Search
            Assert.Throws<ArgumentNullException>(() => CustomerReviewSearchService.SearchCustomerReviews(null));

            var criteria = new CustomerReviewSearchCriteria { ProductIds = new[] { ProductId } };
            var searchResult = CustomerReviewSearchService.SearchCustomerReviews(criteria);

            Assert.NotNull(searchResult);
            Assert.Equal(1, searchResult.TotalCount);
            Assert.Single(searchResult.Results);

            // Delete
            CanDeleteCustomerReviews();
        }

        [Fact]
        public void CanDeleteCustomerReviews()
        {
            CustomerReviewService.DeleteCustomerReviews(new[] { CustomerReviewId });

            var getByIdsResult = CustomerReviewService.GetByIds(new[] { CustomerReviewId });
            Assert.NotNull(getByIdsResult);
            Assert.Empty(getByIdsResult);
        }

        private ICustomerReviewSearchService CustomerReviewSearchService
        {
            get
            {
                return new CustomerReviewSearchService(GetRepository, CustomerReviewService);
            }
        }
        private ICustomerReviewService CustomerReviewService
        {
            get
            {
                return new CustomerReviewService(GetRepository);
            }
        }


        protected ICustomerReviewRepository GetRepository()
        {
            var repository = new CustomerReviewRepository(ConnectionString, new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor(null));
            EnsureDatabaseInitialized(() => new CustomerReviewRepository(ConnectionString), () => Database.SetInitializer(new SetupDatabaseInitializer<CustomerReviewRepository, Configuration>()));
            return repository;
        }
    }
}
