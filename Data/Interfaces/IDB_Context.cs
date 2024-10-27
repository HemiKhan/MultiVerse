using Microsoft.EntityFrameworkCore;

namespace Data.Interfaces
{
    public interface IMultiVerse_DB_Context_10 : IMultiVerse_DB_Context
    {
        public IMultiVerse_DB_Context_10 DBContext_Instance { get; }
    }
    public interface IMultiVerse_DB_Context
    {
        DbSet<T> Set<T>() where T : class;
        int Save();

        Task<int> SaveAsync();
    }
}
