using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork;
using DataAccessLayer.UnitOfWork.Contracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class ProductInShoppingCartService : IProductInShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IGenericMapper _genericMapper;
        private readonly ISellerProductService _sellerProductService;

        public ProductInShoppingCartService(IUnitOfWork unitOfWork, IUserService userService,
            IGenericMapper genericMapper, ISellerProductService sellerProductService)
        {
            this._unitOfWork = unitOfWork;
            this._userService = userService;
            this._genericMapper = genericMapper;
            this._sellerProductService = sellerProductService;
        }

        private async Task<bool> _completeAsync()
        {
            var numbersOfRowsAffeted = await _unitOfWork.CompleteAsync();
            return numbersOfRowsAffeted > 0;
        }
        public async Task<SellerProductInShoppingCartDto> AddAsync(SellerProductInShoppingCartDto productInShoppingCartDto, long ShoppingCartId, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(productInShoppingCartDto, nameof(productInShoppingCartDto));
            ParamaterException.CheckIfLongIsBiggerThanZero(ShoppingCartId, nameof(ShoppingCartId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var UserDto = await _userService.FindByIdAsync(UserId);
            if (UserDto == null) return null;


            var ActiveShoppingCart = await _unitOfWork.shoppingCartRepository.GetActiveShoppingCartByUserIdAsync(UserId);
            if (ActiveShoppingCart is null || ActiveShoppingCart.Id != ShoppingCartId) return null;

            var sellerProductDto = await _sellerProductService.FindByIdAsync(productInShoppingCartDto.SellerProductId);
            if (sellerProductDto == null) return null;


            // بتاكد انه مش متضاف قبل كدة في السلة
            if(ActiveShoppingCart.SellerProductsInShoppingCart.Any())
            {
                
                foreach(var productInActiveShoppingCart in ActiveShoppingCart.SellerProductsInShoppingCart)
                {
                    if (productInActiveShoppingCart.SellerProductId == productInShoppingCartDto.SellerProductId)
                        return null;
                }
            }


            if (productInShoppingCartDto.Number > sellerProductDto.NumberInStock)
                return null;


            var productInShoppingCart = _genericMapper.MapSingle<SellerProductInShoppingCartDto, SellerProductInShoppingCart>(productInShoppingCartDto);
            if (productInShoppingCart == null) return null;


            productInShoppingCart.ShoppingCartId = ShoppingCartId;
            productInShoppingCart.TotalPrice = productInShoppingCartDto.Number * sellerProductDto.Price;

            
            await _unitOfWork.SellerProductsInShoppingCartRepository.AddAsync(productInShoppingCart);


            var IsProductInShoppingCartAdded = await _completeAsync();
            if (!IsProductInShoppingCartAdded) return null;

            _genericMapper.MapSingle(productInShoppingCart, productInShoppingCartDto);

            return productInShoppingCartDto;

        }

        public async Task<IEnumerable<SellerProductInShoppingCartDto>> AddRangeAsync(IEnumerable<SellerProductInShoppingCartDto> productsInShoppingCartsDtoList, long ShoppingCartId, string UserId)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(productsInShoppingCartsDtoList, nameof(productsInShoppingCartsDtoList));
            ParamaterException.CheckIfLongIsBiggerThanZero(ShoppingCartId, nameof(ShoppingCartId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var NewProductsInShoppingCartDtosList = new List<SellerProductInShoppingCartDto>();

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                foreach(var productInShoppingCart in productsInShoppingCartsDtoList)
                {
                    var NewProductInShoppingCart = await AddAsync(productInShoppingCart,ShoppingCartId, UserId);

                    if(NewProductInShoppingCart == null)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return null;
                    }

                    NewProductsInShoppingCartDtosList.Add(NewProductInShoppingCart);

                }

                await _unitOfWork.CommitTransactionAsync();

                return NewProductsInShoppingCartDtosList;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long productInShopingCartId, long ShoppingCartId, string UserId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productInShopingCartId, nameof(productInShopingCartId));
            ParamaterException.CheckIfLongIsBiggerThanZero(ShoppingCartId, nameof(ShoppingCartId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var UserDto = await _userService.FindByIdAsync(UserId);
            if (UserDto == null) return false;


            var ActiveShoppingCart = await _unitOfWork.shoppingCartRepository.GetActiveShoppingCartByUserIdAsync(UserId);
            if (ActiveShoppingCart is null || ActiveShoppingCart.Id != ShoppingCartId) return false;

            var productInShopping = await _unitOfWork.SellerProductsInShoppingCartRepository.
                GetByIdAndShoppingCartIdAndUserIdAsync(productInShopingCartId, ShoppingCartId, UserId);

            if (productInShopping is null) return false;

            await _unitOfWork.SellerProductsInShoppingCartRepository.DeleteAsync(productInShopingCartId);

            var IsProductInShoppingCartDeleted = await _completeAsync();

            return IsProductInShoppingCartDeleted;
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<long> productsInShoppingCartIdsList, long ShoppingCartId, string UserId)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(productsInShoppingCartIdsList, nameof(productsInShoppingCartIdsList));
            ParamaterException.CheckIfLongIsBiggerThanZero(ShoppingCartId, nameof(ShoppingCartId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var NewProductsInShoppingCartDtosList = new List<SellerProductInShoppingCartDto>();

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                foreach (var productInShopingCartId in productsInShoppingCartIdsList)
                {
                    var IsProductInShoppingCartDeleted = await DeleteAsync(productInShopingCartId, ShoppingCartId, UserId);

                    if(!IsProductInShoppingCartDeleted)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return false;
                    }
                }

                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<SellerProductInShoppingCartDto> FindByIdAndShoppingCartIdAndUserIdAsync(long productInShoppingCartId, long ShoppingCartId,string UserId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productInShoppingCartId,nameof(productInShoppingCartId));
            var productInShoppingCart = await _unitOfWork.SellerProductsInShoppingCartRepository.GetByIdAndShoppingCartIdAndUserIdAsync(productInShoppingCartId,ShoppingCartId,UserId);

            if (productInShoppingCart is null) return null;

            var productInShoppingCartDto = _genericMapper.MapSingle<SellerProductInShoppingCart, SellerProductInShoppingCartDto>(productInShoppingCart);
            return productInShoppingCartDto;
        }

        public async Task<IEnumerable<SellerProductInShoppingCartDto>> GetAllSellerProductsInShoppingCartByShoppingCartIdAsync(long shoppingCartId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(shoppingCartId, nameof(shoppingCartId));


            var productsInShoppingCartList = await _unitOfWork.SellerProductsInShoppingCartRepository.GetAllSellerProductsInShoppingCartByShoppingCartIdAsync(shoppingCartId);
            if (productsInShoppingCartList is null || !productsInShoppingCartList.Any()) return null;


            var productsInShoppingCartDtosList = _genericMapper.MapCollection<SellerProductInShoppingCart, SellerProductInShoppingCartDto>(productsInShoppingCartList);
            return productsInShoppingCartDtosList;

        }

        public async Task<bool> UpdateAsync(long ProductInShoppingCartId, SellerProductInShoppingCartDto productInShoppingCartDto, long ShoppingCartId, string UserId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ProductInShoppingCartId, nameof(ProductInShoppingCartId));
            ParamaterException.CheckIfObjectIfNotNull(productInShoppingCartDto, nameof(productInShoppingCartDto));
            ParamaterException.CheckIfLongIsBiggerThanZero(ShoppingCartId, nameof(ShoppingCartId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var UserDto = await _userService.FindByIdAsync(UserId);
            if (UserDto == null) return false;


            var ActiveShoppingCart = await _unitOfWork.shoppingCartRepository.GetActiveShoppingCartByUserIdAsync(UserId);
            if (ActiveShoppingCart is null || ActiveShoppingCart.Id != ShoppingCartId) return false;

            var productInShoppingCart = await _unitOfWork.SellerProductsInShoppingCartRepository.
                GetByIdAndShoppingCartIdAndUserIdAsync(ProductInShoppingCartId, ShoppingCartId, UserId);
            if(productInShoppingCart is null) return false;


            var sellerProductDto = await _sellerProductService.FindByIdAsync(productInShoppingCartDto.SellerProductId);
            if (sellerProductDto == null) return false;


            if (productInShoppingCartDto.Number > sellerProductDto.NumberInStock)
                return false;


            _genericMapper.MapSingle(productInShoppingCartDto, productInShoppingCart);

            productInShoppingCart.TotalPrice = productInShoppingCartDto.Number * sellerProductDto.Price;

            await _unitOfWork.SellerProductsInShoppingCartRepository.UpdateAsync(ShoppingCartId, productInShoppingCart);

            var IsProductInShoppingCartUpdated = await _completeAsync();

            return IsProductInShoppingCartUpdated;
        }


    }
}
