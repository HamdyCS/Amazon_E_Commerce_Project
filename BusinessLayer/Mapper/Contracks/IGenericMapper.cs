using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mapper.Contracks
{
    public interface IGenericMapper
    {
        public TDestination MapSingle<TSourse, TDestination>(TSourse sourse) where TDestination : class where TSourse : class;
        public IEnumerable<TDestination> MapCollection<TSourse, TDestination>(IEnumerable<TSourse> sourse) where TDestination : class where TSourse : class;
        public void MapSingle<TSourse, TDestination>(TSourse sourse, TDestination destination) where TDestination : class where TSourse : class;
    }
}
