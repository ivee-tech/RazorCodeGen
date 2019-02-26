using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

using Iv.Data;

namespace Iv.Common
{
    public class ObjectDefBase<TKey> : INotifyPropertyChanging, INotifyPropertyChanged
        where TKey : IComparable
    {

        private bool _isInactive = false;
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        /*
         * public setter left for WCF serialization requirements.
         * Recommended use of SetNew / SetDirty / SetDeleted, not the public setter.
         * However, proper security check overcomes this issue.
        */
        [Browsable(false)]
        public bool IsNew { get; set; }
        [Browsable(false)]
        public bool IsDirty { get; set; }
        [Browsable(false)]
        public bool IsDeleted { get; set; }
        [Browsable(false)]
        public bool IsInactive
        {
            get
            {
                return _isInactive;
            }
            set
            {
                _isInactive = value;
                if (_isInactive)
                {
                    SetDirty();
                }
            }
        }
        public virtual TKey Key { get; set; }

        public virtual string Name { get; set; }

        public virtual void SetNew()
        {
            Clean();
            IsNew = true;
        }

        public virtual void SetDirty()
        {
            Clean();
            IsDirty = true;
        }

        public virtual void SetDeleted()
        {
            Clean();
            IsDeleted = true;
        }

        public virtual void SetInactive()
        {
            Clean();
            IsInactive = true;
        }

        public void Clean()
        {
            IsNew = IsDirty = IsDeleted = false;
        }

        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, e);
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            SetDirty();
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public virtual void SetRefRelationship()
        {

        }
    }
}
