using Data.Dtos;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.AppContext
{
    public class MultiVerse_DB_Context_10 : MultiVerse_DB_Context<MultiVerse_DB_Context_10>, IMultiVerse_DB_Context_10
    {
        public MultiVerse_DB_Context_10(DbContextOptions<MultiVerse_DB_Context_10> options) : base(options)
        {
        }

        public IMultiVerse_DB_Context_10 DBContext_Instance
        {
            get
            {
                return this;
            }
        }

        public int Save()
        {
            return this.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await this.SaveChangesAsync();
        }
    }
    public abstract partial class MultiVerse_DB_Context<T> : DbContext where T : DbContext
    {
        public MultiVerse_DB_Context(DbContextOptions<T> options)
        : base(options)
        {
        }

        #region DBSet
        [NotMapped]
        public virtual DbSet<P_Get_User_Info_SP> P_Get_User_Info { get; set; } = null!;
        #endregion

        public int Save()
        {
            return this.SaveChanges();
        }
        public async Task<int> SaveAsync()
        {
            return await this.SaveChangesAsync();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region modelBuilder

            #endregion

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
