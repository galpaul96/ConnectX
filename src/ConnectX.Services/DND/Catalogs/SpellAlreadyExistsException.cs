namespace ConnectX.Services.DND.Catalogs;

public sealed class SpellAlreadyExistsException() : InvalidOperationException("A spell with this source ID or slug already exists.");
