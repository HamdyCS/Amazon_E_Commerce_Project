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
    public class SellerProductInShoppingCartService : ISellerProductInShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IGenericMapper _genericMapper;
        private readonly ISellerProductService _sellerProductService;
        private readonly IShoppingCartService _shoppingCartService;


        public SellerProductInShoppingCartService(IUnitOfWork unitOfWork, IUserService userService,
            IGenericMapper genericMapper, ISellerProductService sellerProductService, IShoppingCartService shoppingCartService)
        {
            this._unitOfWork = unitOfWork;
            this._userService = userService;
            this._genericMapper = genericMapper;
            this._sellerProductService = sellerProductService;
            this._shoppingCartService = shoppingCartService;
        }

        private async Task<bool> _completeAsync()
        {
            var numbersOfRowsAffeted = await _unitOfWork.CompleteAsync();
            return numbersOfRowsAffeted > 0;
        }

        private async Task _AddAsync(AddSellerProductToShoppingCartDto addProductInShoppingCartDto, ShoppingCartDto ActiveShoppingCartDto, string UserId)
        {


            var sellerProductDto = await _sellerProductService.FindByIdAsync(addProductInShoppingCartDto.SellerProductId);
            if (sellerProductDto == null) throw new KeyNotFoundException($"Not found seller product. Id = {addProductInShoppingCartDto.SellerProductId}");

            //check if Quantity greater than stock
            if (addProductInShoppingCartDto.Quantity > sellerProductDto.NumberInStock)
                addProductInShoppingCartDto.Quantity = sellerProductDto.NumberInStock;

            //mapping from AddSellerProductToShoppingCartDto to SellerProductInShoppingCart
            var sellerProductInShoppingCart = _genericMapper.MapSingle<AddSellerProductToShoppingCartDto, SellerProductInShoppingCart>(addProductInShoppingCartDto);

            // check if the product is already in the cart, if yes update the quantity and total price only without adding new record in database
            if (ActiveShoppingCartDto.SellerProducts.Any())
            {

                foreach (var sellerProductInActiveShoppingCart in ActiveShoppingCartDto.SellerProducts)
                {
                    if (sellerProductInActiveShoppingCart.SellerProductId == addProductInShoppingCartDto.SellerProductId)
                    {

                        sellerProductInActiveShoppingCart.Quantity = addProductInShoppingCartDto.Quantity;
                        sellerProductInActiveShoppingCart.TotalPrice = addProductInShoppingCartDto.Quantity * sellerProductDto.Price;


                        //mapping from SellerProductInShoppingCartDto to SellerProductInShoppingCart
                        var sellerProductInShoppingCartToUpdate = _genericMapper.MapSingle<SellerProductInShoppingCartDto, SellerProductInShoppingCart>(sellerProductInActiveShoppingCart);


                        //update product in shopping cart
                        _unitOfWork.SellerProductsInShoppingCartRepository.Update(sellerProductInShoppingCartToUpdate);
                        var IsProductInShoppingCartUpdated = await _completeAsync();

                        //check if update successfuly
                        if (!IsProductInShoppingCartUpdated) throw new Exception("Didnot update seller product in cart successfuly");
                        return;
                    }
                }
            }




            sellerProductInShoppingCart.ShoppingCartId = ActiveShoppingCartDto.Id;
            sellerProductInShoppingCart.TotalPrice = addProductInShoppingCartDto.Quantity * sellerProductDto.Price;

            //add product in shopping cart
            await _unitOfWork.SellerProductsInShoppingCartRepository.AddAsync(sellerProductInShoppingCart);

            //check if add successfuly
            var IsProductInShoppingCartAdded = await _completeAsync();
            if (!IsProductInShoppingCartAdded) throw new Exception("Didnot add seller product to cart successfuly");
        }

        public async Task<ShoppingCartDto> AddAsync(AddSellerProductToShoppingCartDto addProductInShoppingCartDto, long ShoppingCartId, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(addProductInShoppingCartDto, nameof(addProductInShoppingCartDto));
            ParamaterException.CheckIfLongIsBiggerThanZero(ShoppingCartId, nameof(ShoppingCartId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var UserDto = await _userService.FindByIdAsync(UserId);
            if (UserDto == null) throw new KeyNotFoundException($"Not found User. Id = {UserId}");


            var ActiveShoppingCartDto = await _shoppingCartService.FindActiveShoppingCartByUserIdAsync(UserId);
            if (ActiveShoppingCartDto is null || ActiveShoppingCartDto.Id != addProductInShoppingCartDto.ShoppingCartId)
                throw new KeyNotFoundException($"Not found cart or ActiveShoppingCart not equal shopping cart. Id: {addProductInShoppingCartDto.ShoppingCartId}");

            //add product in shopping cart
            await _AddAsync(addProductInShoppingCartDto, ActiveShoppingCartDto, UserId);

            //update cart info
            ActiveShoppingCartDto = await _shoppingCartService.FindByIdAsync(ShoppingCartId);
            return ActiveShoppingCartDto;

        }

        public async Task<ShoppingCartDto> AddRangeAsync(IEnumerable<AddSellerProductToShoppingCartDto> addProductsInShoppingCartsDtoList, long ShoppingCartId, string UserId)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(addProductsInShoppingCartsDtoList, nameof(addProductsInShoppingCartsDtoList));
            ParamaterException.CheckIfLongIsBiggerThanZero(ShoppingCartId, nameof(ShoppingCartId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));


            var UserDto = await _userService.FindByIdAsync(UserId);
            if (UserDto == null) throw new KeyNotFoundException($"Not found User. Id = {UserId}");


            var ActiveShoppingCartDto = await _shoppingCartService.FindActiveShoppingCartByUserIdAsync(UserId);
            if(ActiveShoppingCartDto is null || ActiveShoppingCartDto.Id != ShoppingCartId)
                throw new KeyNotFoundException($"Not found cart or ActiveShoppingCart not equal shopping cart. Id: {ShoppingCartId}");


            foreach (var addProductInShoppingCartDto in addProductsInShoppingCartsDtoList)
            {
                
                //add product in shopping cart
                await _AddAsync(addProductInShoppingCartDto, ActiveShoppingCartDto, UserId);
            }


            //update cart info
            ActiveShoppingCartDto = await _shoppingCartService.FindByIdAsync(ShoppingCartId);
            return ActiveShoppingCartDto;

        }

        public async Task<ShoppingCartDto> DeleteAsync(DeleteProductFromShoppingCartDto deleteProductFromShoppingCartDto, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(deleteProductFromShoppingCartDto, nameof(deleteProductFromShoppingCartDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));



            var UserDto = await _userService.FindByIdAsync(UserId);
            if (UserDto == null) throw new KeyNotFoundException($"Not found User. Id = {UserId}");


            var ActiveShoppingCart = await _unitOfWork.shoppingCartRepository.GetActiveShoppingCartByUserIdAsync(UserId);
            if (ActiveShoppingCart is null || ActiveShoppingCart.Id != deleteProductFromShoppingCartDto.ShoppingCartId)
                throw new KeyNotFoundException($"Not found cart or ActiveShoppingCart not equal shopping cart. Id: {deleteProductFromShoppingCartDto.ShoppingCartId}");


            var productInShopping = await _unitOfWork.SellerProductsInShoppingCartRepository.
                GetByIdAndShoppingCartIdAndUserIdAsync(deleteProductFromShoppingCartDto.SellerProductInShoppingCartId, deleteProductFromShoppingCartDto.ShoppingCartId, UserId);

            if (productInShopping is null)
                throw new KeyNotFoundException($"Not found Product in shopping cart. Id: {deleteProductFromShoppingCartDto.SellerProductInShoppingCartId}");

            await _unitOfWork.SellerProductsInShoppingCartRepository.DeleteAsync(deleteProductFromShoppingCartDto.SellerProductInShoppingCartId);

            await _completeAsync();


            //get cart
            var cart = await _shoppingCartService.FindByIdAsync(deleteProductFromShoppingCartDto.ShoppingCartId);
            return cart;
        }

        public async Task<ShoppingCartDto> DeleteRangeAsync(IEnumerable<DeleteProductFromShoppingCartDto> deleteProductFromShoppingCartDtos, string UserId)
        {
            ParamaterException.CheckIfIEnumerableIsNotNullOrEmpty(deleteProductFromShoppingCartDtos, nameof(deleteProductFromShoppingCartDtos));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var NewProductsInShoppingCartDtosList = new List<AddSellerProductToShoppingCartDto>();

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                foreach (var deleteProductFromShoppingCartDto in deleteProductFromShoppingCartDtos)
                {
                    var shoppingCart = await DeleteAsync(deleteProductFromShoppingCartDto, UserId);

                    if (shoppingCart == null)
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                        return null;
                    }
                }

                await _unitOfWork.CommitTransactionAsync();

                //get cart
                var cart = await _shoppingCartService.FindByIdAsync(deleteProductFromShoppingCartDtos.ElementAt(0).SellerProductInShoppingCartId);
                return cart;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<SellerProductInShoppingCartDto> FindByIdAndShoppingCartIdAndUserIdAsync(long productInShoppingCartId, long ShoppingCartId, string UserId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productInShoppingCartId, nameof(productInShoppingCartId));
            var productInShoppingCart = await _unitOfWork.SellerProductsInShoppingCartRepository.GetByIdAndShoppingCartIdAndUserIdAsync(productInShoppingCartId, ShoppingCartId, UserId);

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

        public async Task<ShoppingCartDto> UpdateAsync(long ProductInShoppingCartId, AddSellerProductToShoppingCartDto addProductToShoppingCartDto, string UserId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ProductInShoppingCartId, nameof(ProductInShoppingCartId));
            ParamaterException.CheckIfObjectIfNotNull(addProductToShoppingCartDto, nameof(addProductToShoppingCartDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var UserDto = await _userService.FindByIdAsync(UserId);
            if (UserDto == null) throw new KeyNotFoundException($"Not found User. Id = {UserId}");


            var ActiveShoppingCart = await _unitOfWork.shoppingCartRepository.GetActiveShoppingCartByUserIdAsync(UserId);
            if (ActiveShoppingCart is null || ActiveShoppingCart.Id != addProductToShoppingCartDto.ShoppingCartId)
                throw new KeyNotFoundException($"Not found cart or ActiveShoppingCart not equal shopping cart. Id: {addProductToShoppingCartDto.ShoppingCartId}");

            var productInShoppingCart = await _unitOfWork.SellerProductsInShoppingCartRepository.
                GetByIdAndShoppingCartIdAndUserIdAsync(ProductInShoppingCartId, addProductToShoppingCartDto.ShoppingCartId, UserId);
            if (productInShoppingCart is null) throw new KeyNotFoundException($"Not found Product in shopping cart. Id: {ProductInShoppingCartId}");



            var sellerProductDto = await _sellerProductService.FindByIdAsync(addProductToShoppingCartDto.SellerProductId);


            if (addProductToShoppingCartDto.Quantity > sellerProductDto.NumberInStock)
                addProductToShoppingCartDto.Quantity = sellerProductDto.NumberInStock;


            _genericMapper.MapSingle(addProductToShoppingCartDto, productInShoppingCart);

            productInShoppingCart.TotalPrice = addProductToShoppingCartDto.Quantity * sellerProductDto.Price;

            await _unitOfWork.SellerProductsInShoppingCartRepository.UpdateAsync(addProductToShoppingCartDto.ShoppingCartId, productInShoppingCart);

            var IsProductInShoppingCartUpdated = await _completeAsync();

            //get cart
            var cart = await _shoppingCartService.FindByIdAsync(addProductToShoppingCartDto.ShoppingCartId);
            return cart;
        }


    }
}
