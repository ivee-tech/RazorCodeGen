using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using System.Linq.Expressions;

using Iv.Common;

namespace Iv.Data
{

    /// <summary>
    /// Simple repository that uses Xml files as storage.
    /// </summary>
    /// <remarks>The T type must be serializable.</remarks>
    public class XmlFileRepository<T, TKey> : IDataQuery<T, TKey>, IDataCommand<T, TKey>
        where T : ObjectDefBase<TKey>
        where TKey : IComparable

    {

        private List<T> _list;
        private string _fileName;
        private T _item;

        public XmlFileRepository()
        {
            _item = null;
            _list = new List<T>();
        }

        public string StoreName { get => _fileName; }

        public T Create(T t)
        {
            LoadList();
            _item = t;
            _list.Add(_item);
            SaveList();
            return _item;
        }

        public T Create(T t, IDataCommandSpecification<T> spec)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            LoadList();
            return _list;
        }

        public IEnumerable<T> Filter(Expression<Func<T, bool>> predicate)
        {
            LoadList();
            var q = _list.AsQueryable().Where(predicate);
            return q.ToList();
        }
        public T Find(IDataQuerySpecification<T> spec)
        {
            throw new NotImplementedException();
        }

        public T Update(T t)
        {
            LoadList();
            _item = _list.Where(x => x.Key.Equals(t.Key)).FirstOrDefault();
            if(_item != null)
            {
                _list.Remove(_item);
                _list.Add(t);
                SaveList();
                return t;
            }
            return null;
        }
        public T Update(T entity, IDataCommandSpecification<T> spec)
        {
            throw new NotImplementedException();
        }

        public void SetEntityState<TEntity, TEntityKey>(TEntity e)
            where TEntity : ObjectDefBase<TEntityKey>
            where TEntityKey : IComparable
        {
            throw new NotImplementedException();
        }

        public void SetEntityState<TEntity, TEntityKey>(TEntity e, out TEntity output)
            where TEntity : ObjectDefBase<TEntityKey>
            where TEntityKey : IComparable
        {
            throw new NotImplementedException();
        }

        public void Delete(T t)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Filter(IDataQuerySpecification<T> spec)
        {
            throw new NotImplementedException();
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            LoadList();
            _item = _list.AsQueryable().Where(predicate).FirstOrDefault();
            return _item;
        }
        public T Find(TKey key)
        {
            LoadList();
            _item = _list.Where(x => x.Key.Equals(key)).SingleOrDefault();
            return _item;
        }


        public void Initialize(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName", "The file name cannot be empty.");
            }
            _fileName = fileName;
        }



        #region "IDisposable Support"
        // To detect redundant calls
        private bool disposedValue;

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                // TODO: set large fields to null.
            }
            this.disposedValue = true;
        }

        // TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        //Protected Overrides Sub Finalize()
        //    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        //    Dispose(False)
        //    MyBase.Finalize()
        //End Sub

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region "Private methods"

        private void LoadItem()
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                throw new ArgumentNullException("fileName", "The file name cannot be empty.");
            }
            if (!(File.Exists(_fileName)))
            {
                _item = null;
            }
            else
            {
                using (FileStream fs = File.Open(_fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    XmlSerializer oSerializer = new XmlSerializer(typeof(T));
                    _item = (T)oSerializer.Deserialize(fs);
                }
            }
        }

        private void SaveItem()
        {
            using (FileStream fs = File.Open(_fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                XmlSerializer oSerializer = new XmlSerializer(typeof(T));
                oSerializer.Serialize(fs, _item);
            }
        }

        private void LoadList()
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                throw new ArgumentNullException("fileName", "The file name cannot be empty.");
            }
            if (!(File.Exists(_fileName)))
            {
                _list = new List<T>();
            }
            else
            {
                using (FileStream fs = File.Open(_fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    XmlSerializer oSerializer = new XmlSerializer(typeof(List<T>));
                    _list = (List<T>)oSerializer.Deserialize(fs);
                }
            }
        }

        private void SaveList()
        {
            using (FileStream fs = File.Open(_fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                XmlSerializer oSerializer = new XmlSerializer(typeof(List<T>));
                oSerializer.Serialize(fs, _list);
            }
        }
        #endregion

    }

}
