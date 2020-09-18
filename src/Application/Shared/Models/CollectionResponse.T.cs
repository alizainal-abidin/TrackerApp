namespace Application.Shared.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public class CollectionResponse<T>
    {
        public CollectionResponse(IReadOnlyCollection<T> items) => this.Items = items;

        public CollectionResponse(IReadOnlyCollection<T> items, int count, int pageIndex, int pageSize)
        {
            this.Items = items;
            this.PageIndex = pageIndex;
            this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.TotalCount = count;
        }

        public IReadOnlyCollection<T> Items { get; }

        public int PageIndex { get; }

        public int TotalPages { get; }

        public int TotalCount { get; }

        public static async Task<CollectionResponse<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new CollectionResponse<T>(items, count, pageIndex, pageSize);
        }
    }
}