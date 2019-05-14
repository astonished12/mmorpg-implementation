using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using PubComp.NoSql.Core;

namespace PubComp.NoSql.ServiceStackRedis
{
    public abstract class RedisContext : IDomainContext
    {
        protected readonly ServiceStack.Redis.RedisClient innerContext;
        private readonly IEnumerable<IEntitySet> entitySets;

        public RedisContext(RedisConnectionInfo connectionInfo)
            : this(connectionString: connectionInfo.ConnectionString)
        {
        }

        public RedisContext(string host = "localhost", int port = 6379, string password = null, long db = 0)
            : this(new RedisConnectionInfo { Host = host, Port = port, Password = password, Db = db })
        {
        }

        public RedisContext(string connectionString)
        {
            this.innerContext = new ServiceStack.Redis.RedisClient(new Uri(connectionString));
            var entitySets = new List<IEntitySet>();

            var entitySetProperties =
                this.GetType().GetProperties()
                    .Where(p => p.PropertyType.IsGenericType
                        && (p.PropertyType.GetGenericTypeDefinition() == typeof(IEntitySet<,>)
                        || p.PropertyType.GetGenericTypeDefinition() == typeof(EntitySet<,>)
                        ));

            foreach (var prop in entitySetProperties)
            {
                var keyType = prop.PropertyType.GetGenericArguments()[0];
                var entityType = prop.PropertyType.GetGenericArguments()[1];

                var createEntitySetMethod = typeof(EntitySet<,>).MakeGenericType(new[] { keyType, entityType })
                    .GetConstructor(
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                        null, new[] { typeof(RedisContext) }, null);

                var entitySet = createEntitySetMethod.Invoke(new object[] { this });
                prop.SetValue(this, entitySet, null);

                entitySets.Add(entitySet as IEntitySet);
            }

            this.entitySets = entitySets;
        }

        public void Shutdown()
        {
            this.innerContext.Shutdown();
        }

        public void Dispose()
        {
            this.innerContext.Dispose();
        }

        public IEnumerable<IEntitySet> EntitySets
        {
            get
            {
                return this.entitySets;
            }
        }

        public IEntitySet<TKey, TEntity> GetEntitySet<TKey, TEntity>() where TEntity : class, IEntity<TKey>
        {
            var set = this.entitySets.Where(s => s.KeyType == typeof(TKey) && s.EntityType == typeof(TEntity))
                .FirstOrDefault() as IEntitySet<TKey, TEntity>;

            return set;
        }

        public IFileSet Files
        {
            get
            {
                return null;
            }
        }

        public void DeleteAll()
        {
            foreach (var entitySet in this.entitySets)
                (entitySet as EntitySet).DeleteAll();
        }

#if DEBUG
        public void SuperDeleteAll()
        {
            var keys = this.innerContext.GetAllKeys().ToArray();
            if (keys.Length > 0)
                this.innerContext.Del(keys);
        }
#endif

        internal ServiceStack.Redis.IRedisClient InnerContext
        {
            get
            {
                return this.innerContext;
            }
        }

        public abstract class EntitySet
        {
            public abstract void DeleteAll();
        }

        public class EntitySet<TKey, TEntity> : EntitySet, IEntitySet<TKey, TEntity> where TEntity : class, IEntity<TKey>
        {
            protected readonly RedisContext parent;
            protected readonly ServiceStack.Redis.Generic.RedisTypedClient<TEntity> innerSet;
            private readonly string keyHeader;

            static EntitySet()
            {
                var ignoreProperties = new List<PropertyInfo>();

                var properties = ContextUtils.GetProperiesOfTypeAndSubTypes(typeof(TEntity));

                foreach (var prop in properties)
                {
                    if (prop.Name == "Id" || !prop.CanRead || !prop.CanWrite || prop.GetIndexParameters().Any())
                        continue;

                    if (prop.GetCustomAttributes(typeof(DbIgnoreAttribute), true).Any()
                        || prop.GetCustomAttributes(typeof(NavigationAttribute), true).Any())
                    {
                        ignoreProperties.Add(prop);
                    }
                }

                var propsPerGroups = ignoreProperties.GroupBy(p => p.DeclaringType).ToList();

                var types = new List<Type> { typeof(TEntity) };
                var subTypes = ContextUtils.FindInheritingTypes(typeof(TEntity).Assembly, new[] { typeof(TEntity) });
                types.AddRange(subTypes);

                foreach (var type in types)
                {
                    var currentType = type;
                    var propNames = propsPerGroups.Where(g =>
                        g.Key == currentType || currentType.IsSubclassOf(g.Key))
                        .SelectMany(g => g).Select(p => p.Name).ToArray();

                    if (propNames.Any())
                        SetExcludePropertyNames(type, propNames);
                }
            }

