﻿using AutoMapper;
using Helper;
using MusicStore.BussinessEntity;
using MusicStore.Model.DataModels;
using MusicStore.Model.UnitOfWork;
using MusicStore.Service.IService;
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
		public IEnumerable<UserEntity> GetAll()
		{
			var tasks = _unitOfWork.UserRepository.GetAll().ToList();
			if (tasks.Any())
			{
				var config = new MapperConfiguration(cfg => cfg.CreateMap<system_User, UserEntity>());
				var mapper = config.CreateMapper();
				var results = mapper.Map<List<system_User>, List<UserEntity>>(tasks);
				return results;
			}
			return null;
		}

		/// <summary>
		/// Get user details
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public UserEntity GetById(int id)
		{
			var entity = _unitOfWork.UserRepository.GetByID(id);
			if (entity != null)
			{
				var config = new MapperConfiguration(cfg => cfg.CreateMap<system_User, UserEntity>());
				var mapper = config.CreateMapper();
				var model = mapper.Map<system_User, UserEntity>(entity);
                if (String.IsNullOrEmpty(model.Photo))
                {
                    model.Photo = Constants.DEFAULT_USER_PHOTO;
                }

                return model;
			}
			return null;
		}

		public UserEntity Add(UserEntity model)
		{
			using (var scope = new TransactionScope())
			{
				var config = new MapperConfiguration(cfg => cfg.CreateMap<UserEntity, system_User>());
				var mapper = config.CreateMapper();
				var entity = mapper.Map<UserEntity, system_User>(model);
                if (String.IsNullOrEmpty(entity.Photo))
                {
                    entity.Photo = Constants.DEFAULT_USER_PHOTO;
                }
				_unitOfWork.UserRepository.Insert(entity);
				_unitOfWork.Save();
				scope.Complete();
				model.UserId = entity.UserId;
				return model;
			}
		}

		public bool Delete(int id)
		{
			var success = false;
			if (id > 0)
			{
				using (var scope = new TransactionScope())
				{
					var user = _unitOfWork.UserRepository.GetByID(id);
					if (user != null)
					{
						_unitOfWork.UserRepository.Delete(user);
						_unitOfWork.Save();
						scope.Complete();
						success = true;
					}
				}
			}

			return success;
		}

		public int Authenticate(string userName, string password)
		{
			var user = _unitOfWork.UserRepository.Get(u => u.UserName == userName && u.Password == password);
			if (user != null && user.UserId > 0)
			{
				return user.UserId;
			}

			return 0;
		}

		public void ChangePassword(int userId, string oldPassword, string newPassword)
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

        public UserEntity UpdateProfile(UserEntity profile)
        {
            if (!IsExisted(profile.UserId)) throw new Exception("User is not existed.");
            using (var scope = new TransactionScope())
            {
                var user = _unitOfWork.UserRepository.GetByID(profile.UserId);
                user.Name = profile.Name;
                user.Email = profile.Email;
                _unitOfWork.Save();
                scope.Complete();

                var config = new MapperConfiguration(cfg => cfg.CreateMap<system_User, UserEntity>());
                var mapper = config.CreateMapper();
                var entity = mapper.Map<system_User, UserEntity>(user);
                return entity;
            }
        }

        public string UpdateUserPhoto(int userId, string photo)
        {
            if (!IsExisted(userId)) throw new Exception("User is not existed.");
            var user = _unitOfWork.UserRepository.GetByID(userId);
            user.Photo = String.IsNullOrEmpty(photo)? Constants.DEFAULT_USER_PHOTO: photo;
            _unitOfWork.Save();

            return photo;
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

		public bool IsUserExisted(string userName)
		{
			var user = _unitOfWork.UserRepository.Get(u => u.UserName == userName);
			if (user != null && user.UserId > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public int GetTotalUser()
		{
			return _unitOfWork.UserRepository.Count(u => u.IsActive);
		}

		public UserEntity RequestRegisterUser(string userName, string password, string email)
		{
			try
			{
				if (_unitOfWork.UserRepository.Count(u => u.UserName == userName) > 0)
				{
					throw new Exception("UserName is existed.");
				}

				if (_unitOfWork.UserRepository.Count(u => u.Email == email) > 0)
				{
					throw new Exception("Email is existed.");
				}

				var newUser = new system_User()
				{
					UserName = userName,
					Password = password,
					Email = email,
					Name = userName,
					RoleId = (int)UserRoleEnum.USER,
					IsActive = false,
				};
				_unitOfWork.UserRepository.Insert(newUser);
				_unitOfWork.Save();

				return new UserEntity()
				{
					UserId = newUser.UserId,
					UserName = newUser.UserName,
					Password = newUser.Password,
					Email = newUser.Email
				};
			}
			catch (Exception ex)
			{
				throw new Exception("Error when create user");
			}
		}

        

        #endregion
    }
}