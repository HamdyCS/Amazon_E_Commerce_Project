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
            try
            {
                var result = await _unitOfWork.CompleteAsync();
                return result > 0;

            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public async Task<PersonDto> AddAsync(PersonDto personDto)
        {
            if (personDto == null) throw new ArgumentNullException(nameof(personDto));

            try
            {
                var person = _genericMapper.MapSingle<PersonDto, Person>(personDto);

                if (person == null)
                {
                    return null;
                }

                await _unitOfWork.personRepository.AddAsync(person);

                var IsComoleted = await _completeAsync();

                if (!IsComoleted)
                    return null;


                var result = _genericMapper.MapSingle<Person, PersonDto>(person);

                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<PersonDto>> AddRangeAsync(IEnumerable<PersonDto> peopleDtos)
        {
            try
            {
                var People = _genericMapper.MapCollection<PersonDto, Person>(peopleDtos);

                if (People == null)
                    return null;

                await _unitOfWork.personRepository.AddRangeAsync(People);

                var result =  _genericMapper.MapCollection<Person, PersonDto>(People);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<PersonDto> FindByIdAsync(long id)
        {
            if (id < 1) throw new ArgumentException("Id cannot be smaller than 1");
            try
            {
                var person = await _unitOfWork.personRepository.GetByIdAsTrackingAsync(id);

                if (person == null)
                    return null;

                var personDto = _genericMapper.MapSingle<Person, PersonDto>(person);

                return personDto;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<IEnumerable<PersonDto>> GetAllAsync()
        {
            try
            {

                var People = await _unitOfWork.personRepository.GetAllAsNoTrackingAsync();

                if (People == null)
                    return null;

                var PeopleDtos = _genericMapper.MapCollection<Person, PersonDto>(People);

                return PeopleDtos;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<long> GetCountOfAsync()
        {
            try
            {

                var result = await _unitOfWork.personRepository.GetCountAsync();

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> UpdateByIdAsync(long Id, PersonDto dto)
        {
            if (Id < 1) throw new ArgumentException("Id cannot be smaller than 1");
            if (dto == null) throw new ArgumentNullException("PersonDto cannot be null");
            try
            {

                var person = await _unitOfWork.personRepository.GetByIdAsTrackingAsync(Id);

                if (person == null)
                    return false;

                _genericMapper.MapSingle(dto, person);

                await _unitOfWork.personRepository.UpdateAsync(Id,person);

                var IsCompleted = await _completeAsync();

                return IsCompleted;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(long Id)
        {
            if(Id < 1) throw new ArgumentException("cannot be smaller than 1",nameof(Id));
            try
            {
                var Person = await _unitOfWork.personRepository.GetByIdAsTrackingAsync(Id);

                if (Person == null)
                {
                    return false;
                }

                await _unitOfWork.personRepository.DeleteAsync(Id);

                return await _completeAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PersonDto>> GetPagedDataAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) throw new ArgumentException("Must be greater than zero", nameof(pageNumber));
            if (pageSize < 1) throw new ArgumentException("Page size must be greater than zero", nameof(pageSize));

            try
            {
                var People = await _unitOfWork.personRepository.GetPagedDataAsNoTractingAsync(pageNumber, pageSize);

                if (People == null)
                    return null;

                var PeopleDtos = _genericMapper.MapCollection<Person, PersonDto>(People);

                return PeopleDtos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteRangeByIdAsync(IEnumerable<long> Ids)
        {
            if (Ids is null || !Ids.Any()) throw new ArgumentException("cannot be null or empty", nameof(Ids));

            try
            {
                await _unitOfWork.personRepository.DeleteRangeAsync(Ids);

                var result = await _completeAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

}
