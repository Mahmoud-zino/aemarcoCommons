﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Toolbox.SyncTools
{
    public class AppSingleStartup : IDisposable
    {

        #region ctor

        /// <summary>
        /// uses provided name
        /// </summary>
        /// <param name="name"></param>
        public AppSingleStartup(string name)
        {
            Name = name;
        }

        /// <summary>
        /// uses Manifest Name
        /// </summary>
        public AppSingleStartup()
            : this(Assembly.GetEntryAssembly().ManifestModule.Name)
        {

        }

        #endregion

        public string Name { get; set; }


        private Mutex _mutex;
        public bool Start()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(AppSingleStartup));

            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Name of starting App needs to be specified.", nameof(Name));


            _mutex = new Mutex(true, Name, out bool _owned);
            if (!_owned) Dispose();
            return _owned;
        }

        #region IDisposable

        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                //managed resources here
                _mutex.Dispose();
            }
            //unmanaged resources here

            _disposed = true;
        }


        //needed when unmanaged resources
        ~AppSingleStartup()
        {
            Dispose(false);
        }

        #endregion

    }
}