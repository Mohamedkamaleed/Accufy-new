using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WarehouseManagement.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedMultipleTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Warehouses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Brands",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "DefaultTaxes",
                columns: table => new
                {
                    TaxID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TaxValue = table.Column<decimal>(type: "numeric(10,4)", nullable: false),
                    Type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Mode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultTaxes", x => x.TaxID);
                });

            migrationBuilder.CreateTable(
                name: "ItemGroups",
                columns: table => new
                {
                    GroupID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CategoryID = table.Column<int>(type: "integer", nullable: true),
                    BrandID = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemGroups", x => x.GroupID);
                    table.ForeignKey(
                        name: "FK_ItemGroups_Brands_BrandID",
                        column: x => x.BrandID,
                        principalTable: "Brands",
                        principalColumn: "BrandID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ItemGroups_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CategoryID = table.Column<int>(type: "integer", nullable: true),
                    SupplierID = table.Column<int>(type: "integer", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    MinPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Discount = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    DiscountType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ProfitMargin = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ServiceID);
                    table.ForeignKey(
                        name: "FK_Services_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Services_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TaxProfiles",
                columns: table => new
                {
                    TaxProfileID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TaxRate = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxProfiles", x => x.TaxProfileID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SKU = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CategoryID = table.Column<int>(type: "integer", nullable: false),
                    BrandID = table.Column<int>(type: "integer", nullable: true),
                    SupplierID = table.Column<int>(type: "integer", nullable: true),
                    Barcode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    SellingPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    MinPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Discount = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    DiscountType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ProfitMargin = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    TrackStock = table.Column<bool>(type: "boolean", nullable: false),
                    InitialStock = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    LowStockThreshold = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GroupID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandID",
                        column: x => x.BrandID,
                        principalTable: "Brands",
                        principalColumn: "BrandID");
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_ItemGroups_GroupID",
                        column: x => x.GroupID,
                        principalTable: "ItemGroups",
                        principalColumn: "GroupID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Products_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID");
                });

            migrationBuilder.CreateTable(
                name: "TaxProfileTaxes",
                columns: table => new
                {
                    TaxProfileID = table.Column<int>(type: "integer", nullable: false),
                    TaxID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxProfileTaxes", x => new { x.TaxProfileID, x.TaxID });
                    table.ForeignKey(
                        name: "FK_TaxProfileTaxes_DefaultTaxes_TaxID",
                        column: x => x.TaxID,
                        principalTable: "DefaultTaxes",
                        principalColumn: "TaxID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaxProfileTaxes_TaxProfiles_TaxProfileID",
                        column: x => x.TaxProfileID,
                        principalTable: "TaxProfiles",
                        principalColumn: "TaxProfileID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemGroupItems",
                columns: table => new
                {
                    GroupItemID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupID = table.Column<int>(type: "integer", nullable: false),
                    ProductID = table.Column<int>(type: "integer", nullable: false),
                    SKU = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    SellingPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Barcode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemGroupItems", x => x.GroupItemID);
                    table.ForeignKey(
                        name: "FK_ItemGroupItems_ItemGroups_GroupID",
                        column: x => x.GroupID,
                        principalTable: "ItemGroups",
                        principalColumn: "GroupID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemGroupItems_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTaxProfiles",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "integer", nullable: false),
                    TaxProfileID = table.Column<int>(type: "integer", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTaxProfiles", x => new { x.ProductID, x.TaxProfileID });
                    table.ForeignKey(
                        name: "FK_ProductTaxProfiles_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTaxProfiles_TaxProfiles_TaxProfileID",
                        column: x => x.TaxProfileID,
                        principalTable: "TaxProfiles",
                        principalColumn: "TaxProfileID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Name",
                table: "Brands",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DefaultTaxes_Name",
                table: "DefaultTaxes",
                column: "Name",
                unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_ItemGroupItems_Barcode",
            //    table: "ItemGroupItems",
            //    column: "Barcode",
            //    unique: true,
            //    filter: "[Barcode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGroupItems_GroupID_ProductID",
                table: "ItemGroupItems",
                columns: new[] { "GroupID", "ProductID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemGroupItems_ProductID",
                table: "ItemGroupItems",
                column: "ProductID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ItemGroupItems_SKU",
            //    table: "ItemGroupItems",
            //    column: "SKU",
            //    unique: true,
            //    filter: "[SKU] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGroups_BrandID",
                table: "ItemGroups",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGroups_CategoryID",
                table: "ItemGroups",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGroups_Name",
                table: "ItemGroups",
                column: "Name",
                unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Products_Barcode",
            //    table: "Products",
            //    column: "Barcode",
            //    unique: true,
            //    filter: "[Barcode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandID",
                table: "Products",
                column: "BrandID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_GroupID",
                table: "Products",
                column: "GroupID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Products_SKU",
            //    table: "Products",
            //    column: "SKU",
            //    unique: true,
            //    filter: "[SKU] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierID",
                table: "Products",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTaxProfiles_TaxProfileID",
                table: "ProductTaxProfiles",
                column: "TaxProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_Services_CategoryID",
                table: "Services",
                column: "CategoryID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Services_Code",
            //    table: "Services",
            //    column: "Code",
            //    unique: true,
            //    filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Services_SupplierID",
                table: "Services",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_TaxProfiles_Name",
                table: "TaxProfiles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaxProfileTaxes_TaxID",
                table: "TaxProfileTaxes",
                column: "TaxID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemGroupItems");

            migrationBuilder.DropTable(
                name: "ProductTaxProfiles");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "TaxProfileTaxes");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "DefaultTaxes");

            migrationBuilder.DropTable(
                name: "TaxProfiles");

            migrationBuilder.DropTable(
                name: "ItemGroups");

            migrationBuilder.DropIndex(
                name: "IX_Brands_Name",
                table: "Brands");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Warehouses",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Brands",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
