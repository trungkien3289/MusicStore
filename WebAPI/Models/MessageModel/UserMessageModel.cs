using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models.MessageModel
{
    public class DeveloperItemMessageModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class UpdateUserProfileRequest
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class UpdateUserPhotoRequest
    {
        public string Photo { get; set; }
    }

    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }

}