using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using DynamicExpressions;
using Infrastructure.Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Utilities.Auths;
using Utilities.Common;
using Utilities.Constants;
using Utilities.Contracts;
using Utilities.Models.Settings;
using Utilities.Services;
using ViewModels.Auth;
using ViewModels.Users;

namespace Application.Services
{
    public class DynamicService : Service, IDynamicService
    {
        private readonly IJwtHandler _jwtHandler;
        private readonly IBaseRepository _repository;
        private readonly IDapperProvider _dapperProvider;
        private readonly IAuthValidator _authValidator;
        private readonly AuthConfig _authConfig;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public DynamicService(IJwtHandler jwtHandler
            , IBaseRepository repository
            , IAuthValidator authValidator
            , IOptions<AuthConfig> authConfig
            , UserManager<AppUser> userManager
            , IMapper mapper
            , IUnitOfWork unitOfWork
            , IConfiguration config
            , IEmailService emailService
            , IDapperProvider dapperProvider
            , ICurrentPrincipal currentPrincipal
            , IHttpContextAccessor httpContextAccessor) : base(currentPrincipal, httpContextAccessor)
        {
            _jwtHandler = jwtHandler;
            _repository = repository;
            _authValidator = authValidator;
            _authConfig = authConfig.Value;
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _config = config;
            _emailService = emailService;
            _dapperProvider = dapperProvider;
        }

        public async Task<ServiceResponse> GetAllAsync<T>(object objectInstance, PaginatedInputModel pagingParams)
        {
            if (pagingParams == null)
            {
                return Ok(new PagedResult<T>());
            }
            var listItem = _repository.AsQueryable<T>(null);

            #region [Filter]

            if (pagingParams.FilterParam.Any())
            {
                DynamicFilterBuilder<T> predicate = new DynamicFilterBuilder<T>();
                predicate = FilterUtility.Filter<T>.FilteredData(pagingParams.FilterParam, predicate);
                var expression = predicate.Build();
                listItem = listItem.Where(expression);
            }

            #endregion [Filter]

            #region [Sorting]

            if (pagingParams.SortingParams.Any())
            {
                listItem = SortingUtility.Sorting<T>.SortData(listItem, pagingParams.SortingParams);
            }

            #endregion [Sorting]

            #region [Grouping]

            if (pagingParams.GroupingColumns != null && pagingParams.GroupingColumns.Any())
            {
                listItem = SortingUtility.Sorting<T>.GroupingData(listItem, pagingParams.GroupingColumns) ?? listItem;
            }

            #endregion [Grouping]

            #region [Paging]

            var totalRow = await listItem.CountAsync();
            var pageData = await listItem.Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize).Take(pagingParams.PageSize).ToListAsync();

            return Ok(new PagedResult<T>
            {
                TotalCount = totalRow,
                PageSize = pagingParams.PageSize,
                PageIndex = pagingParams.PageNumber,
                Items = pageData
            });

            #endregion [Paging]
        }
    }
}