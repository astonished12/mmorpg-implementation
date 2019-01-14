using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGF.Domain;

namespace MGF.Mappers
{
    public class CharacterMapper : MapperBase<Character>
    {
        protected override Character Delete(Character domainObject)
        {
            if (null == domainObject)
            {
                throw new ArgumentNullException(nameof(domainObject));
            }

            //Call delete now and return the object
            DeleteNow(domainObject.Id);
            return domainObject;
        }

        protected override void DeleteNow(int id)
        {
            using (MGFContext entities = new MGFContext())
            {
                DataEntities.Character entity = new DataEntities.Character() { Id = id };
                entities.Characters.Attach(entity);
                //Remove the character from container
                entities.Characters.Remove(entity);
                entities.SaveChanges();
            }
        }

        //Get a list of all character in the database
        protected override IList<Character> Fetch()
        {
            using (MGFContext entities = new MGFContext())
            {
                return entities.Characters
                    //Not cache entities
                    .AsNoTracking()
                    .OrderBy(characterEntity => characterEntity.Id)
                    .ToList()
                    .Select(characterEntity => new Character(
                        characterEntity.Id,
                        characterEntity.Name)).ToList();
            }
        }

        protected override Character Fetch(int id)
        {
            Character characterObj = null;
            using (MGFContext entities = new MGFContext())
            {
                DataEntities.Character entity = entities.Characters
                    .Include(characterEntity => characterEntity.Stats)
                    .FirstOrDefault(characterEntity => characterEntity.Id == id);

                if (entity != null)
                {
                    characterObj = new Character(entity.Id, entity.Name);
                }
            }

            return characterObj;
        }

        protected override Character Insert(Character domainObject)
        {
            using (MGFContext entities = new MGFContext())
            {
                DataEntities.Character entity = new DataEntities.Character();
                Map(domainObject, entity);
                entities.Characters.Add(entity);
                domainObject = SaveChanges(entities, entity);
            }

            return domainObject;
        }

        public IList<Character> LoadByUserId(int userId)
        {
            using (MGFContext entities = new MGFContext())
            {
                return entities.Characters
                    //Not cache entities
                    .AsNoTracking()
                    .OrderBy(characterEntity => characterEntity.Id)
                    .Where(characterEntity => characterEntity.UserId == userId)
                    .ToList()
                    .Select(characterEntity => new Character(
                        characterEntity.Id,
                        characterEntity.Name)).ToList();
            }
        }

        //one way mapping
        protected override void Map(Character domainObject, object entity)
        {
            DataEntities.Character characterEntity = entity as DataEntities.Character;
            if (null == domainObject)
            {
                throw new ArgumentNullException(nameof(domainObject));
            }

            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (null == characterEntity)
            {
                throw new ArgumentOutOfRangeException(nameof(entity));
            }

            characterEntity.Name = domainObject.Name;
            foreach (var stat in domainObject.Stats)
            {
                DataEntities.Stat statEntity = null;
                StatMapper mapper = new StatMapper();
                mapper.MapStat(stat, statEntity);
                characterEntity.Stats.Add(statEntity);
            }
        }

        protected override Character Update(Character domainObject)
        {
            //Pull out the id beacause we ll using it in a lambda that might be deferred when calling and the thread
            int id;

            if (null == domainObject)
            {
                throw new ArgumentNullException(nameof(domainObject));
            }

            id = domainObject.Id;
            using (MGFContext entities = new MGFContext())
            {
                DataEntities.Character entity = entities.Characters
                    .Include(characterEntity => characterEntity.Stats)
                    .FirstOrDefault(characterEntity => characterEntity.Id == id);

                if (entity != null)
                {
                    Map(domainObject, entity);
                    domainObject = SaveChanges(entities, entity);
                }
            }
            return domainObject;
        }

        private Character SaveChanges(MGFContext entities, DataEntities.Character entity)
        {
            //Save everything in the context (unit of work means it should only be this entity)
            entities.SaveChanges();
            //relog what the databases has based on the Id that we modified
            return Fetch(entity.Id);
        }

        public static IList<Stat> LoadStats(Character domainObject)
        {
            int id;
            List<Stat> stats;
            if (null == domainObject)
            {
                throw new ArgumentNullException(nameof(domainObject));
            }

            id = domainObject.Id;
            stats = new List<Stat>();
            using (MGFContext entities = new MGFContext())
            {
                var query = entities.Stats
                    .Where(statEntity => statEntity.CharacterId == id);
                foreach (var stat in query)
                {
                        stats.Add(new Stat(stat.StatId, stat.Name, stat.Value));
                }
            }

            return stats;
        }
    }
}
