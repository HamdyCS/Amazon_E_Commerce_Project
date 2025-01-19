using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Drawing.Printing;

namespace DataAccessLayer.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(AppDbContext context, ILogger<ProductRepository> logger) : base(context, logger, "Products")
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllOrderByBestSellerDescAsync()
        {
            try
            {
                List<Product>? productsList = new();


                using (var commend = _context.Database.GetDbConnection().CreateCommand())
                {
                    if (commend == null) return null;
                    await _context.Database.GetDbConnection().OpenAsync();

                    var sql = "select CountOfDeliveryApplicationOrder = \r\n(\r\nSELECT  count = Count(ApplicationOrders.Id)  \r\nFROM            Products INNER JOIN\r\n                         SellerProducts ON Products.Id = SellerProducts.ProductId INNER JOIN\r\n                         ProductsInShoppingCarts ON SellerProducts.Id = ProductsInShoppingCarts.SellerProductId\r\n\t\t\t\t\t\t INNER JOIN\r\n                         ShoppingCarts ON ProductsInShoppingCarts.ShoppingCartId = ShoppingCarts.Id INNER JOIN\r\n                         ApplicationOrders ON ShoppingCarts.Id = ApplicationOrders.ShoppingCartId\r\n\t\t\t\t\t\t where ApplicationOrders.ApplicationOrderTypeId = 3 and Products.Id = P.Id \r\n),p.* from Products as p\r\nwhere p.IsDeleted = 0\r\norder by CountOfDeliveryApplicationOrder desc";
                    commend.CommandText = sql;

                    using (var reader = commend.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Product product = new()
                            {
                                Id = Convert.ToInt64(reader["Id"]),
                                NameEn = Convert.ToString(reader["Name_EN"]),
                                NameAr = Convert.ToString(reader["Name_En"]),
                                Size = Convert.ToString(reader["Size"]),
                                Color = Convert.ToString(reader["Color"]),
                                Height = Convert.ToDecimal(reader["Height"]),
                                Length = Convert.ToDecimal(reader["Length"]),
                                CreatedBy = Convert.ToString(reader["CreatedBy"]),
                                ProductSubCategoryId = Convert.ToInt64(reader["ProductSubCategoryId"]),
                                BrandId = Convert.ToInt64(reader["BrandId"]),
                                IsDeleted = Convert.ToBoolean(reader["IsDeleted"]),

                            };

                            if (reader["DateOfDeletion"] == DBNull.Value)
                                product.DateOfDeletion = null;
                            else
                                product.DateOfDeletion = Convert.ToDateTime(reader["DateOfDeletion"]);


                            if (reader["DescriptionAr"] == DBNull.Value)
                                product.DescriptionEn = null;
                            else

                                product.DescriptionAr = Convert.ToString(reader["DescriptionAr"]);

                            if (reader["DescriptionEn"] == DBNull.Value)
                                product.DescriptionEn = null;
                            else
                                product.DescriptionEn = Convert.ToString(reader["DescriptionEn"]);


                            productsList.Add(product);
                        }
                    }
                }

                return productsList;

            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
            finally
            {
                await _context.Database.GetDbConnection().CloseAsync();

            }
        }

        public async Task<Product> GetByNameArAsync(string NameAr)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(NameAr, nameof(NameAr));

            try
            {
                var product = await _context.Products.
                    FirstOrDefaultAsync(e => e.NameAr == NameAr);
                return product;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<Product> GetByNameEnAsync(string NameEn)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(NameEn, nameof(NameEn));

            try
            {
                var product = await _context.Products.
                    FirstOrDefaultAsync(e => e.NameEn == NameEn);
                return product;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<Product>> GetPagedOrderByBestSellerDescAsync(int pageNumber, int pageSize)
        {
            ParamaterException.CheckIfIntIsBiggerThanZero(pageNumber, nameof(pageNumber));
            ParamaterException.CheckIfIntIsBiggerThanZero(pageSize, nameof(pageSize));

            try
            {
                List<Product>? productsList = new();


                using (var commend = _context.Database.GetDbConnection().CreateCommand())
                {
                    if (commend == null) return null;
                    await _context.Database.GetDbConnection().OpenAsync();

                    var sql = "select CountOfDeliveryApplicationOrder = \r\n(\r\nSELECT  count = Count(ApplicationOrders.Id)  \r\nFROM            Products INNER JOIN\r\n                         SellerProducts ON Products.Id = SellerProducts.ProductId INNER JOIN\r\n                         ProductsInShoppingCarts ON SellerProducts.Id = ProductsInShoppingCarts.SellerProductId\r\n\t\t\t\t\t\t INNER JOIN\r\n                         ShoppingCarts ON ProductsInShoppingCarts.ShoppingCartId = ShoppingCarts.Id INNER JOIN\r\n                         ApplicationOrders ON ShoppingCarts.Id = ApplicationOrders.ShoppingCartId\r\n\t\t\t\t\t\t where ApplicationOrders.ApplicationOrderTypeId = 3 and Products.Id = P.Id \r\n),p.* from Products as p\r\nwhere p.IsDeleted = 0\r\norder by CountOfDeliveryApplicationOrder desc\r\n\r\noffset (@PageNumber-1)*@PageSize rows \r\nfetch next @PageSize rows only ";

                    commend.CommandText = sql;

                    commend.Parameters.Add(new SqlParameter("@PageNumber",pageNumber));
                    commend.Parameters.Add(new SqlParameter("@PageSize", pageSize));

                    using (var reader = commend.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Product product = new()
                            {
                                Id = Convert.ToInt64(reader["Id"]),
                                NameEn = Convert.ToString(reader["Name_EN"]),
                                NameAr = Convert.ToString(reader["Name_En"]),
                                Size = Convert.ToString(reader["Size"]),
                                Color = Convert.ToString(reader["Color"]),
                                Height = Convert.ToDecimal(reader["Height"]),
                                Length = Convert.ToDecimal(reader["Length"]),
                                CreatedBy = Convert.ToString(reader["CreatedBy"]),
                                ProductSubCategoryId = Convert.ToInt64(reader["ProductSubCategoryId"]),
                                BrandId = Convert.ToInt64(reader["BrandId"]),
                                IsDeleted = Convert.ToBoolean(reader["IsDeleted"]),

                            };

                            if (reader["DateOfDeletion"] == DBNull.Value)
                                product.DateOfDeletion = null;
                            else
                                product.DateOfDeletion = Convert.ToDateTime(reader["DateOfDeletion"]);


                            if (reader["DescriptionAr"] == DBNull.Value)
                                product.DescriptionEn = null;
                            else

                                product.DescriptionAr = Convert.ToString(reader["DescriptionAr"]);

                            if (reader["DescriptionEn"] == DBNull.Value)
                                product.DescriptionEn = null;
                            else
                                product.DescriptionEn = Convert.ToString(reader["DescriptionEn"]);


                            productsList.Add(product);
                        }
                    }
                }

                return productsList;

            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
            finally
            {
                await _context.Database.GetDbConnection().CloseAsync();

            }
        }

        public async Task<IEnumerable<Product>> SearchByNameArAsync(string NameAr, int pageSize)
        {

            try
            {
                //var products = await _context.Products.FromSqlInterpolated($"select * from Products where Name_Ar like N'%{NameAr}%' order by Name_Ar Offset 0 rows fetch next {pageSize} rows only")
                //   .ToListAsync();

                var products = await _context.Products.Where(e => e.NameAr.Contains(NameAr)).Take(pageSize)
                    .ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }

        public async Task<IEnumerable<Product>> SearchByNameEnAsync(string NameEn, int pageSize)
        {

            try
            {
                //var products = await _context.Products.FromSqlInterpolated($"select * from Products where Name_En like N'%{NameEn}%'")
                //    .ToListAsync();

                var products = await _context.Products.Where(e => e.NameEn.Contains(NameEn)).Take(pageSize)
                    .ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                throw HandleDatabaseException(ex);
            }
        }
    }
}
