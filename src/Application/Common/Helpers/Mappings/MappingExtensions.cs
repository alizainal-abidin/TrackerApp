namespace Application.Common.Helpers.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Shared.Models;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    public static class MappingExtensions
    {
        public static Task<CollectionResponse<TDestination>> CollectionResponseAsync<TDestination>(
            this IQueryable<TDestination> queryable,
            int pageNumber,
            int pageSize)
            => CollectionResponse<TDestination>.CreateAsync(queryable, pageNumber, pageSize);

        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(
            this IQueryable queryable,
            IConfigurationProvider configuration)
            => queryable.ProjectTo<TDestination>(configuration).ToListAsync();
    }
}