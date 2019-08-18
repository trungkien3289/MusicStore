using AutoMapper;
using MusicStore.BussinessEntity;
using MusicStore.Model.DataModels;
using MusicStore.Model.UnitOfWork;
using MusicStore.Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MusicStore.Service.Services
{
    public class UserServices : IUserServices
    {
        private readonly UnitOfWork _unitOfWork;

        public UserServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region public functions

        public UserEntity Add(UserEntity model)
        {
            using (var scope = new TransactionScope())
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<UserEntity, system_User>());
                var mapper = config.CreateMapper();
                var entity = mapper.Map<UserEntity, system_User>(model);
                _unitOfWork.UserRepository.Insert(entity);
                _unitOfWork.Save();
                scope.Complete();
                model.UserId = entity.UserId;
                return model;
            }
        }

        public int Authenticate(string userName, string password)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.UserName == userName && u.Password == password);
            if(user != null && user.UserId > 0)
            {
                return user.UserId;
            }

            return 0;
        }

        public void ChangePassword(int userId,string oldPassword, string newPassword)
        {
            if (!VerifyPassword(userId, oldPassword)) throw new Exception("Password is not correct");
            using (var scope = new TransactionScope())
            {
                var user = _unitOfWork.UserRepository.GetByID(userId);
                user.Password = newPassword;
                _unitOfWork.Save();
                scope.Complete();
            }
        }

        public void SetActive(int userId, bool isActive)
        {
            if (!IsExisted(userId)) throw new Exception("User is not existed.");
            using (var scope = new TransactionScope())
            {
                var user = _unitOfWork.UserRepository.GetByID(userId);
                user.IsActive = isActive;
                _unitOfWork.Save();
                scope.Complete();
            }
        }

        public UserEntity UpdateName(int userId, string userName)
        {
            if (!IsExisted(userId)) throw new Exception("User is not existed.");
            using (var scope = new TransactionScope())
            {
                var user = _unitOfWork.UserRepository.GetByID(userId);
                user.Name = userName;
                _unitOfWork.Save();
                scope.Complete();

                var config = new MapperConfiguration(cfg => cfg.CreateMap<system_User, UserEntity>());
                var mapper = config.CreateMapper();
                var entity = mapper.Map<system_User, UserEntity>(user);
                return entity;
            }
        }

        public void UpdateRole(int userId, int roleId)
        {
            if (!IsExisted(userId)) throw new Exception("User is not existed.");
            using (var scope = new TransactionScope())
            {
                var user = _unitOfWork.UserRepository.GetByID(userId);
                user.RoleId = roleId;
                _unitOfWork.Save();
                scope.Complete();
            }
        }

        #endregion

        #region private functions
        private bool VerifyPassword(int userId, string password)
        {
            var user = _unitOfWork.UserRepository.GetByID(userId);
            return user.Password == password;
        }

        private bool IsExisted(int userId)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.UserId == userId);
            return user != null ? true : false;
        }

        #endregion
    }
}
