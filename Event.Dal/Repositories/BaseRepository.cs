using Event.Dal.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Event.Dal.Repositories;

public abstract class BaseRepository<TEntity> where TEntity : class, IEntity, new()
{
    protected readonly DbSet<TEntity> Table;

    protected BaseRepository(DbContext context)
    {
        Table = context.Set<TEntity>();
    }

    public async Task CreateAsync(TEntity entity) => await Table.AddAsync(entity);

    public void Update(TEntity entity) => Table.Update(entity);

    public async Task DeleteAsync(int id)
    {
        var entity = await Table.FindAsync(id);

        if (entity is null)
        {
            return;
        }

        Table.Remove(entity);
    }
}