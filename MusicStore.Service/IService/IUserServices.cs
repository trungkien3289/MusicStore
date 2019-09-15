using MusicStore.BussinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Service.IService
{
    public interface IUserServices
    {
        IEnumerable<UserEntity> GetAll();
		UserEntity GetById(int id);
        int Authenticate(string userName, string password);
		UserEntity Add(UserEntity model);
		bool Delete(int id);
		void SetActive(int userId, bool isActive);
        UserEntity UpdateName(int userId, string userName);
        void ChangePassword(int userId, string oldPassword, string newPassword);
        void UpdateRole(int userId, int roleId);
        bool IsUserExisted(string userName);
        int GetTotalUser();
        UserEntity RequestRegisterUser(string userName, string password, string email);
        UserEntity UpdateProfile(UserEntity profile);
        string UpdateUserPhoto(int userId, string photo);
    }
}
