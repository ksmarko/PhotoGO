using PagedList;
using System.Collections.Generic;

namespace PhotoGO.WEB.Helpers
{
    public class CreatePagedList
    {
        const int pageSize = 12;

        public static IEnumerable<T> From<T> (ICollection<T> list, int? page) where T : class
        {            
            int pageNumber = (page ?? 1);

            return list.ToPagedList(pageNumber, pageSize);
        }
    }
}