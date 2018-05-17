using PagedList;
using System.Collections.Generic;

namespace PhotoGO.WEB.Helpers
{
    /// <summary>
    /// Creates Paged list for input list
    /// </summary>
    public class CreatePagedList
    {
        /// <summary>
        /// The count of items on the page
        /// </summary>
        const int PageSize = 12;

        /// <summary>
        /// Creates paged list from input list
        /// </summary>
        /// <typeparam name="T">Type of items</typeparam>
        /// <param name="list">Input list</param>
        /// <param name="page">Current page</param>
        /// <returns></returns>
        public static IEnumerable<T> From<T> (ICollection<T> list, int? page) where T : class
        {            
            int pageNumber = (page ?? 1);

            return list.ToPagedList(pageNumber, PageSize);
        }

        /// <summary>
        /// Creates empty paged list
        /// </summary>
        /// <typeparam name="T">Type of items</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Empty<T>() where T : class
        {
            List<T> list = new List<T>();

            return list.ToPagedList(1, PageSize);
        }
    }
}