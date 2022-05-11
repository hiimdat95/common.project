using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Common;
using Utilities.Constants;
using ViewModels.Files;

namespace Repository.Repositories
{
    public partial class FilesRepository : IFilesRepository
    {
        protected readonly ILogger<IBaseRepository> _logger;
        protected readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly ICurrentPrincipal _currentPrincipal;

        private string domainFile = string.Empty;

        public FilesRepository(ILogger<IBaseRepository> logger,
            IConfiguration configuration,
            ICurrentPrincipal currentPrincipal,
            AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
            _configuration = configuration;
            _currentPrincipal = currentPrincipal;

            domainFile = _configuration.GetSection(SystemConstants.AppSettings
                .SectionAppSettings)[SystemConstants.AppSettings.URL_DOMAIN];
        }

        public async Task<bool> DeleteFileUpload(List<DeleteFileUploadModel> param)
        {
            try
            {
                foreach (var item in param)
                {
                    var uploadFile = await _appDbContext.Files
                                            .FirstOrDefaultAsync(s =>
                                            s.EntityId == item.EntityId &&
                                            s.FileTypeUpload.ToLower()
                                            .Equals(item.EntityType.ToLower()) &&
                                            (domainFile + s.Path.ToLower())
                                            .Equals(item.FilePath.ToLower()));

                    if (uploadFile != null)
                    {
                        uploadFile.IsDeleted = true;
                        await _appDbContext.SaveChangesAsync();
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
