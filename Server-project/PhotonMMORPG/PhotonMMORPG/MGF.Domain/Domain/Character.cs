using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGF.Mappers;

namespace MGF.Domain
{
    [Serializable]
    public class Character : DomainBase
    {
        #region Private Fields

        private int id;
        private string name;

        private List<Stat> stats;
        private static Character nullValue = new Character();
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

        public List<Stat> Stats
        {
            get
            {
                EnsureStatListExists();
                return stats;
            }

        }
        #endregion

        #region Constructs

        public Character() : base() { }

        public Character(int id, string name)
        {
            this.id = id;
            this.name = name;
            base.MarkOld();
        }
        #endregion

        #region Methods
        //BUSSINESS LOGIC

        protected void EnsureStatListExists()
        {
            if (null == stats)
            {
                stats = (IsNew || 0 == id)
                    ? new List<Stat>() 
                    : CharacterMapper.LoadStats(this).ToList();
            }
        }

        public override bool Equals(object obj)
        {
            if (null == obj)
            {
                return false;
            }

            Character other = obj as Character;
            if (null == other)
            {
                return false;
            }

            return this.GetHashCode().Equals(other.GetHashCode()) &&
                   this.Stats.SequenceEqual(other.Stats);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, "{0}: {1} {2}",
                this.GetType(), this.Name, this.Id);
        }

        public static Character NullValue
        {
            get { return nullValue; }
        }
    }
    #endregion
}

