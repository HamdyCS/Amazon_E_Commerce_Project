using AutoMapper;
using BusinessLayer.Mapper.Contracks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mapper
{
    public class GenericMapper : IGenericMapper
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GenericMapper> _logger;

        public GenericMapper(IMapper mapper,ILogger<GenericMapper> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public TDestination MapSingle<TSourse, TDestination>(TSourse sourse) where TDestination : class where TSourse : class
        {
            try
            {
                return _mapper.Map<TDestination>(sourse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot map from {sourse} to {TDestination}", typeof(TSourse).Name, typeof(TDestination).Name);
                return null;
            }
            
        }

        public void MapSingle<TSourse, TDestination>(TSourse sourse, TDestination destination) where TDestination : class where TSourse : class
        {
            try
            {
                 _mapper.Map(sourse,destination);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Cannot map from {sourse} to {TDestination}", typeof(TSourse).Name, typeof(TDestination).Name); 
                throw new Exception($"Cannot map from {typeof(TSourse).Name} to {typeof(TDestination).Name}");
            }

        }
        public IEnumerable<TDestination> MapCollection<TSourse, TDestination>(IEnumerable<TSourse> sourse) where TDestination : class where TSourse : class
        {
            try
            {
                return _mapper.Map<IEnumerable<TDestination>>(sourse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot map from {sourse} to {TDestination}", typeof(TSourse).Name, typeof(TDestination).Name);
                return null;
            }
        }
    }
}
