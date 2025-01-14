using DataAccessLayer.Contracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.UnitOfWork.Contracks
{
    public interface IUnitOfWork : IDisposable
    {
        public ICityRepository cityRepository { get; }
        public IPersonRepository personRepository { get; }
        public IRefreshTokenRepository refreshTokenRepository { get; }
        public IRoleManagerRepository roleManagerRepository { get; }
        public IUserAdderssRepository userAdderssRepository { get; }
        public IUserRepository userRepository { get; }
        public IOtpRepository otpRepository { get; }
        public IProductCategoryImageRepository productCategoryImageRepository { get; }

        public IProductCategoryRepository productCategoryRepository { get; }

        public IBrandRepository brandRepository { get; }

        public IProductSubCategoryRepository productSubCategoryRepository { get; }

        public IProductImageRepository productImageRepository { get; }

        public IProductRepository productRepository { get; }

        public ISellerProductRepository sellerProductRepository { get; }

        public ISellerProductReviewRepository sellerProductReviewRepository { get; }

        public ICityWhereDeliveyWorkRepository CitiyWhereDeliveyWorkRepository { get; }

        public IShoppingCartRepository shoppingCartRepository { get; }

        public IProductsInShoppingCartRepository productsInShoppingCartRepository { get; }

        public IApplicationTypeRepository applicationTypeRepository { get; }

        public IShippingCostRepository shippingCostRepository { get; }

        public IPaymentTypeRepository paymentTypeRepository { get; }

        public IPaymentRepository paymentRepository { get; }
        
        public IApplicationRepository applicationRepository { get; }

        public Task<long> CompleteAsync();

        public Task BeginTransactionAsync();

        public Task CommitTransactionAsync();

        public Task RollbackTransactionAsync();

    }
}
