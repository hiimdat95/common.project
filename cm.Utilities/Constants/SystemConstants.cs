namespace cm.Utilities.Constants
{
    public static class SystemConstants
    {
        public const string MainConnectionString = "Default";
        public const string KeyConnect_DNCrawl = "DNCrawlConnection";
        public const string KeyConnect_Ioc = "IocConnection";

        public static string ConnectionString_DNCrawl { get; set; }
        public static string ConnectionString_Ioc { get; set; }

        public const int MaxAnsiCode = 255;

        public static class Claims
        {
            public const string Permissions = "permissions";
        }

        public static class AppSettings
        {
            public const string DefaultLanguageId = "DefaultLanguageId";
            public const string Token = "Token";
            public const string BaseAddress = "BaseAddress";

            public static string URL_YKienNguoiDan_ThongKe { get; set; }
            public static string URL_YKienNguoiDan_ThongKeXuLyThaiDo { get; set; }
            public static string URL_YKienNguoiDan_ChiTietXuLyThaiDo { get; set; }
            public static string URL_YKienNguoiDan_ChiTietDanhSachPhanAnh { get; set; }

            public static string Value_YKienNguoiDan_From { get; set; }
            public static string Value_YKienNguoiDan_To { get; set; }
            public static string Value_YKienNguoiDan_ApiKey { get; set; }

            public static string URL_SendText_TextToSpeech { get; set; }
            public static string URL_GetResult_TextToSpeech { get; set; }
            public static string URL_GetCoronaStatistical_Info { get; set; }

            public static string APIKEY_PARAM_TTS { get; set; }
            public static string APIKEY_VALUE_TTS { get; set; }

            public static string UPLOAD_FOLDER { get; set; } = "AppSettings:UPLOAD_FOLDER";

            public static string UPLOAD_FOLDER_FAQS { get; set; }
        }

        public static class Roles
        {
            public const string Admin = "Admin";
        }

        public static class FileFolders
        {
            public const string Products = "products";
            public const string Attachments = "attachments";
        }

        public static class TrangThaiVanBan
        {
            public const string DaXyLy = "Đã xử lý";
            public const string ChoXuLy = "Chờ xử lý";
            public const string DangXuLy = "Đang xử lý";
        }

        public static class TypeFunctionMapping
        {
            public const string Users = "Users";
            public const string Roles = "Roles";
            public const string Group = "Group";
        }

        public static class LoaiLichThongKe
        {
            public const string TinhTrangHoSo = "TinhTrangHoSo";
            public const string TinhTrangHoSo5NamGanNhat = "HoSo5NamGanNhat";
            public const string SoLuongHoSo5NamGanNhat = "SoLuongHoSo5NamGanNhat";
            public const string TinhHinhHoSoTheoSoNganh = "HoSoTheoCoSoNganh";
            public const string TinhHinhHoSoTheoDonVi = "HoSoTheoDonVi";
        }

        public static class LoaiYkndThongKe
        {
            public const string TinhHinhXuLyTrangChu = "TinhHinhXuLyTrangChu";
            public const string TinhHinhXuLyPakn = "TinhHinhXuLyPakn";
            public const string TinhHinhXuLyTheoDoiTuong = "TinhHinhXuLyTheoDoiTuong";
            public const string TinhHinhXuLyTheoNoiDung = "TinhHinhXuLyTheoNoiDung";
        }

        public static class LoaiNhiemVuThongKe
        {
            public const string ChuaXuLy = "ChuaXuLy";
            public const string DangXuLy = "DangXuLy";
            public const string DaXuLy = "DaXuLy";
        }

        public static class LoaiNhiemVu
        {
            public const string DaXong = "Đã xong";
            public const string ChuaXuLy = "Chưa xử lý";
            public const string DangXuLy = "Đang xử lý";
        }

        public static class TrangThaiXuLyNhiemVu
        {
            public const string DungHan = "0";
        }

        public static class CapCoQuan
        {
            public const string SoBanNganh = "Sở - Ban - Ngành";
            public const string HuyenXaTp = "Huyện - Thị xã - Thành phố";
            public const string Khac = "Khác";
        }
    }
}