using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IAttachmentService
    {
        //filestream  => send to data base to save photo and call it again to represent the photo in application
        //
        Task<string?> UploadAsync(Stream FileStream ,string FileName,string FolderName ,CancellationToken ct =default);

        bool Delete(string FileName, string FolderName);
        (Stream stream, string ContantType)? GetFile(string FileName, string FolderName);
    }
}
