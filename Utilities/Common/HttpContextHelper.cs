using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Security.Claims;

namespace Utilities.Common
{
    public static class HttpContextHelper
    {
        public static Guid GetCurrentUserId(this HttpContext httpContext)
        {
            var currentUserId = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid.TryParse(currentUserId, out Guid userId);
            return userId;
        }

        public static RplyThongTinUserDto UserInfo(this HttpContext httpContext)
        {
            var userInfo = JsonConvert.DeserializeObject
                <RplyThongTinUserDto>(httpContext.User.FindFirst("CanBoViewModel").Value);

            return userInfo;
        }

        public class RplyThongTinUserDto
        {
            public Guid Id { get; set; }
            public string HoTen { get; set; }
            public string AnhDaiDien_FilePath { get; set; }
            public string ChucVu { get; set; }
            public string DonVi { get; set; }
            public string Email { get; set; }
            public string UserName { get; set; }
            public DonViTrucThuoc DonViTrucThuoc { get; set; }
        }

        public class DonViTrucThuoc
        {
            public Guid Id { get; set; }
            public string TenDonVi { get; set; }
        }
    }
}