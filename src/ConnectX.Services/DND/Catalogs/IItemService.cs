using ConnectX.Domain.Services.DND.Catalogs.Items;

namespace ConnectX.Services.DND.Catalogs;

public interface IItemService
{
    Task<ItemPageDto> GetPageAsync(int pageNumber = 1, int pageSize = 24, string? search = null, CancellationToken cancellationToken = default);
    Task<ItemDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(ItemWriteDto input, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(ItemWriteDto input, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid id, ItemWriteDto input, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
