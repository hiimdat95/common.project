using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Files;

namespace Repository.Interfaces
{
    public partial interface IFilesRepository
    {
        Task<bool> DeleteFileUpload(List<DeleteFileUploadModel> param);
    }
}
