using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Developers.Stream.Infrastructure.Contracts;


public interface IRepository<T> : IRepositoryBase<T>
    where T : class
{
}

public class Repository<T>(DbContext context) : RepositoryBase<T>(context), IRepository<T>
    where T : class
{
    protected readonly DbContext Context = context;
}