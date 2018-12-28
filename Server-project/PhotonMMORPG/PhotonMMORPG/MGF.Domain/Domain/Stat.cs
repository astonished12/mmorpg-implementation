using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGF.Domain
{
    public class Stat : DomainBase
    {
        #region Private Fields

        private int id;
        private string name;
        private int value;

        private static Stat nullValue = new Stat();

        private int characterId;
        private Character character;
        #endregion

        #region Properties

        public int Id
        {
            get { return id; }
            set
            {
                if (null == value)
                {
                    value = 0;
                }

                if (id != value)
                {
                    Id = value;
                    PropertyHasChanged(nameof(Id));
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (null == value)
                {
                    value = string.Empty;
                }

                if (name != value)
                {
                    name = value;
                    PropertyHasChanged(nameof(Name));
                }

            }
        }

        public int Value
        {
            get { return value; }
            set
            {
                if (null == value)
                {
                    value = 0;
                }

                if (this.value != value)
                {
                    this.value = value;
                    PropertyHasChanged(nameof(Value));
                }
            }
        }

        public int CharacterId
        {
            get { return characterId; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(CharacterId));
                }

                if (this.characterId != value)
                {
                    this.characterId = value;
                    PropertyHasChanged(nameof(CharacterId));
                }
            }
        }

        #endregion

            #region Constructs

        public Stat()
        {

        }

        public Stat(int id, string name, int value)
        {
            this.id = id;
            this.name = name;
            this.value = value;
            base.MarkOld();
        }

        #endregion

        #region Methods
        //BUSSINESS LOGIC
        public override bool Equals(object obj)
        {
            if (null == obj)
            {
                return false;
            }

            Stat other = obj as Stat;
            if (null == other)
            {
                return false;
            }

            return this.GetHashCode().Equals(other.GetHashCode());
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, "{0}: {1} - {3} ({2})",
                this.GetType(), this.Name, this.Id, this.value);
        }

        public static Stat NullValue
        {
            get { return nullValue; }
        }
    }
    #endregion

}


