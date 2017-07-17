using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyUniversity
{
    public class PaginatedList<T> : List<T>
    {
        public int pageIndex{ get; private set; }
        public int TotalPages { get; private set; }
        public PaginatedList(List<T> items, int count, int PageIndex, int pageSize)
        {
            pageIndex = PageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }
        public bool HasPreviousPage
        {
            get
            {
                return (pageIndex > 1);
            }
        }
        public bool HasNextPage
        {
            get
            {
                return (pageIndex < TotalPages);
            }
        }
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}