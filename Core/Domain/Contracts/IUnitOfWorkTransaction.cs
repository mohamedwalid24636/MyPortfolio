namespace Domain.Contracts
{
    /// <summary>
    /// A database transaction started by <see cref="IUnitOfWork"/>. It keeps the exposed boundary
    /// free of the persistence technology, so services can group several saves into one logical
    /// operation without knowing what is underneath.
    /// </summary>
    public interface IUnitOfWorkTransaction : IAsyncDisposable
    {
        /// <summary>Makes every write inside the transaction permanent.</summary>
        Task CommitAsync();

        /// <summary>Discards every write inside the transaction.</summary>
        Task RollbackAsync();
    }
}