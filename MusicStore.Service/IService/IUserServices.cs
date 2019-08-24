using MusicStore.BussinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.IServices
{
    public interface IUserServices
    {
        IEnumerable<UserEntity> GetAll();
        int Authenticate(string userName, string password);
		UserEntity Add(UserEntity model);
        void SetActive(int userId, bool isActive);
        UserEntity UpdateName(int userId, string userName);
        void ChangePassword(int userId, string oldPassword, string newPassword);
        void UpdateRole(int userId, int roleId);
    }
}
