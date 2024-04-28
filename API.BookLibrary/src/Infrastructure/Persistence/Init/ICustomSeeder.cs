﻿namespace Infrastructure.Persistence.Init
{
    public interface ICustomSeeder
    {
        Task InitializeAsync(CancellationToken cancellationToken);
    }
}
