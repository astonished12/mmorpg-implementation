using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MGF.Domain
{
    [Serializable]
    public abstract class DomainBase : IProcessDirty
    {
        //Keep track of whether object is new deleted or dirty
        private bool isObjectNew = true;
        private bool isObjectDirty = true;
        private bool isObjectDeleted;

        #region IProcessDirty Members

        [Browsable(false)]
        public bool IsNew
        {
            get { return isObjectNew; }
            set { isObjectNew = value; }
        }

        [Browsable(false)]
        public bool IsDirty
        {
            get { return isObjectDirty; }
            set { isObjectDirty = value; }
        }

        [Browsable(false)]
        public bool IsDeleted
        {
            get { return isObjectDeleted; }
            set { isObjectDeleted = value; }
        }

        #endregion

        [NonSerialized()] public PropertyChangedEventHandler _nonSerializableHandlers;
        public PropertyChangedEventHandler _serializableHandlers;

        [Browsable(false),
         XmlIgnore()]
        public virtual bool IsSavable
        {
            //validation here
            get { return isObjectDirty; }
        }

        //Pattern from CSLA.NET - a Domain Driven Design Pattern based on BindableBase -cslanet.com
        //Necessary to make a serialization work propely and more important safley
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if ((value.Method.IsPublic) && (value.Method.DeclaringType.IsSerializable || value.Method.IsStatic))
                {
                    _serializableHandlers =
                        (PropertyChangedEventHandler)Delegate.Combine(_serializableHandlers, value);
                }
                else
                {
                    _nonSerializableHandlers =
                        (PropertyChangedEventHandler)Delegate.Combine(_nonSerializableHandlers, value);
                }

            }

            remove
            {
                if ((value.Method.IsPublic) && (value.Method.DeclaringType.IsSerializable || value.Method.IsStatic))
                {
                    _serializableHandlers = (PropertyChangedEventHandler)Delegate.Remove(_serializableHandlers, value);
                }
                else
                {
                    _nonSerializableHandlers =
                        (PropertyChangedEventHandler)Delegate.Remove(_nonSerializableHandlers, value);
                }

            }
        }

        //Automatically called by MarkDirty
        protected virtual void OnUnkownPropertyChanged()
        {
            OnPropertyChanged(string.Empty);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (_nonSerializableHandlers != null)
            {
                _nonSerializableHandlers.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            if (_serializableHandlers != null)
            {
                _serializableHandlers.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void MarkNew()
        {
            isObjectNew = true;
            isObjectDeleted = false;
            MarkDirty();
        }

        protected virtual void MarkOld()
        {
            isObjectNew = false;
            MarkClean();
        }

        protected void MarkDeleted()
        {
            isObjectDeleted = true;
            MarkDirty();
        }

        protected void MarkDirty()
        {
            MarkDirty(false);
        }

        protected void MarkDirty(bool surpressEvent)
        {
            isObjectDirty = true;
            if (!surpressEvent)
            {
                //Force propreties to refresh - only usfull for web pages and windows form
                OnUnkownPropertyChanged();
            }
        }

        protected void PropertyHasChanged()
        {
            PropertyHasChanged(new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name.Substring(4));
        }

        protected virtual void PropertyHasChanged(string propertyName)
        {
            MarkDirty(true);
            OnPropertyChanged(propertyName);
        }

        protected void MarkClean()
        {
            isObjectDirty = false;
        }

        public virtual void Delete()
        {
            this.MarkDeleted();
        }
    }

}
