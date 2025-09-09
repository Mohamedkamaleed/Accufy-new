CREATE DATABASE InventoryDb;
USE InventoryDb;


CREATE TABLE Warehouses (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    ShippingAddress NVARCHAR(255),
    IsActive BIT NOT NULL DEFAULT 1,
    IsPrimary BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE UNIQUE INDEX UX_PrimaryWarehouse ON Warehouses(IsPrimary)
WHERE IsPrimary = 1;

ALTER TABLE Warehouses Add UpdatedAt DATETIME, Status BIT NOT NULL DEFAULT 1;
ALTER TABLE Warehouses DROP COlumn IsActive

ALTER TABLE Warehouses ADD CONSTRAINT UQ_Warehouses_Name UNIQUE(Name);

-- ALTER TABLE Warehouses ADD CONSTRAINT DF_Warehouses_CreatedAt DEFAULT GETDATE() FOR CreatedAt;
-- ALTER TABLE Warehouses ADD CONSTRAINT DF_Warehouses_IsActive DEFAULT 1 FOR IsActive;
-- ALTER TABLE Warehouses ADD CONSTRAINT DF_Warehouses_Status DEFAULT 1 FOR Status;

CREATE UNIQUE NONCLUSTERED INDEX [UX_PrimaryWarehouse]
ON [dbo].[Warehouses]([IsPrimary] ASC) 
WHERE ([IsPrimary] = 1);


-- 1. Categories Table
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    ParentCategoryID INT NULL,
    CONSTRAINT FK_Categories_Parent FOREIGN KEY (ParentCategoryID) REFERENCES Categories(CategoryID)
);

-- 2. Brands Table
CREATE TABLE Brands (
    BrandID INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL
);

-- 3. Suppliers Table (assumed needed for FK)
CREATE TABLE Suppliers (
    SupplierID INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL
);

-- 4. UnitTemplates Table
CREATE TABLE UnitTemplates (
    TemplateID INT PRIMARY KEY IDENTITY,
    TemplateName NVARCHAR(100) NOT NULL,
    BaseUnitName NVARCHAR(50) NOT NULL,
    ShortName NVARCHAR(20),
    Status BIT NOT NULL DEFAULT 1
);

-- 5. UnitTemplateUnits Table
CREATE TABLE UnitTemplateUnits (
    UnitID INT PRIMARY KEY IDENTITY,
    TemplateID INT NOT NULL,
    UnitName NVARCHAR(50) NOT NULL,
    ShortName NVARCHAR(20),
    Factor DECIMAL(18, 6) NOT NULL CHECK (Factor > 0),
    CONSTRAINT FK_UnitTemplateUnits_Template FOREIGN KEY (TemplateID) REFERENCES UnitTemplates(TemplateID),
    CONSTRAINT UQ_UnitTemplateUnits_Template_Unit UNIQUE (TemplateID, UnitName)
);

-- 6. DefaultTaxes Table
CREATE TABLE DefaultTaxes (
    TaxID INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    TaxValue DECIMAL(10, 4) NOT NULL,
    Type NVARCHAR(20) CHECK (Type IN ('Percentage', 'Fixed')),
    Mode NVARCHAR(20) CHECK (Mode IN ('Included', 'Exclusive'))
);

-- 7. TaxProfiles Table
CREATE TABLE TaxProfiles (
    TaxProfileID INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL
);

-- 8. TaxProfileTaxes Table (many-to-many)
CREATE TABLE TaxProfileTaxes (
    TaxProfileID INT NOT NULL,
    TaxID INT NOT NULL,
    PRIMARY KEY (TaxProfileID, TaxID),
    FOREIGN KEY (TaxProfileID) REFERENCES TaxProfiles(TaxProfileID),
    FOREIGN KEY (TaxID) REFERENCES DefaultTaxes(TaxID)
);

