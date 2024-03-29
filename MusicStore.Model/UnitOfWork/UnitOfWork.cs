﻿using MusicStore.Model.DataContext;
using MusicStore.Model.DataModels;
using MusicStore.Model.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Model.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        #region Private member variables...

        private DbContext _context = null;
        private GenericRepository<ms_Album> _albumRepository;
        private GenericRepository<ms_Artist> _artistRepository;
        private GenericRepository<ms_Collection> _collectionRepository;
        private GenericRepository<ms_Genre> _genreRepository;
        private GenericRepository<ms_Song> _songRepository;
        private GenericRepository<system_User> _userRepository;
        private GenericRepository<system_Token> _tokenRepository;
        private GenericRepository<ms_Application> _applicationRepository;

        #endregion

        public UnitOfWork()
        {
            _context = new MainDbContext();
        }

        #region Public Repository Creation properties...

        /// <summary>
        /// Get/Set Property for album repository.
        /// </summary>
        public GenericRepository<ms_Album> AlbumRepository
        {
            get
            {
                if (this._albumRepository == null)
                {
                    this._albumRepository = new GenericRepository<ms_Album>(_context);
                }

                return this._albumRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for artist repository.
        /// </summary>
        public GenericRepository<ms_Artist> ArtistRepository
        {
            get
            {
                if (this._artistRepository == null)
                {
                    this._artistRepository = new GenericRepository<ms_Artist>(_context);
                }

                return this._artistRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for collection repository.
        /// </summary>
        public GenericRepository<ms_Collection> CollectionRepository
        {
            get
            {
                if (this._collectionRepository == null)
                {
                    this._collectionRepository = new GenericRepository<ms_Collection>(_context);
                }

                return this._collectionRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for genre repository.
        /// </summary>
        public GenericRepository<ms_Genre> GenreRepository
        {
            get
            {
                if (this._genreRepository == null)
                {
                    this._genreRepository = new GenericRepository<ms_Genre>(_context);
                }

                return this._genreRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for song repository.
        /// </summary>
        public GenericRepository<ms_Song> SongRepository
        {
            get
            {
                if (this._songRepository == null)
                {
                    this._songRepository = new GenericRepository<ms_Song>(_context);
                }

                return this._songRepository;
            }
        }

        public GenericRepository<system_User> UserRepository
        {
            get
            {
                if(this._userRepository == null)
                {
                    this._userRepository = new GenericRepository<system_User>(_context);
                }

                return _userRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for token repository.
        /// </summary>
        public GenericRepository<system_Token> TokenRepository
        {
            get
            {
                if (this._tokenRepository == null)
                {
                    this._tokenRepository = new GenericRepository<system_Token>(_context);
                }

                return _tokenRepository;
            }
        }

        /// <summary>
        /// Get/Set Property for application repository.
        /// </summary>
        public GenericRepository<ms_Application> ApplicationRepository
        {
            get
            {
                if (this._applicationRepository == null)
                {
                    this._applicationRepository = new GenericRepository<ms_Application>(_context);
                }

                return _applicationRepository;
            }
        }
        #endregion

        #region Public member methods...

        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now,
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }

        public ms_Album GetAlbumOfSong(int songId)
        {
            try
            {
                var song = _context.Set<ms_Song>().Where(s => s.Id == songId).First();
                var album = song.Albums.FirstOrDefault();
                return album;
            }catch(Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
