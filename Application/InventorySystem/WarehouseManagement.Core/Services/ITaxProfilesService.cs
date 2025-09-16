using WarehouseManagement.Core.Common;
using WarehouseManagement.Core.Entities;
using WarehouseManagement.Core.Repositories;
using WarehouseManagement.Core.ViewModels;

namespace WarehouseManagement.Core.Services
{
    public interface ITaxProfilesService
    {
        Task<Result<TaxProfile>> CreateTaxProfileAsync(TaxProfileCreateViewModel model);
        Task<Result<TaxProfile>> UpdateTaxProfileAsync(int id, TaxProfileEditViewModel model);
        Task<Result> DeleteTaxProfileAsync(int id);
        Task<TaxProfile?> GetTaxProfileByIdAsync(int id);
        Task<TaxProfile?> GetTaxProfileByIdWithTaxesAsync(int id);
        Task<IEnumerable<TaxProfile>> GetAllTaxProfilesAsync();
    }
    public class TaxProfilesService : ITaxProfilesService
    {
        private readonly ITaxProfilesRepository _taxProfilesRepository;
        private readonly ITaxProfileTaxesRepository _taxProfileTaxesRepository;
        private readonly IDefaultTaxesRepository _defaultTaxesRepository;

        public TaxProfilesService(
            ITaxProfilesRepository taxProfilesRepository,
            ITaxProfileTaxesRepository taxProfileTaxesRepository,
            IDefaultTaxesRepository defaultTaxesRepository)
        {
            _taxProfilesRepository = taxProfilesRepository;
            _taxProfileTaxesRepository = taxProfileTaxesRepository;
            _defaultTaxesRepository = defaultTaxesRepository;
        }

        public async Task<Result<TaxProfile>> CreateTaxProfileAsync(TaxProfileCreateViewModel model)
        {
            var existing = await _taxProfilesRepository.GetByNameAsync(model.Name);
            if (existing != null)
                return Result<TaxProfile>.Failure("Tax profile name already exists");

            var taxProfile = new TaxProfile
            {
                Name = model.Name
            };

            await _taxProfilesRepository.AddAsync(taxProfile);

            // Add taxes to the profile
            if (model.TaxIds != null && model.TaxIds.Any())
            {
                foreach (var taxId in model.TaxIds)
                {
                    var taxExists = await _defaultTaxesRepository.GetByIdAsync(taxId);
                    if (taxExists != null)
                    {
                        await _taxProfileTaxesRepository.AddTaxToProfileAsync(taxProfile.TaxProfileID, taxId);
                    }
                }
            }

            return Result<TaxProfile>.Success(taxProfile);
        }

        public async Task<Result<TaxProfile>> UpdateTaxProfileAsync(int id, TaxProfileEditViewModel model)
        {
            var taxProfile = await _taxProfilesRepository.GetByIdAsync(id);
            if (taxProfile == null)
                return Result<TaxProfile>.Failure("Tax profile not found");

            // Check for duplicate name (excluding current tax profile)
            if (model.Name != taxProfile.Name)
            {
                var existing = await _taxProfilesRepository.GetByNameAsync(model.Name);
                if (existing != null && existing.TaxProfileID != id)
                    return Result<TaxProfile>.Failure("Tax profile name already exists");
            }

            taxProfile.Name = model.Name;
            await _taxProfilesRepository.UpdateAsync(taxProfile);

            // Update taxes for the profile
            if (model.TaxIds != null)
            {
                // Get current taxes
                var currentTaxes = await _taxProfileTaxesRepository.GetByTaxProfileIdAsync(id);
                var currentTaxIds = currentTaxes.Select(t => t.TaxID).ToList();

                // Add new taxes
                foreach (var taxId in model.TaxIds.Except(currentTaxIds))
                {
                    var taxExists = await _defaultTaxesRepository.GetByIdAsync(taxId);
                    if (taxExists != null)
                    {
                        await _taxProfileTaxesRepository.AddTaxToProfileAsync(id, taxId);
                    }
                }

                // Remove taxes that are no longer selected
                foreach (var taxId in currentTaxIds.Except(model.TaxIds))
                {
                    await _taxProfileTaxesRepository.RemoveTaxFromProfileAsync(id, taxId);
                }
            }

            return Result<TaxProfile>.Success(taxProfile);
        }

        public async Task<Result> DeleteTaxProfileAsync(int id)
        {
            var taxProfile = await _taxProfilesRepository.GetByIdAsync(id);
            if (taxProfile == null)
                return Result.Failure("Tax profile not found");

            try
            {
                await _taxProfilesRepository.DeleteAsync(taxProfile);
                return Result.Success();
            }
            catch (InvalidOperationException ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<TaxProfile?> GetTaxProfileByIdAsync(int id)
        {
            return await _taxProfilesRepository.GetByIdAsync(id);
        }

        public async Task<TaxProfile?> GetTaxProfileByIdWithTaxesAsync(int id)
        {
            return await _taxProfilesRepository.GetByIdWithTaxesAsync(id);
        }

        public async Task<IEnumerable<TaxProfile>> GetAllTaxProfilesAsync()
        {
            return await _taxProfilesRepository.GetAllAsync();
        }
    }
}