            private static void SetExcludePropertyNames(Type type, string[] propertyNames)
            {
                var genericMethod = typeof(EntitySet<TKey, TEntity>).GetMethod("SetExcludePropertyNamesGeneric",
                    BindingFlags.NonPublic | BindingFlags.Static);

                var specificMethod = genericMethod.MakeGenericMethod(type);
                specificMethod.Invoke(null, new object[] { propertyNames });
            }

            private static void SetExcludePropertyNamesGeneric<T>(string[] propertyNames)
            {
                ServiceStack.Text.JsConfig<T>.ExcludePropertyNames = propertyNames;
            }

            internal EntitySet(RedisContext parent)
            {
                this.parent = parent;
                this.innerSet = new ServiceStack.Redis.Generic.RedisTypedClient<TEntity>(parent.innerContext);
                keyHeader = string.Concat("urn:", typeof(TEntity).Name.ToLower(), ':');
            }

            private string KeyToString(TKey key)
            {
                var result = keyHeader + key.ToString();
                return result;
            }

            public Type KeyType
            {
                get
                {
                    return typeof(TKey);
                }
            }

            public Type EntityType
            {
                get
                {
                    return typeof(TEntity);
                }
            }

            public IQueryable<TEntity> AsQueryable()
            {
                return this.innerSet.GetAll().AsQueryable();
            }

            IQueryable<IEntity> IEntitySet.AsQueryable()
            {
                return this.AsQueryable();
            }

            public IEnumerable<TEntity> Query(Expression<Func<TEntity, bool>> filter)
            {
                return this.innerSet.GetAll().AsQueryable().Where(filter);
            }

            public bool AddIfNotExists(TEntity entity)
            {
                if (EqualityComparer<TKey>.Default.Equals(entity.Id, default(TKey)))
                    throw new DalNullIdFailure("Could not add entity - entity.Id is undefined.", entity, DalOperation.Add);

                if (OnModifying != null && !Contains(entity.Id))
                    CheckIfCanModify(entity);

                var added = this.innerSet.SetEntryIfNotExists(KeyToString(entity.Id), entity);
                return added;
            }

            bool IEntitySet.AddIfNotExists(IEntity entity)
            {
                if (entity is TEntity == false)
                    throw new InvalidOperationFailure();

                return this.AddIfNotExists((TEntity)entity);
            }

            public void AddOrUpdate(TEntity entity)
            {
                if (EqualityComparer<TKey>.Default.Equals(entity.Id, default(TKey)))
                    throw new DalNullIdFailure("Could not add entity - entity.Id is undefined.", entity, DalOperation.Add);

                CheckIfCanModify(entity);

                this.innerSet.SetEntry(KeyToString(entity.Id), entity);
            }

            void IEntitySet.AddOrUpdate(IEntity entity)
            {
                if (entity is TEntity == false)
                    throw new InvalidOperationFailure();

                this.AddOrUpdate((TEntity)entity);
            }

            public TEntity GetOrAdd(TEntity entity)
            {
                if (EqualityComparer<TKey>.Default.Equals(entity.Id, default(TKey)))
                    throw new DalNullIdFailure("Could not add entity - entity.Id is undefined.", entity, DalOperation.Add);

                var key = entity.Id;

                bool canModify;
                CheckIfCanModify(entity, out canModify);

                if (!canModify && !Contains(key))
                    throw new DalAccessRestrictionFailure("Modify operation for this entity is forbidden.");

                var added = this.innerSet.SetEntryIfNotExists(KeyToString(key), entity);

                if (!added)
                    return ReturnAfterCheck(this.innerSet.GetById(key));
                else
                    return null;
            }

            IEntity IEntitySet.GetOrAdd(IEntity entity)
            {
                if (entity is TEntity == false)
                    throw new InvalidOperationException();

                return this.GetOrAdd((TEntity)entity);
            }

            public void Add(TEntity entity)
            {
                if (EqualityComparer<TKey>.Default.Equals(entity.Id, default(TKey)))
                    throw new DalNullIdFailure("Could not add entity - entity.Id is undefined.", entity, DalOperation.Add);

                CheckIfCanModify(entity);

                this.innerSet.Store(entity);
            }

            void IEntitySet.Add(IEntity entity)
            {
                if (entity is TEntity == false)
                    throw new InvalidOperationException();

                this.Add((TEntity)entity);
            }

            public void Add(IEnumerable<TEntity> entities)
            {
                if (entities.Any(entity => EqualityComparer<TKey>.Default.Equals(entity.Id, default(TKey))))
                    throw new DalNullIdFailure("Could not add entities - entity.Id is undefined for at least one entity.", default(TEntity), DalOperation.Add);

                CheckIfCanModify(entities);

                this.innerSet.StoreAll(entities);
            }

