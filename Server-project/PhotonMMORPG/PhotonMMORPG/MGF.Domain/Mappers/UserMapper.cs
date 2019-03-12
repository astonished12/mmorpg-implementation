using System;
using System.Collections.Generic;
using System.Data.Entity;
using MGF.Domain;
using System.Linq;

namespace MGF.Mappers
{
    public class UserMapper : MapperBase<User>
    {
        protected override User Delete(User domainObject)
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
                DataEntities.User entity = new DataEntities.User() { Id = id };
                entities.Users.Attach(entity);
                //Remove the character from container
                entities.Users.Remove(entity);
                entities.SaveChanges();
            }
        }

        //Get a list of all character in the database
        protected override IList<User> Fetch()
        {
            using (MGFContext entities = new MGFContext())
            {
                return entities.Users
                    //Not cache entities
                    .AsNoTracking()
                    .OrderBy(userEntity => userEntity.Id)
                    .ToList()
                    .Select(userEntity => new User(
                        userEntity.Id,
                        userEntity.LoginName,
                        userEntity.PasswordHash,
                        userEntity.Salt))
                    .ToList();
            }
        }

        protected override User Fetch(int id)
        {
            User userObject = null;
            using (MGFContext entities = new MGFContext())
            {
                DataEntities.User entity = entities.Users
                    //.Include(characterEntity => characterEntity.Stats)
                    .FirstOrDefault(userEntity => userEntity.Id == id);

                if (entity != null)
                {
                    userObject = new User(entity.Id, entity.LoginName, entity.PasswordHash, entity.Salt);
                }
            }

            return userObject;
        }

        protected override User Insert(User domainObject)
        {
            using (MGFContext entities = new MGFContext())
            {
                DataEntities.User entity = new DataEntities.User();
                Map(domainObject, entity);
                entities.Users.Add(entity);
                domainObject = SaveChanges(entities, entity);
            }

            return domainObject;
        }

        //one way mapping
        protected override void Map(User domainObject, object entity)
        {
            DataEntities.User userEntity = entity as DataEntities.User;
            if (null == domainObject)
            {
                throw new ArgumentNullException(nameof(domainObject));
            }

            if (null == entity)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (null == userEntity)
            {
                throw new ArgumentOutOfRangeException(nameof(entity));
            }

            userEntity.LoginName = domainObject.LoginName;
            userEntity.PasswordHash = domainObject.PasswordHash;
            userEntity.Salt = domainObject.Salt;

        }

        protected override User Update(User domainObject)
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
                DataEntities.User entity = entities.Users
                    .Include(userEntity => userEntity.Characters)
                    .FirstOrDefault(userEntity => userEntity.Id == id);

                if (entity != null)
                {
                    Map(domainObject, entity);
                    domainObject = SaveChanges(entities, entity);
                }
            }
            return domainObject;
        }

        private User SaveChanges(MGFContext entities, DataEntities.User entity)
        {
            //Save everything in the context (unit of work means it should only be this entity)
            entities.SaveChanges();
            //relog what the databases has based on the Id that we modified
            return Fetch(entity.Id);
        }

        public static IList<Character> LoadCharacters(User domainObject)
        {
            int id;
            List<Character> characters;
            if (null == domainObject)
            {
                throw new ArgumentNullException(nameof(domainObject));
            }

            id = domainObject.Id;
            characters = new List<Character>();
            using (MGFContext entities = new MGFContext())
            {
                var query = entities.Characters
                    .Where(characterEntity => characterEntity.UserId == id);
                foreach (var character in query)
                {
                    characters.Add(new Character(character.Id, character.UserId, character.Name, character.Class, character.Level, character.Loc_x, character.Loc_y, character.Loc_z, character.ExperiencePoints));
                }
            }

            return characters;
        }

        public static User LoadByUserName(string loginName)
        {
            User userObject = null;
            using (MGFContext entities = new MGFContext())
            {
                DataEntities.User entity = entities.Users
                    //.Include(characterEntity => characterEntity.Stats)
                    .FirstOrDefault(userEntity => userEntity.LoginName == loginName);

                if (entity != null)
                {
                    userObject = new User(entity.Id, entity.LoginName, entity.PasswordHash, entity.Salt);
                }
            }

            return userObject;
        }
    }
}
