using ConnectX.Domain.Services.DND.Catalogs.Spells;

namespace ConnectX.Services.DND.Catalogs;

public interface ISpellService
{
    Task<SpellPageDto> GetPageAsync(int pageNumber = 1, int pageSize = 24, string? search = null, CancellationToken cancellationToken = default);
    Task<SpellDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(SpellWriteDto input, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(SpellWriteDto input, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid id, SpellWriteDto input, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
