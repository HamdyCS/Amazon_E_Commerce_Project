
-- People Table
CREATE TABLE People (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(255),
    LastName NVARCHAR(255),
    DateOfBirth DATE
);

-- PeopleAddresses Table
CREATE TABLE PeopleAddresses (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Address NVARCHAR(500),
    CityId BIGINT,
    PersonId BIGINT
);

-- Phones Table
CREATE TABLE Phones (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    PhoneNumber NVARCHAR(50),
    PersonId BIGINT
);

-- ProductCategories Table
CREATE TABLE ProductCategories (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Name_En NVARCHAR(255) UNIQUE,
    Name_Ar NVARCHAR(255) UNIQUE,
    Description_En NVARCHAR(1000),
    Description_Ar NVARCHAR(1000),
    UserId BIGINT
);

-- ProductCategoryImages Table
CREATE TABLE ProductCategoryImages (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Image NVARCHAR(MAX),
    ProductCategoryId BIGINT
);

-- Brands Table
CREATE TABLE Brands (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Name_En NVARCHAR(255),
    Name_Ar NVARCHAR(255),
    UserId BIGINT
);

-- Products Table
CREATE TABLE Products (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Name_En NVARCHAR(255),
    Name_Ar NVARCHAR(255),
    Size NVARCHAR(50),
    Color NVARCHAR(50),
    Height DECIMAL(10, 2),
    Length DECIMAL(10, 2),
    UserId BIGINT,
    ProductCategoryId BIGINT,
    BrandId BIGINT
);

-- ProductImages Table
CREATE TABLE ProductImages (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Image NVARCHAR(MAX),
    ProductId BIGINT
);

-- SellerProducts Table
CREATE TABLE SellerProducts (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    SellerId BIGINT,
    Price DECIMAL(10, 2),
    NumberInStock INT,
    ProductId BIGINT
);

-- ProductReviews Table
CREATE TABLE ProductReviews (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    NumberOfStars INT,
    Message NVARCHAR(1000),
    CreatedAt DATETIME,
    ReviewedBy BIGINT,
    SellerProductId BIGINT
);

-- RefreshTokens Table
CREATE TABLE RefreshTokens (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Token NVARCHAR(MAX),
    CreatedAt DATETIME,
    ExpiresAt DATETIME,
    UserId BIGINT
);

-- ShoppingCarts Table
CREATE TABLE ShoppingCarts (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    UserId BIGINT
);

-- ProductsInShoppingCarts Table
CREATE TABLE ProductsInShoppingCarts (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Number INT,
    TotalPrice DECIMAL(10, 2),
    SellerProductId BIGINT,
    ShoppingCartId BIGINT
);

-- Cities Table
CREATE TABLE Cities (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Name_En NVARCHAR(255),
    Name_Ar NVARCHAR(255)
);

-- ShippingCosts Table
CREATE TABLE ShippingCosts (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Price DECIMAL(10, 2),
    UserId BIGINT,
    CityId BIGINT
);

-- ApplicationTypes Table
CREATE TABLE ApplicationTypes (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255),
    Description NVARCHAR(1000)
);

-- Application Table
CREATE TABLE Application (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    CreatedAt DATETIME,
    UserId BIGINT,
    ApplicationTypeId BIGINT
);

-- Deliveries Table
CREATE TABLE Deliveries (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    UserId BIGINT
);

-- CitiesWhereDeliveiesWorks Table
CREATE TABLE CitiesWhereDeliveiesWorks (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    CityId BIGINT,
    DeliveryId BIGINT
);

-- ApplicationOrdersTypes Table
CREATE TABLE ApplicationOrdersTypes (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255),
    Description NVARCHAR(1000)
);

-- ApplicationOrders Table
CREATE TABLE ApplicationOrders (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    ApplicationId BIGINT,
    ApplicationOrderTypeId BIGINT,
    ShippingCost DECIMAL(10, 2),
    ShoppingCartId BIGINT,
    PersonAddress NVARCHAR(500),
    PaymentId BIGINT,
    DeliveryId BIGINT
);

-- Payments Table
CREATE TABLE Payments (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    TotalPrice DECIMAL(10, 2),
    CreatedAt DATETIME,
    PaymentTypeId BIGINT
);

