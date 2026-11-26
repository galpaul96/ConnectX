using ConnectX.Services.DND.Catalogs;
using ConnectX.Web.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ConnectX.Web.Components.Pages.DND.Catalogs.Items;

public sealed class ItemUploadService(AuthenticationStateProvider authentication, IServiceScopeFactory scopes, IItemService items)
{
    public async Task EnsureAuthorizedAsync()
    {
        var principal = (await authentication.GetAuthenticationStateAsync()).User;
        if (principal.Identity?.IsAuthenticated != true || !principal.IsInRole("DungeonMaster"))
            throw new UnauthorizedAccessException("Only signed-in DungeonMasters can create items.");
        // Recheck current membership, including role removal during an existing circuit.
        await using var scope = scopes.CreateAsyncScope();
        var users = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var user = await users.GetUserAsync(principal);
        if (user is null || !await users.IsInRoleAsync(user, "DungeonMaster"))
            throw new UnauthorizedAccessException("Your DungeonMaster role is no longer available. Please sign in again.");
    }

    public async Task<Guid> AddAsync(Stream stream, string filename, long size, CancellationToken cancellationToken = default)
    {
        await EnsureAuthorizedAsync();
        var input = await ItemUploadReader.ReadAsync(stream, filename, size, cancellationToken);
        await EnsureAuthorizedAsync();
        return await items.AddAsync(input, cancellationToken);
    }
}
