using BlogEnginer.API.Entites;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace BlogEnginer.API.Data.Repository
{
    public interface IRepository
    {
        Task<Post> Get(int id);
        Task<IQueryable<Post>> Get();
        Task Add(Post post);
        Task Remove(int id);
        Task Update(Post post);

        Task<bool> SaveChangesAsync();
    }
    public class Repository : IRepository
    {
        private SQLiteDbContext _ctx;

        public Repository(SQLiteDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task Add(Post post)
        {
            await _ctx.Posts.AddAsync(post);
        }

        public async Task<IQueryable<Post>> Get()
        {
            return await Task.Run(() =>
            {
                return _ctx.Posts.AsQueryable<Post>();
            });

        }

        public async Task<Post> Get(int id)
        {
            return await _ctx.Posts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task Remove(int id)
        {
            var p = await Get(id);
            _ctx.Posts.Remove(p);
        }
        public async Task Update(Post post)
        {
            _ctx.Posts.Update(post);
        }
        public async Task<bool> SaveChangesAsync()
        {
            if (await _ctx.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
