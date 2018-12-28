using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGF.Domain;

namespace MGF.Mappers
{
    public class StatMapper : MapperBase<Stat>
    {
        protected override Stat Delete(Stat domainObject)
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
                DataEntities.Stat entity = new DataEntities.Stat() { StatId = id };
                entities.Stats.Attach(entity);
                //Remove the character from container
                entities.Stats.Remove(entity);
                entities.SaveChanges();
            }
        }

        protected override IList<Stat> Fetch()
        {
            using (MGFContext entities = new MGFContext())
            {
                return entities.Stats
                    //Not cache entities
                    .AsNoTracking()
                    .OrderBy(statEntity => statEntity.StatId)
                    .ToList()
                    .Select(statEntity => new Stat(
                        statEntity.StatId,
                        statEntity.Name,
                        statEntity.Value))
                    .ToList();
            }
        }

        protected override Stat Fetch(int id)
        {
            Stat statObject = null;
            using (MGFContext entities = new MGFContext())
            {
                DataEntities.Stat entity = entities.Stats
                    //.Include(characterEntity => characterEntity.Stats)
                    .FirstOrDefault(statEntity => statEntity.StatId == id);

                if (entity != null)
                {
                    statObject = new Stat(entity.StatId, entity.Name, entity.Value);
                }
            }

            return statObject;
        }

        protected override Stat Insert(Stat domainObject)
        {
            using (MGFContext entities = new MGFContext())
            {
                DataEntities.Stat entity = new DataEntities.Stat();
                Map(domainObject, entity);
                entities.Stats.Add(entity);
                domainObject = SaveChanges(entities, entity);
            }

            return domainObject;
        }

        protected override void Map(Stat domainObject, object entity)
        {
            DataEntities.Stat statEntity = entity as DataEntities.Stat;
            if (null == domainObject)
            {
                throw new ArgumentNullException(nameof(domainObject));
            }

            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (null == statEntity)
            {
                throw new ArgumentOutOfRangeException(nameof(entity));
            }

            statEntity.Name = domainObject.Name;
            statEntity.Value = domainObject.Value;
        }

        public void MapStat(Stat domainObject, object entity)
        {
            Map(domainObject, entity);
        }

        protected override Stat Update(Stat domainObject)
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
                DataEntities.Stat entity = entities.Stats
                    .FirstOrDefault(statEntity => statEntity.StatId == id);

                if (entity != null)
                {
                    Map(domainObject, entity);
                    domainObject = SaveChanges(entities, entity);
                }
            }
            return domainObject;
        }

        private Stat SaveChanges(MGFContext entities, DataEntities.Stat entity)
        {
            //Save everything in the context (unit of work means it should only be this entity)
            entities.SaveChanges();
            //relog what the databases has based on the Id that we modified
            return Fetch(entity.StatId);
        }
    }
}