-- 9. Products Table
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(200) NOT NULL,
    SKU NVARCHAR(100) UNIQUE,
    Description NVARCHAR(MAX),
    CategoryID INT NOT NULL,
    BrandID INT,
    SupplierID INT,
    Barcode NVARCHAR(100) UNIQUE,
    PurchasePrice DECIMAL(18, 2),
    SellingPrice DECIMAL(18, 2),
    MinPrice DECIMAL(18, 2),
    Discount DECIMAL(10, 2),
    DiscountType NVARCHAR(20),
    ProfitMargin DECIMAL(10, 2),
    TrackStock BIT DEFAULT 1,
    InitialStock DECIMAL(18, 2),
    LowStockThreshold DECIMAL(18, 2),
    Status BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),
    FOREIGN KEY (BrandID) REFERENCES Brands(BrandID),
    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID)
);

-- 10. Services Table
CREATE TABLE Services (
    ServiceID INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(200) NOT NULL,
    Code NVARCHAR(50) UNIQUE,
    Description NVARCHAR(MAX),
    CategoryID INT,
    SupplierID INT,
    PurchasePrice DECIMAL(18, 2),
    UnitPrice DECIMAL(18, 2),
    MinPrice DECIMAL(18, 2),
    Discount DECIMAL(10, 2),
    DiscountType NVARCHAR(20),
    ProfitMargin DECIMAL(10, 2),
    Status BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),
    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID)
);

-- 11. ItemGroups Table
CREATE TABLE ItemGroups (
    GroupID INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    CategoryID INT,
    BrandID INT,
    Description NVARCHAR(MAX),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),
    FOREIGN KEY (BrandID) REFERENCES Brands(BrandID)
);

