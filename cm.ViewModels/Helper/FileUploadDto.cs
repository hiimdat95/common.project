namespace cm.ViewModels.Helper
{
    public class FileUploadDto
    {
        public string OriginName { get; set; }
        public string FilePath { get; set; }
        public string FilePathFull { get; set; }
        public long? FileLength { get; set; }
        public string FileExtend { get; set; }
    }
}