namespace Application.Common
{
    using Application.Common.Interfaces;
    using AutoMapper;
    using Microsoft.AspNetCore.Http;

    public abstract class BaseHandler
    {
        public BaseHandler(ITrackerDbContext context) => this.Context = context;

        public BaseHandler(ITrackerDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.Mapper = mapper;
        }

        public BaseHandler(ITrackerDbContext context, ICurrentUserService currentUserService)
        {
            this.Context = context;
            this.CurrentUserService = currentUserService;
        }

        public BaseHandler(
            ITrackerDbContext context,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            this.Context = context;
            this.CurrentUserService = currentUserService;
            this.Mapper = mapper;
        }

        public BaseHandler(
            ITrackerDbContext context,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IHttpContextAccessor httpContext)
        {
            this.Context = context;
            this.CurrentUserService = currentUserService;
            this.Mapper = mapper;
            this.HttpContext = httpContext;
        }

        protected ITrackerDbContext Context { get; }

        protected ICurrentUserService CurrentUserService { get; }

        protected IMapper Mapper { get; }

        protected IHttpContextAccessor HttpContext { get; }
    }
}