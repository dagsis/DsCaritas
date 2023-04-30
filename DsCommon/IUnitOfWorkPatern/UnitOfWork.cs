using Caritas.Insfrastructure.Data;
using DsCommon.Services;
using System.Collections;

namespace DsCommon.IUnitOfWorkPatern
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable? _repositories;
        private readonly ApplicationDbContext _context;
      //  private readonly IHttpClientFactory _clienteFactory;

        public IUserService Usuarios { get; private set; }

        public UnitOfWork(ApplicationDbContext context, IHttpClientFactory clienteFactory)
        {
            _context = context;
         //   _clienteFactory = clienteFactory;

            Usuarios = new UserService(clienteFactory);
        }

        public async Task<int> Complete()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error en transacion", e);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories is null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<TEntity>)_repositories[type]!;
        }
    }
}