-- PaymentsTypes Table
CREATE TABLE PaymentsTypes (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255)
);
-- Adding Foreign Keys after all tables are created
ALTER TABLE PeopleAddresses ADD CONSTRAINT FK_PeopleAddresses_CityId FOREIGN KEY (CityId) REFERENCES Cities(Id);
ALTER TABLE PeopleAddresses ADD CONSTRAINT FK_PeopleAddresses_PersonId FOREIGN KEY (PersonId) REFERENCES People(Id);

ALTER TABLE Phones ADD CONSTRAINT FK_Phones_PersonId FOREIGN KEY (PersonId) REFERENCES People(Id);

ALTER TABLE ProductCategoryImages ADD CONSTRAINT FK_ProductCategoryImages_ProductCategoryId FOREIGN KEY (ProductCategoryId) REFERENCES ProductCategories(Id);

ALTER TABLE Products ADD CONSTRAINT FK_Products_ProductCategoryId FOREIGN KEY (ProductCategoryId) REFERENCES ProductCategories(Id);
ALTER TABLE Products ADD CONSTRAINT FK_Products_BrandId FOREIGN KEY (BrandId) REFERENCES Brands(Id);

ALTER TABLE ProductImages ADD CONSTRAINT FK_ProductImages_ProductId FOREIGN KEY (ProductId) REFERENCES Products(Id);

ALTER TABLE SellerProducts ADD CONSTRAINT FK_SellerProducts_ProductId FOREIGN KEY (ProductId) REFERENCES Products(Id);

ALTER TABLE ProductReviews ADD CONSTRAINT FK_ProductReviews_SellerProductId FOREIGN KEY (SellerProductId) REFERENCES SellerProducts(Id);

ALTER TABLE ProductsInShoppingCarts ADD CONSTRAINT FK_ProductsInShoppingCarts_SellerProductId FOREIGN KEY (SellerProductId) REFERENCES SellerProducts(Id);
ALTER TABLE ProductsInShoppingCarts ADD CONSTRAINT FK_ProductsInShoppingCarts_ShoppingCartId FOREIGN KEY (ShoppingCartId) REFERENCES ShoppingCarts(Id);

ALTER TABLE ShippingCosts ADD CONSTRAINT FK_ShippingCosts_CityId FOREIGN KEY (CityId) REFERENCES Cities(Id);

ALTER TABLE Application ADD CONSTRAINT FK_Application_ApplicationTypeId FOREIGN KEY (ApplicationTypeId) REFERENCES ApplicationTypes(Id);

ALTER TABLE CitiesWhereDeliveiesWorks ADD CONSTRAINT FK_CitiesWhereDeliveiesWorks_CityId FOREIGN KEY (CityId) REFERENCES Cities(Id);
ALTER TABLE CitiesWhereDeliveiesWorks ADD CONSTRAINT FK_CitiesWhereDeliveiesWorks_DeliveryId FOREIGN KEY (DeliveryId) REFERENCES Deliveries(Id);

ALTER TABLE ApplicationOrders ADD CONSTRAINT FK_ApplicationOrders_ApplicationId FOREIGN KEY (ApplicationId) REFERENCES Application(Id);
ALTER TABLE ApplicationOrders ADD CONSTRAINT FK_ApplicationOrders_ApplicationOrderTypeId FOREIGN KEY (ApplicationOrderTypeId) REFERENCES ApplicationOrdersTypes(Id);
ALTER TABLE ApplicationOrders ADD CONSTRAINT FK_ApplicationOrders_ShoppingCartId FOREIGN KEY (ShoppingCartId) REFERENCES ShoppingCarts(Id);
ALTER TABLE ApplicationOrders ADD CONSTRAINT FK_ApplicationOrders_PaymentId FOREIGN KEY (PaymentId) REFERENCES Payments(Id);
ALTER TABLE ApplicationOrders ADD CONSTRAINT FK_ApplicationOrders_DeliveryId FOREIGN KEY (DeliveryId) REFERENCES Deliveries(Id);

ALTER TABLE Payments ADD CONSTRAINT FK_Payments_PaymentTypeId FOREIGN KEY (PaymentTypeId) REFERENCES PaymentsTypes(Id);
