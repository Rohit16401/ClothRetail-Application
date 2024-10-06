using AutoMapper;

namespace Domain.Mapper
{
    public  class AutoMapperGenericsHelper
    {
        private readonly IMapper _mapper;

        public AutoMapperGenericsHelper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        public IEnumerable<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> sourceList)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(sourceList);
        }

        public TDestination CreateMapAndMap<TSource, TDestination>(TSource source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
                cfg.AllowNullCollections = true;
            });
            var mapper = config.CreateMapper();
            return mapper.Map<TSource, TDestination>(source);
        }
    }
}
