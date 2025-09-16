using WarehouseManagement.Core.Common;
using WarehouseManagement.Core.Entities;
using WarehouseManagement.Core.Repositories;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Core.Services
{
    public interface IItemGroupItemsService
    {
        Task<Result<ItemGroupItem>> CreateItemGroupItemAsync(ItemGroupItemCreateViewModel model);
        Task<Result<ItemGroupItem>> UpdateItemGroupItemAsync(int id, ItemGroupItemEditViewModel model);
        Task<Result> DeleteItemGroupItemAsync(int id);
        Task<ItemGroupItem?> GetItemGroupItemByIdAsync(int id);
        Task<IEnumerable<ItemGroupItem>> GetAllItemGroupItemsAsync();
        Task<IEnumerable<ItemGroupItem>> GetItemGroupItemsByGroupAsync(int groupId);
        Task<IEnumerable<ItemGroupItem>> GetItemGroupItemsByProductAsync(int productId);
    }

    public class ItemGroupItemsService : IItemGroupItemsService
    {
        private readonly IItemGroupItemsRepository _repository;
        private readonly IItemGroupsRepository _itemGroupsRepository;
        private readonly IProductsRepository _productsRepository;

        public ItemGroupItemsService(
            IItemGroupItemsRepository repository,
            IItemGroupsRepository itemGroupsRepository,
            IProductsRepository productsRepository)
        {
            _repository = repository;
            _itemGroupsRepository = itemGroupsRepository;
            _productsRepository = productsRepository;
        }

        public async Task<Result<ItemGroupItem>> CreateItemGroupItemAsync(ItemGroupItemCreateViewModel model)
        {
            // Validate item group exists
            var itemGroup = await _itemGroupsRepository.GetByIdAsync(model.GroupID);
            if (itemGroup == null)
                return Result<ItemGroupItem>.Failure("Item group not found");

            // Validate product exists
            var product = await _productsRepository.GetByIdAsync(model.ProductID);
            if (product == null)
                return Result<ItemGroupItem>.Failure("Product not found");

            // Check if product is already in the group
            var existing = await _repository.GetByGroupAndProductAsync(model.GroupID, model.ProductID);
            if (existing != null)
                return Result<ItemGroupItem>.Failure("Product is already in this item group");

            var itemGroupItem = new ItemGroupItem
            {
                GroupID = model.GroupID,
                ProductID = model.ProductID,
                SKU = model.SKU,
                PurchasePrice = model.PurchasePrice,
                SellingPrice = model.SellingPrice,
                Barcode = model.Barcode
            };

            await _repository.AddAsync(itemGroupItem);

            return Result<ItemGroupItem>.Success(itemGroupItem);
        }

        public async Task<Result<ItemGroupItem>> UpdateItemGroupItemAsync(int id, ItemGroupItemEditViewModel model)
        {
            var itemGroupItem = await _repository.GetByIdAsync(id);
            if (itemGroupItem == null)
                return Result<ItemGroupItem>.Failure("Item group item not found");

            // Check for duplicate SKU if provided
            if (!string.IsNullOrEmpty(model.SKU) && model.SKU != itemGroupItem.SKU)
            {
                var existingWithSku = (await _repository.GetAllAsync())
                    .FirstOrDefault(i => i.SKU == model.SKU && i.GroupItemID != id);

                if (existingWithSku != null)
                    return Result<ItemGroupItem>.Failure("SKU already exists");
            }

            // Check for duplicate barcode if provided
            if (!string.IsNullOrEmpty(model.Barcode) && model.Barcode != itemGroupItem.Barcode)
            {
                var existingWithBarcode = (await _repository.GetAllAsync())
                    .FirstOrDefault(i => i.Barcode == model.Barcode && i.GroupItemID != id);

                if (existingWithBarcode != null)
                    return Result<ItemGroupItem>.Failure("Barcode already exists");
            }

            itemGroupItem.SKU = model.SKU;
            itemGroupItem.PurchasePrice = model.PurchasePrice;
            itemGroupItem.SellingPrice = model.SellingPrice;
            itemGroupItem.Barcode = model.Barcode;

            await _repository.UpdateAsync(itemGroupItem);

            return Result<ItemGroupItem>.Success(itemGroupItem);
        }

        public async Task<Result> DeleteItemGroupItemAsync(int id)
        {
            var itemGroupItem = await _repository.GetByIdAsync(id);
            if (itemGroupItem == null)
                return Result.Failure("Item group item not found");

            await _repository.DeleteAsync(itemGroupItem);

            return Result.Success();
        }

        public async Task<ItemGroupItem?> GetItemGroupItemByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ItemGroupItem>> GetAllItemGroupItemsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<ItemGroupItem>> GetItemGroupItemsByGroupAsync(int groupId)
        {
            return await _repository.GetByGroupIdAsync(groupId);
        }

        public async Task<IEnumerable<ItemGroupItem>> GetItemGroupItemsByProductAsync(int productId)
        {
            return await _repository.GetByProductIdAsync(productId);
        }
    }
}