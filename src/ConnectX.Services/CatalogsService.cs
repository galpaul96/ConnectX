using AutoMapper;
using ConnectX.Domain.Entities.Catalogs.Spells;
using ConnectX.Domain.Services.Catalogs.Spells;
using ConnectX.Ef;
using Microsoft.EntityFrameworkCore;

namespace ConnectX.Services;

public sealed class CatalogsService : ICatalogsService
{
    private readonly IRepository repository;
    private readonly IMapper mapper;
    public CatalogsService(IRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<SpellDto>> GetAllSpellsAsync(CancellationToken cancellationToken)
    {
        var spells = repository.GetAllAsync<Spell>();

        return await mapper.ProjectTo<SpellDto>(spells).ToListAsync();
    }
}
