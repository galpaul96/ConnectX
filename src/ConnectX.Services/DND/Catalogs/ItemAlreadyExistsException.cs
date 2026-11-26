namespace ConnectX.Services.DND.Catalogs;

public sealed class ItemAlreadyExistsException() : InvalidOperationException("An item with this source ID or slug already exists.");
