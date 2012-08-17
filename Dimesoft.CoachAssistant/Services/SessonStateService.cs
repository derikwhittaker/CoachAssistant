using System.Collections.Generic;

namespace Dimesoft.CoachAssistant.Services
{
    public interface ISessonStateService
    {
        void Set(string key, object value );
        object TryGet(string key);
        void Clear();
    }

    public class SessonStateService : ISessonStateService
    {
        private static IDictionary<string, object> _cache = new Dictionary<string, object>();

        public void Set(string key, object value )
        {
            if (_cache.ContainsKey(key))
            {
                _cache[key] = value;
            }
            else
            {
                _cache.Add(key, value);    
            }
        }

        public object TryGet(string key)
        {
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }

            return null;
        }

        public void Clear()
        {
            _cache.Clear();
        }
    }
}
