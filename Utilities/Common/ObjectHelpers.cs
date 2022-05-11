using System.Collections.Generic;
using System.Linq;

namespace Utilities.Common
{
    public static class ObjectHelpers
    {
        public static TU MapToModel<T, TU>(this T source) where TU : new()
        {
            if (source == null) return default(TU);

            var dest = new TU();
            var sourceProps = typeof(T).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(TU).GetProperties()
                    .Where(x => x.CanWrite)
                    .ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.Any(x => x.Name == sourceProp.Name))
                {
                    var p = destProps.First(x => x.Name == sourceProp.Name);
                    if (p.CanWrite)
                    {
                        p.SetValue(dest, sourceProp.GetValue(source, null), null);
                    }
                }
            }
            return dest;
        }

        public static List<TU> MapToListModels<T, TU>(this List<T> source) where TU : new()
        {
            if (source == null) return new List<TU>();

            var dest = new List<TU>();

            foreach (var item in source)
            {
                var destItem = item.MapToModel<T, TU>();
                dest.Add(destItem);
            }

            return dest;
        }
    }
}