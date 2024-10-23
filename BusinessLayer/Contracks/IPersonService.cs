using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IPersonService
    {
        public Task<PersonDto> FindByIdAsync(long id);

        public Task<IEnumerable<PersonDto>> GetAllPeopleAsync();

        public Task<long> GetCountOfPeopleAsync();

        public Task<bool> AddAsync(PersonDto personDto);

        public Task<bool> AddRangeAsync(IEnumerable<PersonDto> peopleDtos);

        public Task<bool> UpdateAsync(int Id,PersonDto personDto);

        public Task<bool> UpdateRangeAsync(List<int> Ids, List<PersonDto> peopleDtos);

        public Task<bool> DeleteByIdAsync(int Id);

        public Task<bool> DeleteRangeByIdAsync(IEnumerable<int> Ids);

        public Task<IEnumerable<PersonDto>> GetAllPagedAsync(int pageNumber, int pageSize);

    }
}
