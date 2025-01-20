select CountOfDeliveryApplicationOrder = 
(
SELECT  count = Count(ApplicationOrders.Id)  
FROM            Products INNER JOIN
                         SellerProducts ON Products.Id = SellerProducts.ProductId INNER JOIN
                         ProductsInShoppingCarts ON SellerProducts.Id = ProductsInShoppingCarts.SellerProductId
						 INNER JOIN
                         ShoppingCarts ON ProductsInShoppingCarts.ShoppingCartId = ShoppingCarts.Id INNER JOIN
                         ApplicationOrders ON ShoppingCarts.Id = ApplicationOrders.ShoppingCartId
						 where ApplicationOrders.ApplicationOrderTypeId = 3 and Products.Id = P.Id 
),p.* from Products as p
where p.IsDeleted = 0
order by CountOfDeliveryApplicationOrder desc