using DsCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsCommon.IUnitOfWorkPatern
{
    public interface IUnitOfWork : IDisposable
    {
        IUserService Usuarios { get; }
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<int> Complete();
    }
}
