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
        private string _class;
        private int level;
        private decimal experiencePoints;
        private float loc_x;
        private float loc_y;
        private float loc_z;

        private int userId;
        private User user;

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
                    id = value;
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

        public string Class
        {
            get { return _class; }
            set
            {
                if (null == value)
                {
                    value = string.Empty;
                }

                if (_class != value)
                {
                    _class = value;
                    PropertyHasChanged(nameof(Class));
                }
            }
        }

        public int Level
        {
            get { return level; }
            set
            {
                if (null == value)
                {
                    value = 0;
                }

                if (level != value)
                {
                    level = value;
                    PropertyHasChanged(nameof(Level));
                }
            }
        }

        public decimal ExperiencePoints
        {
            get { return experiencePoints; }
            set
            {
                if (null == value)
                {
                    value = 0;
                }

                if (experiencePoints != value)
                {
                    experiencePoints = value;
                    PropertyHasChanged(nameof(ExperiencePoints));
                }
            }
        }


        public float Loc_X
        {
            get { return loc_x; }
            set
            {
                if (null == value)
                {
                    value = 0;
                }

                if (loc_x != value)
                {
                    loc_x = value;
                    PropertyHasChanged(nameof(Loc_X));
                }
            }
        }

        public float Loc_Y
        {
            get { return loc_y; }
            set
            {
                if (null == value)
                {
                    value = 0;
                }

                if (loc_y != value)
                {
                    loc_y = value;
                    PropertyHasChanged(nameof(Loc_Y));
                }
            }
        }

        public float Loc_Z
        {
            get { return loc_z; }
            set
            {
                if (null == value)
                {
                    value = 0;
                }

                if (loc_z != value)
                {
                    loc_z = value;
                    PropertyHasChanged(nameof(Loc_Z));
                }
            }
        }

        public int UserId
        {
            get { return userId; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(UserId));
                }

                if (this.userId != value)
                {
                    this.userId = value;
                    PropertyHasChanged(nameof(UserId));
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

        public Character(int id, string name, string _class, int level, float loc_x, float loc_y, float loc_z, decimal experiencePoints)
        {
            this.id = id;
            this.name = name;
            this.Class = _class;
            this.Level = level;
            this.loc_x = loc_x;
            this.loc_y = loc_y;
            this.loc_z = loc_z;

            this.experiencePoints = experiencePoints;
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

