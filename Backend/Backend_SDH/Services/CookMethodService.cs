﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Backend_SDH.Data;
using Backend_SDH.Dtos;
using Backend_SDH.Models;
using Backend_SDH.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_SDH.Services
{
    public class CookMethodService : ICookMethodService
    {
        private readonly IMapper _mapper;

        private readonly DataContext _dataContext;

        public CookMethodService(IMapper mapper, DataContext dataContext)
        {
            _mapper = mapper;
            _dataContext = dataContext;
        }

        public async Task<ServiceResponse<CookMethodDto>> AddCookMethod([FromBody]CookMethodDto newCookMethod)
        {
            var serviceResponse = new ServiceResponse<CookMethodDto>();

            try
            {
                var addCookMethod = _mapper.Map<CookMethod>(newCookMethod);
                _dataContext.CookMethods.Add(addCookMethod);

                await _dataContext.SaveChangesAsync();

                serviceResponse.Data = await _dataContext.CookMethods
                    .ProjectTo<CookMethodDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(c => c.Id == addCookMethod.Id);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<CookMethodDto>>> GetCookMethods()
        {
            var serviceResponse = new ServiceResponse<List<CookMethodDto>>();

            try
            {
                serviceResponse.Data = await _dataContext.CookMethods.ProjectTo<CookMethodDto>(_mapper.ConfigurationProvider).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public ServiceResponse<bool> NameExists(string name, int id = 0)
        {
            var serviceResponse = new ServiceResponse<bool>();

            try
            {
                serviceResponse.Data = _dataContext.CookMethods.Any(c => c.Name == name && (id == 0 || c.Id != id));
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
