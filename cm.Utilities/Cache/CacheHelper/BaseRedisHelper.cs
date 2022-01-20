using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cm.Utilities.Cache.CacheHelper
{
    public class BaseRedisHelper
    {
        private readonly string _connectionString;
        private ConnectionMultiplexer _conn;
        private readonly Dictionary<string, ConnectionMultiplexer> dicConn = new Dictionary<string, ConnectionMultiplexer>();
        private readonly object _synRoot = new object();
        private IDatabase _db { get; set; }
        private string _namespace = string.Empty;

        public BaseRedisHelper()
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();
            var Configuration = builder.Build();
            //_connectionString = Configuration.GetSection("Redis:ConnectionString").Value;
            var redisCacheSettings = new RedisCacheSettings();
            Configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);
            services.AddSingleton(redisCacheSettings);
        }

        private IDatabase GetDBInstance()
        {
            try
            {
                lock (_synRoot)
                {
                    if (_db == null)
                    {
                        _db = GetConnection().GetDatabase();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
            return _db;
        }

        protected ConnectionMultiplexer GetConnection()
        {
            try
            {
                lock (_synRoot)
                {
                    if (!dicConn.TryGetValue(Namespace, out _conn) || _conn == null || !_conn.IsConnected)
                    {
                        _conn = ConnectionMultiplexer.Connect(_connectionString);

                        if (dicConn.ContainsKey(Namespace))
                        {
                            dicConn[Namespace] = _conn;
                        }
                        else
                        {
                            dicConn.Add(Namespace, _conn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
            return _conn;
        }

        public string Namespace
        {
            get
            {
                return _namespace;
            }
            set
            {
                _namespace = value;
                if (!string.IsNullOrEmpty(_namespace))
                {
                    _namespace = _namespace.Trim();
                }
            }
        }

        protected string NameOf(object entity)
        {
            var name = "";
            if (entity != null)
            {
                if (string.IsNullOrEmpty(Namespace))
                    name = entity.GetType().Name.ToLower();
                else
                    name = (Namespace + ":" + entity.GetType().Name).ToLower();
            }
            return name;
        }

        public bool AddInHash<T>(string suffix, string key, T document) where T : class, new()
        {
            try
            {
                var valueJson = JsonConvert.SerializeObject(document);

                bool returnValue;
                var db = GetDBInstance();
                var hashid = typeof(T).Name.ToString().ToLower() + suffix;
                returnValue = db.HashSet(hashid, key, valueJson);

                return returnValue;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddInHashAsync<T>(string suffix, string key, T document) where T : class, new()
        {
            try
            {
                var valueJson = JsonConvert.SerializeObject(document);

                bool returnValue;

                var db = GetDBInstance();
                var hashid = typeof(T).Name.ToString().ToLower() + suffix;
                returnValue = await db.HashSetAsync(hashid, key, valueJson);

                return returnValue;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }

        public bool UpdateInHash<T>(string suffix, string key, T document) where T : class, new()
        {
            try
            {
                var hashid = typeof(T).Name.ToString().ToLower() + suffix;

                bool returnValue = false;
                var db = GetDBInstance();
                if (!db.HashExists(hashid, key))
                {
                    db.HashSet(hashid, key, string.Empty);
                }
                var exist = db.HashGet(hashid, key);
                var existData = JsonConvert.DeserializeObject<List<T>>(exist);
                if (existData == null) existData = new List<T>();
                existData.Add(document);
                var valueJson = JsonConvert.SerializeObject(existData);

                returnValue = db.HashSet(hashid, key, valueJson);

                return returnValue;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateInHashAsync<T>(string suffix, string key, T document) where T : class, new()
        {
            try
            {
                var hashid = typeof(T).Name.ToString().ToLower() + suffix;

                bool returnValue = false;
                var db = GetDBInstance();
                if (!(await db.HashExistsAsync(hashid, key)))
                {
                    db.HashSet(hashid, key, string.Empty);
                }
                var exist = await db.HashGetAsync(hashid, key);
                var existData = JsonConvert.DeserializeObject<List<T>>(exist);
                if (existData == null) existData = new List<T>();
                existData.Add(document);
                var valueJson = JsonConvert.SerializeObject(existData);

                returnValue = await db.HashSetAsync(hashid, key, valueJson);

                return returnValue;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }

        public T GetFromHash<T>(string hashId, string key) where T : class, new()
        {
            try
            {
                T returnValue = default(T);

                var db = GetDBInstance();
                var valueReply = db.HashGet(NameOf(hashId), key);
                if (valueReply.HasValue)
                {
                    returnValue = JsonConvert.DeserializeObject<T>(valueReply);
                }

                return returnValue;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }

        public async Task<T> GetFromHashAsync<T>(string hashId, string key) where T : class, new()
        {
            try
            {
                T returnValue = default(T);

                var db = GetDBInstance();
                var valueReply = await db.HashGetAsync(NameOf(hashId), key);
                if (valueReply.HasValue)
                {
                    returnValue = JsonConvert.DeserializeObject<T>(valueReply);
                }

                return returnValue;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }

        public bool RemoveFromHash(string hashId, string key)
        {
            try
            {
                var db = GetDBInstance();
                return db.HashDelete(NameOf(hashId), key);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }

        public bool RemoveMuiltiFromHash<T>(string suffix, List<string> keys) where T : class, new()
        {
            try
            {
                var hashId = typeof(T).Name + suffix;
                long returnValue;

                var db = GetDBInstance();

                var valueReplies = new List<RedisValue>();
                valueReplies.AddRange(keys.Select(s => (RedisValue)s));

                returnValue = db.HashDelete(NameOf(hashId), valueReplies.ToArray());

                return returnValue > 0;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }
    }
}