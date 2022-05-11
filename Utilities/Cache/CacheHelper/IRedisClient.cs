using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utilities.Cache.CacheHelper
{
    public interface IRedisClient
    {
        bool Add(string key, string value);

        bool Add(string key, string value, DateTime expiresAt);

        bool Add<T>(string key, T document) where T : class, new();

        bool Add<T>(string key, T document, DateTime expiresAt) where T : class, new();

        bool Add<T>(string key, List<T> documents) where T : class, new();

        bool Add<T>(string key, List<T> documents, long expiresTime) where T : class, new();

        bool AddInHash<T>(string hashId, string key, T document) where T : class, new();

        bool AddInHash<T>(string key, T document) where T : class, new();

        bool AddInHash<T>(string hashId, IEnumerable<KeyValuePair<string, T>> data) where T : class, new();

        bool AddInHash<T>(IEnumerable<KeyValuePair<string, T>> data) where T : class, new();

        bool AddInHashCustom<T>(string extHashName, string key, T document) where T : class, new();

        bool AddInHashWithCustomName(string hashName, string key, string value);

        bool AddInHashWithCustomName(string[] hashName, IEnumerable<KeyValuePair<string, string>> data);

        bool AddInHashWithCustomName<T>(string[] hashName, IEnumerable<KeyValuePair<string, T>> data);

        bool SetExpireKey(string key, TimeSpan expireTime);

        bool AddInSortedSet(string key, string value, double score);

        bool AddInSortedSet<T>(string value, double score) where T : class, new();

        T Get<T>(string key) where T : class, new();

        List<T> Gets<T>(string key) where T : class, new();

        T GetFromHash<T>(string hashId, string key) where T : class, new();

        T GetFromHash<T>(string key) where T : class, new();

        Task<T> GetFromHashAsync<T>(string hashId, string key) where T : class, new();

        Task<T> GetFromHashAsync<T>(string key) where T : class, new();

        double? GetScoreFromSortedSet(string key, string value);

        double? GetScoreFromSortedSet<T>(string value) where T : class, new();

        List<T> GetsFromHash<T>(string hashId, List<RedisValue> keys) where T : class, new();

        List<T> GetsFromHash<T>(List<string> keys) where T : class, new();

        List<T> GetsFromHash<T>(string hashId, List<string> keys) where T : class, new();

        List<T> GetsFromHash<T>(string hashId) where T : class, new();

        List<T> GetsFromHash<T>() where T : class, new();

        string GetsFromHashWithCustomKey(string hashId, string key);

        List<T> GetKeysFromHash<T>(string hashId, List<RedisValue> keys) where T : class, new();

        List<T> GetKeysFromHash<T>(List<string> keys) where T : class, new();

        List<T> GetKeysFromHash<T>(string hashId, List<string> keys) where T : class, new();

        List<T> GetKeysFromHash<T>(string hashId) where T : class, new();

        bool Remove(string key);

        bool Remove<T>(string key) where T : class, new();

        bool RemoveFromHash<T>(string hashId, string key) where T : class, new();

        bool RemoveFromHash<T>(string key) where T : class, new();

        bool RemoveMuiltiFromHash<T>(List<string> keys) where T : class, new();

        bool RemoveFromSortedSet(string key, string value);

        bool RemoveFromSortedSet<T>(string value) where T : class, new();

        bool RemoveScoreFromSortedSet(string key, double score);

        bool RemoveScoreFromSortedSet<T>(double score) where T : class, new();
    }
}