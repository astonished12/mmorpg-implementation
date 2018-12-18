using System;
using System.Collections.Generic;
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
                stats = IsNew || 0 == id ? new List<Stat>() : CharacterMapper.LoadStats(this);
            }
        }
    }
    #endregion
}

