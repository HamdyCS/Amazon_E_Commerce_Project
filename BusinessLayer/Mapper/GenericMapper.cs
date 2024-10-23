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

        public TDestination MapModel<TSourse, TDestination>(TSourse sourse) where TDestination : class
        {
            try
            {
                return _mapper.Map<TDestination>(sourse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Cannot map from {nameof(TSourse)} to {nameof(TDestination)} ");
                return null;
            }
            
        }

        public IEnumerable<TDestination> MapModels<TSourse, TDestination>(IEnumerable<TSourse> sourse) where TDestination : class
        {
            try
            {
                return _mapper.Map<IEnumerable<TDestination>>(sourse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Cannot map from {nameof(TSourse)} to {nameof(TDestination)} ");
                return null;
            }
        }
    }
}
