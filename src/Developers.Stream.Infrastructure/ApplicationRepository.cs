using Developers.Stream.Infrastructure.Contexts;
using Developers.Stream.Infrastructure.Contracts;

namespace Developers.Stream.Infrastructure;

public class ApplicationRepository<TEntity>(ApplicationDbContext context) : Repository<TEntity>(context)
    where TEntity : class;