-- 12. ItemGroupItems Table
CREATE TABLE ItemGroupItems (
    GroupItemID INT PRIMARY KEY IDENTITY,
    GroupID INT NOT NULL,
    ProductID INT NOT NULL,
    SKU NVARCHAR(100),
    PurchasePrice DECIMAL(18, 2),
    SellingPrice DECIMAL(18, 2),
    Barcode NVARCHAR(100),
    FOREIGN KEY (GroupID) REFERENCES ItemGroups(GroupID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- 13. ProductTaxProfiles Table
CREATE TABLE ProductTaxProfiles (
    ProductID INT NOT NULL,
    TaxProfileID INT NOT NULL,
    IsPrimary BIT DEFAULT 0,
    PRIMARY KEY (ProductID, TaxProfileID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (TaxProfileID) REFERENCES TaxProfiles(TaxProfileID)
);

-- 14. BarcodeSettings Table
CREATE TABLE BarcodeSettings (
    SettingID INT PRIMARY KEY IDENTITY,
    BarcodeType NVARCHAR(50) NOT NULL,
    EnableWeightEmbedded BIT DEFAULT 0,
    EmbeddedFormat NVARCHAR(100),
    WeightDivider DECIMAL(10, 2),
    CurrencyDivider DECIMAL(10, 2)
);

-- 15. CustomFields Table
CREATE TABLE CustomFields (
    FieldID INT PRIMARY KEY IDENTITY,
    FieldLabel NVARCHAR(100) NOT NULL,
    FieldType NVARCHAR(20) NOT NULL,
    IsRequired BIT DEFAULT 0
);

-- 16. ProductCustomFieldValues Table
CREATE TABLE ProductCustomFieldValues (
    ProductID INT NOT NULL,
    FieldID INT NOT NULL,
    Value NVARCHAR(MAX),
    PRIMARY KEY (ProductID, FieldID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (FieldID) REFERENCES CustomFields(FieldID)
);

-- ✅ 1. Selling Price ≥ Minimum Price (Rule #2)
-- Add SQL check constraint:
ALTER TABLE Products
ADD CONSTRAINT CK_Products_SellingPrice_MinPrice
CHECK (SellingPrice >= MinPrice);



--✅ 2. TrackStock → Warehouse Assignment (Rule #3)
--Since the schema does not currently include warehouse-product linking, you need a ProductWarehouses table (many-to-many), e.g.:

CREATE TABLE ProductWarehouses (
    ProductWarehouseID INT PRIMARY KEY IDENTITY,
    ProductID INT NOT NULL,
    WarehouseID INT NOT NULL,
    Quantity DECIMAL(18, 2) DEFAULT 0,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (WarehouseID) REFERENCES Warehouses(Id)
);


-- Insert into Categories
INSERT INTO Categories (Name, Description, ParentCategoryID) VALUES ('Category 1', 'Description 1', NULL);
INSERT INTO Categories (Name, Description, ParentCategoryID) VALUES ('Category 2', 'Description 2', NULL);
INSERT INTO Categories (Name, Description, ParentCategoryID) VALUES ('Category 3', 'Description 3', NULL);
INSERT INTO Categories (Name, Description, ParentCategoryID) VALUES ('Category 4', 'Description 4', NULL);
INSERT INTO Categories (Name, Description, ParentCategoryID) VALUES ('Category 5', 'Description 5', NULL);

-- Insert into Brands
INSERT INTO Brands (Name) VALUES ('Brand 1');
INSERT INTO Brands (Name) VALUES ('Brand 2');
INSERT INTO Brands (Name) VALUES ('Brand 3');
INSERT INTO Brands (Name) VALUES ('Brand 4');
INSERT INTO Brands (Name) VALUES ('Brand 5');

-- Insert into Suppliers
INSERT INTO Suppliers (Name) VALUES ('Supplier 1');
INSERT INTO Suppliers (Name) VALUES ('Supplier 2');
INSERT INTO Suppliers (Name) VALUES ('Supplier 3');
INSERT INTO Suppliers (Name) VALUES ('Supplier 4');
INSERT INTO Suppliers (Name) VALUES ('Supplier 5');

-- Insert into UnitTemplates
INSERT INTO UnitTemplates (TemplateName, BaseUnitName, ShortName, Status) VALUES ('Template 1', 'Gram', 'gm', 1);
INSERT INTO UnitTemplates (TemplateName, BaseUnitName, ShortName, Status) VALUES ('Template 2', 'Gram', 'gm', 1);
INSERT INTO UnitTemplates (TemplateName, BaseUnitName, ShortName, Status) VALUES ('Template 3', 'Gram', 'gm', 1);
INSERT INTO UnitTemplates (TemplateName, BaseUnitName, ShortName, Status) VALUES ('Template 4', 'Gram', 'gm', 1);
INSERT INTO UnitTemplates (TemplateName, BaseUnitName, ShortName, Status) VALUES ('Template 5', 'Gram', 'gm', 1);

-- Insert into UnitTemplateUnits
INSERT INTO UnitTemplateUnits (TemplateID, UnitName, ShortName, Factor) VALUES (1, 'Kilogram', 'kg', 1000);
INSERT INTO UnitTemplateUnits (TemplateID, UnitName, ShortName, Factor) VALUES (2, 'Kilogram', 'kg', 1000);
INSERT INTO UnitTemplateUnits (TemplateID, UnitName, ShortName, Factor) VALUES (3, 'Kilogram', 'kg', 1000);
INSERT INTO UnitTemplateUnits (TemplateID, UnitName, ShortName, Factor) VALUES (4, 'Kilogram', 'kg', 1000);
INSERT INTO UnitTemplateUnits (TemplateID, UnitName, ShortName, Factor) VALUES (5, 'Kilogram', 'kg', 1000);

-- Insert into DefaultTaxes
INSERT INTO DefaultTaxes (Name, TaxValue, Type, Mode) VALUES ('Tax 1', 10.00, 'Percentage', 'Exclusive');
INSERT INTO DefaultTaxes (Name, TaxValue, Type, Mode) VALUES ('Tax 2', 8.00, 'Percentage', 'Exclusive');
INSERT INTO DefaultTaxes (Name, TaxValue, Type, Mode) VALUES ('Tax 3', 12.00, 'Percentage', 'Exclusive');
INSERT INTO DefaultTaxes (Name, TaxValue, Type, Mode) VALUES ('Tax 4', 5.00, 'Percentage', 'Exclusive');
INSERT INTO DefaultTaxes (Name, TaxValue, Type, Mode) VALUES ('Tax 5', 15.00, 'Percentage', 'Exclusive');

-- Insert into TaxProfiles
INSERT INTO TaxProfiles (Name) VALUES ('Profile 1');
INSERT INTO TaxProfiles (Name) VALUES ('Profile 2');
INSERT INTO TaxProfiles (Name) VALUES ('Profile 3');
INSERT INTO TaxProfiles (Name) VALUES ('Profile 4');
INSERT INTO TaxProfiles (Name) VALUES ('Profile 5');

-- Insert into TaxProfileTaxes
INSERT INTO TaxProfileTaxes (TaxProfileID, TaxID) VALUES (1, 1);
INSERT INTO TaxProfileTaxes (TaxProfileID, TaxID) VALUES (2, 2);
INSERT INTO TaxProfileTaxes (TaxProfileID, TaxID) VALUES (3, 3);
INSERT INTO TaxProfileTaxes (TaxProfileID, TaxID) VALUES (4, 4);
INSERT INTO TaxProfileTaxes (TaxProfileID, TaxID) VALUES (5, 5);

-- Insert into Products
INSERT INTO Products (Name, SKU, Description, CategoryID, BrandID, SupplierID, Barcode, PurchasePrice, SellingPrice, MinPrice, Discount, DiscountType, ProfitMargin, TrackStock, InitialStock, LowStockThreshold, Status, CreatedAt, UpdatedAt)
VALUES ('Product 1', 'SKU001', 'Description 1', 1, 1, 1, 'BARCODE001', 100.00, 150.00, 90.00, 10.00, 'Percentage', 20.00, 1, 10, 2, 1, GETDATE(), GETDATE());

-- (Repeat INSERT INTO Products with different values for Product 2 to 5...)

-- Insert into Services
INSERT INTO Services (Name, Code, Description, CategoryID, SupplierID, PurchasePrice, UnitPrice, MinPrice, Discount, DiscountType, ProfitMargin, Status, CreatedAt, UpdatedAt)
VALUES ('Service 1', 'CODE001', 'Service Description 1', 1, 1, 50.00, 75.00, 40.00, 5.00, 'Fixed', 10.00, 1, GETDATE(), GETDATE());

-- (Repeat INSERT INTO Services for Service 2 to 5...)

-- Insert into ItemGroups
INSERT INTO ItemGroups (Name, CategoryID, BrandID, Description) VALUES ('Group 1', 1, 1, 'Group Description 1');
-- (Repeat for Group 2 to 5...)

-- Insert into ItemGroupItems
INSERT INTO ItemGroupItems (GroupID, ProductID, SKU, PurchasePrice, SellingPrice, Barcode)
VALUES (1, 1, 'SKU001', 100.00, 150.00, 'BARCODE001');
-- (Repeat for GroupID/ProductID 2 to 5...)

-- Insert into ProductTaxProfiles
INSERT INTO ProductTaxProfiles (ProductID, TaxProfileID, IsPrimary) VALUES (1, 1, 1);
-- (Repeat for Product 2 to 5...)

-- Insert into BarcodeSettings
INSERT INTO BarcodeSettings (BarcodeType, EnableWeightEmbedded, EmbeddedFormat, WeightDivider, CurrencyDivider)
VALUES ('Code 128', 0, 'XXXXXXXXWWWWWWPPPPN', 1000, 100);

-- Insert into CustomFields
INSERT INTO CustomFields (FieldLabel, FieldType, IsRequired) VALUES ('Field 1', 'Text', 0);
-- (Repeat for Field 2 to 5...)

-- Insert into ProductCustomFieldValues
INSERT INTO ProductCustomFieldValues (ProductID, FieldID, Value) VALUES (1, 1, 'Value 1');
-- (Repeat for Product 2 to 5...)