            void IEntitySet.Add(IEnumerable<IEntity> entities)
            {
                var typedEntities = entities.OfType<TEntity>().ToList();
                if (typedEntities.Count < entities.Count())
                    throw new InvalidOperationFailure();

                this.Add(typedEntities);
            }

            public TEntity Get(TKey key)
            {
                return ReturnAfterCheck(this.innerSet.GetById(key));
            }

            IEntity IEntitySet.Get(Object key)
            {
                if (key is TKey == false)
                    throw new InvalidOperationFailure();

                return this.Get((TKey)key);
            }

            public bool Contains(TKey key)
            {
                return this.innerSet.ContainsKey(KeyToString(key));
            }

            bool IEntitySet.Contains(Object key)
            {
                if (key is TKey == false)
                    throw new InvalidOperationFailure();

                return this.Contains((TKey)key);
            }

            public IEnumerable<TEntity> Get(IEnumerable<TKey> keys)
            {
                return Filter(this.innerSet.GetByIds(keys));
            }

            IEnumerable<IEntity> IEntitySet.Get(IEnumerable keys)
            {
                var typedKeys = keys.OfType<TKey>().ToList();
                return this.Get(typedKeys);
            }

            public void Update(TEntity entity)
            {
                if (EqualityComparer<TKey>.Default.Equals(entity.Id, default(TKey)))
                    throw new DalNullIdFailure("Could not update entity - entity.Id is undefined.", entity, DalOperation.Update);

                if (this.innerSet.GetById(entity.Id) == default(TEntity))
                    throw new DalItemNotFoundFailure("Could not update entity - entity not found in DB.", entity, DalOperation.Update);

                CheckIfCanModify(entity);
                UpdateExisting(entity);
            }

            void IEntitySet.Update(IEntity entity)
            {
                if (entity is TEntity == false)
                    throw new InvalidOperationFailure();

                this.Update((TEntity)entity);
            }

            private void UpdateExisting(TEntity entity)
            {
                this.innerSet.SetEntry(KeyToString(entity.Id), entity);
            }

            public void Update(IEnumerable<TEntity> entities)
            {
                CheckIfCanModify(entities);

                foreach (var entity in entities)
                    Update(entity);
            }

            void IEntitySet.Update(IEnumerable<IEntity> entities)
            {
                var typedEntities = entities.OfType<TEntity>().ToList();
                if (typedEntities.Count < entities.Count())
                    throw new InvalidOperationFailure();

                this.Update(typedEntities);
            }

            public void Delete(TEntity entity)
            {
                CheckIfCanDelete(entity);
                this.DeleteInner(entity.Id);
            }

            void IEntitySet.Delete(IEntity entity)
            {
                if (entity is TEntity == false)
                    throw new InvalidOperationFailure();

                this.Delete((TEntity)entity);
            }

            public void Delete(IEnumerable<TEntity> entities)
            {
                CheckIfCanDelete(entities);
                var keys = entities.Select(e => e.Id);
                this.DeleteInner(keys);
            }

            void IEntitySet.Delete(IEnumerable<IEntity> entities)
            {
                var typedEntities = entities.OfType<TEntity>().ToList();
                if (typedEntities.Count < entities.Count())
                    throw new InvalidOperationFailure();

                this.Delete(typedEntities);
            }

            public void Delete(TKey key)
            {
                if (EqualityComparer<TKey>.Default.Equals(key, default(TKey)))
                    throw new DalNullIdFailure("Could not delete entity - Id was null.", default(TEntity), DalOperation.Delete);

                if (OnDeleting != null)
                {
                    var entity = Get(key);
                    CheckIfCanDelete(entity);
                }

                this.innerSet.DeleteById(key);
            }

            private void DeleteInner(TKey key)
            {
                if (EqualityComparer<TKey>.Default.Equals(key, default(TKey)))
                    throw new DalNullIdFailure("Could not delete entity - Id was null.", default(TEntity), DalOperation.Delete);

                this.innerSet.DeleteById(key);
            }

            void IEntitySet.Delete(Object key)
            {
                if (key is TKey == false)
                    throw new InvalidOperationFailure();

                this.Delete((TKey)key);
            }

            public void Delete(IEnumerable<TKey> keys)
            {
                if (keys.Any(key => EqualityComparer<TKey>.Default.Equals(key, default(TKey))))
                    throw new DalNullIdFailure("Could not delete entities - at least one provided Id was null.", default(TEntity), DalOperation.Delete);

                if (OnDeleting != null)
                {
                    var entities = Get(keys);
                    CheckIfCanDelete(entities);
                }

                this.innerSet.DeleteByIds(keys);
            }

