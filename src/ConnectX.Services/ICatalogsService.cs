using ConnectX.Domain.Services.Catalogs.Spells;

namespace ConnectX.Services
{
    public interface ICatalogsService
    {
        Task<IEnumerable<SpellDto>> GetAllSpellsAsync(CancellationToken cancellationToken);
    }
}
