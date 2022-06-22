using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Utilities.Common
{
    public class SortingUtility
    {
        public enum SortOrders
        {
            Asc = 1,
            Desc = 2
        }

        public class SortingParams
        {
            public SortOrders SortOrder { get; set; } = SortOrders.Asc;
            public string ColumnName { get; set; }
        }

        public static class Sorting<T>
        {
            public static IQueryable<T> GroupingData(IQueryable<T> data, IEnumerable<string> groupingColumns)
            {
                IOrderedQueryable<T> groupedData = null;

                foreach (string grpCol in groupingColumns.Where(x => !String.IsNullOrEmpty(x)))
                {
                    var col = typeof(T).GetProperty(grpCol, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                    if (col != null)
                    {
                        groupedData = groupedData == null ? data.OrderBy(x => col.GetValue(x, null))
                                                        : groupedData.ThenBy(x => col.GetValue(x, null));
                    }
                }

                return groupedData ?? data;
            }

            public static IQueryable<T> OrderByDynamic(IQueryable<T> source , string orderByProperty, bool desc)
            {
                string command = desc ? "OrderByDescending" : "OrderBy";
                var type = typeof(T);
                var property = type.GetProperty(orderByProperty);
                var parameter = Expression.Parameter(type, "p");
                var properyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExpression = Expression.Lambda(properyAccess, parameter);
                var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));
                return source.Provider.CreateQuery<T>(resultExpression);
            }
            public static IQueryable<T> SortData(IQueryable<T> data, IEnumerable<SortingParams> sortingParams)
            {
                IOrderedQueryable<T> sortedData = null;
                foreach (var sortingParam in sortingParams.Where(x => !String.IsNullOrEmpty(x.ColumnName)))
                {
                    data = OrderByDynamic(data, sortingParam.ColumnName, desc: sortingParam.SortOrder == SortOrders.Desc ? true : false);
                }
                return sortedData ?? data;
            }
        }
    }
}