            private void DeleteInner(IEnumerable<TKey> keys)
            {
                if (keys.Any(key => EqualityComparer<TKey>.Default.Equals(key, default(TKey))))
                    throw new DalNullIdFailure("Could not delete entities - at least one provided Id was null.", default(TEntity), DalOperation.Delete);

                if (OnDeleting != null)
                {
                    var entities = Get(keys);
                    CheckIfCanDelete(entities);
                }

                this.innerSet.DeleteByIds(keys);
            }

            void IEntitySet.Delete(IEnumerable<Object> keys)
            {
                var typedKeys = keys.OfType<TKey>().ToList();
                if (typedKeys.Count < keys.Count())
                    throw new InvalidOperationFailure();

                this.Delete(typedKeys);
            }

            public override void DeleteAll()
            {
                this.innerSet.DeleteAll();
            }

            internal ServiceStack.Redis.Generic.RedisTypedClient<TEntity> InnerSet
            {
                get
                {
                    return this.innerSet;
                }
            }

            #region Navigation

            public void LoadNavigation(IEnumerable<TEntity> entities, IEnumerable<string> propertyNames)
            {
                ContextUtils.LoadNavigation<TKey, TEntity>(parent, entities, propertyNames);
            }

            public void SaveNavigation(IEnumerable<TEntity> entities, IEnumerable<string> propertyNames)
            {
                ContextUtils.SaveNavigation<TKey, TEntity>(parent, this, entities, propertyNames);
            }

            #endregion

            #region Events

            public event EventHandler<AccessEventArgs<TEntity>> OnModifying;

            public event EventHandler<AccessEventArgs<TEntity>> OnDeleting;

            public event EventHandler<AccessEventArgs<TEntity>> OnGetting;

            private void CheckIfCanModify(TEntity entity)
            {
                bool canAccess;
                CheckIfCanModify(entity, out canAccess);
                if (!canAccess)
                    throw new DalAccessRestrictionFailure("Modify operation for this entity is forbidden.");
            }

            private void CheckIfCanModify(IEnumerable<TEntity> entities)
            {
                if (this.OnModifying == null)
                    return;

                bool canAccess = true;
                foreach (var entity in entities)
                {
                    var args = new AccessEventArgs<TEntity>(entity);
                    this.OnModifying(this, args);
                    if (!args.CanAccess)
                    {
                        canAccess = false;
                        break;
                    }
                }

                if (canAccess)
                    throw new DalAccessRestrictionFailure("Modify operation for this entity is forbidden.");
            }

            private void CheckIfCanModify(TEntity entity, out bool canAccess)
            {
                canAccess = true;
                if (this.OnModifying == null)
                    return;

                var args = new AccessEventArgs<TEntity>(entity);
                this.OnModifying(this, args);
                canAccess = args.CanAccess;
            }

            private void CheckIfCanDelete(TEntity entity)
            {
                bool canAccess;
                CheckIfCanDelete(entity, out canAccess);
                if (!canAccess)
                    throw new DalAccessRestrictionFailure("Modify operation for this entity is forbidden.");
            }

            private void CheckIfCanDelete(IEnumerable<TEntity> entities)
            {
                if (this.OnDeleting == null)
                    return;

                bool canAccess = true;
                foreach (var entity in entities)
                {
                    var args = new AccessEventArgs<TEntity>(entity);
                    this.OnDeleting(this, args);
                    if (!args.CanAccess)
                    {
                        canAccess = false;
                        break;
                    }
                }

                if (canAccess)
                    throw new DalAccessRestrictionFailure("Modify operation for this entity is forbidden.");
            }

            private void CheckIfCanDelete(TEntity entity, out bool canAccess)
            {
                canAccess = true;
                if (this.OnDeleting == null)
                    return;

                var args = new AccessEventArgs<TEntity>(entity);
                this.OnDeleting(this, args);
                canAccess = args.CanAccess;
            }

            private TEntity ReturnAfterCheck(TEntity entity)
            {
                if (this.OnGetting == null)
                    return entity;

                var args = new AccessEventArgs<TEntity>(entity);
                this.OnGetting(this, args);
                if (!args.CanAccess)
                    throw new DalAccessRestrictionFailure("Modify operating for this entity is forbidden.");

                return entity;
            }

            public IEnumerable<TEntity> Filter(IEnumerable<TEntity> entities)
            {
                if (OnGetting == null)
                    return entities;

                var results = new List<TEntity>();
                foreach (var entity in entities)
                {
                    var args = new AccessEventArgs<TEntity>(entity);
                    OnGetting(this, args);
                    if (args.CanAccess)
                        results.Add(entity);
                }

                return results;
            }

            #endregion
        }
    }
}
