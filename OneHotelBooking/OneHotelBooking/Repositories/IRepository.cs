using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneHotelBooking.Repositories
{
    /// <summary>
    /// Interface for Repository layer between application and data access layer.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <returns>IQueryable entity</returns>
        IQueryable<T> Get<T>() where T : class;

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        void Add<T>(T entity) where T : class;

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="entities">The entities.</param>
        void AddRange<T>(IEnumerable<T> entities) where T : class;

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        void Update<T>(T entity) where T : class;

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        void Remove<T>(T entity) where T : class;

        /// <summary>
        /// Removes the range.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="entities">The entities.</param>
        void RemoveRange<T>(IEnumerable<T> entities) where T : class;

        /// <summary>
        /// Saves the changes.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task SaveChangesAsync();
    }
}
