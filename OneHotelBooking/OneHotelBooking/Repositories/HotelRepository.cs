using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneHotelBooking.Repositories
{
    public class HotelRepository : IRepository
    {
        private readonly HotelContext _context;

        public HotelRepository(HotelContext context)
        {
            _context = context;
        }

        public IQueryable<T> Get<T>() where T : class
        {
            return _context.Set<T>();
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void AddRange<T>(IEnumerable<T> entities) where T : class
        {
            _context.AddRange(entities);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void RemoveRange<T>(IEnumerable<T> entities) where T : class
        {
            _context.RemoveRange(entities);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
