using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Mapper.Contracks
{
    public interface IGenericMapper
    {
        public TDestination MapModel<TSourse, TDestination>(TSourse sourse) where TDestination : class;
        public IEnumerable<TDestination> MapModels<TSourse, TDestination>(IEnumerable<TSourse> sourse) where TDestination : class;
    }
}
