using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper _genericMapper;
        private readonly ILogger<PersonService> _logger;

        public PersonService(IUnitOfWork unitOfWork, IGenericMapper genericMapper, ILogger<PersonService> logger)
        {
            _unitOfWork = unitOfWork;
            _genericMapper = genericMapper;
            _logger = logger;
        }

        private async Task<bool> _completeAsync()
        {
            var result = await _unitOfWork.CompleteAsync();

            return result > 0;
        }


        public async Task<bool> AddAsync(PersonDto personDto)
        {
            var person = _genericMapper.MapModel<PersonDto, Person>(personDto);

            if (person == null)
            {
                return false;
            }

            await _unitOfWork.personRepository.AddAsync(person);

            return await _completeAsync();

        }

        public async Task<bool> AddRangeAsync(IEnumerable<PersonDto> peopleDtos)
        {
            var People = _genericMapper.MapModels<PersonDto, Person>(peopleDtos);

            if (People == null)
                return false;

            await _unitOfWork.personRepository.AddRangeAsync(People);

            return await _completeAsync();
        }

        public async Task<bool> DeleteByIdAsync(int Id)
        {
            var Person = await _unitOfWork.personRepository.GetByIdAsTrackingAsync(Id);

            if (Person == null)
            {
                return false;
            }

            _unitOfWork.personRepository.Delete(Person);

            return await _completeAsync();

        }

        public async Task<bool> DeleteRangeByIdAsync(IEnumerable<int> Ids)
        {
            var People = new List<Person>();


            foreach (var id in Ids)
            {
                var person = await _unitOfWork.personRepository.GetByIdAsTrackingAsync(id);

                if (person != null)
                    People.Add(person);
            }

            if (People.Count < 1)
            {
                return false;
            }

            _unitOfWork.personRepository.DeleteRange(People);

            return await _completeAsync();
        }

        public async Task<PersonDto> FindByIdAsync(long id)
        {
            var person = await _unitOfWork.personRepository.GetByIdAsTrackingAsync(id);

            if (person == null)
                return null;

            var personDto = _genericMapper.MapModel<Person, PersonDto>(person);

            return personDto;
        }

        public async Task<IEnumerable<PersonDto>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            var People = await _unitOfWork.personRepository.GetAllPagedAsNoTractingAsync(pageNumber, pageSize);

            if (People == null)
                return null;

            var PeopleDtos = _genericMapper.MapModels<Person, PersonDto>(People);

            return PeopleDtos;
        }

        public async Task<IEnumerable<PersonDto>> GetAllPeopleAsync()
        {
            var People = await _unitOfWork.personRepository.GetAllNoTrackingAsync();

            if (People == null)
                return null;

            var PeopleDtos = _genericMapper.MapModels<Person, PersonDto>(People);

            return PeopleDtos;
        }

        public async Task<long> GetCountOfPeopleAsync()
        {
            var result = await _unitOfWork.personRepository.GetCountAsync();

            return result;
        }

        public async Task<bool> UpdateAsync(int Id,PersonDto personDto)
        {
            if (personDto == null|| Id<1)
                return false;

            var person = await _unitOfWork.personRepository.GetByIdAsTrackingAsync(Id);

            if (person == null)
                return false;

            person = _genericMapper.MapModel<PersonDto, Person>(personDto); 

            return await _completeAsync();
        }

        public async Task<bool> UpdateRangeAsync(List<int> Ids, List<PersonDto> peopleDtos)
        {
            if (!Ids.Any() || !peopleDtos.Any())
                return false;

            var PeopleDtosCount = peopleDtos.Count();

            if (Ids.Count()!= PeopleDtosCount)
                return false;

            for(int i = 0;i< PeopleDtosCount; i++)
            {
                var person = await _unitOfWork.personRepository.GetByIdAsTrackingAsync(Ids[i]);

                if (person == null)
                    return false;

                person = _genericMapper.MapModel<PersonDto, Person>(peopleDtos[i]);

                _unitOfWork.personRepository.Update(person);
            }

            return await _completeAsync();
        }
    }

}
