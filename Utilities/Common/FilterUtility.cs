using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DynamicExpressions;
using System.Linq.Expressions;

namespace Utilities.Common
{
    public class FilterUtility
    {
        public class FilterParams
        {
            public string ColumnName { get; set; } = string.Empty;
            public string FilterValue { get; set; } = string.Empty;
            public DynamicExpressions.FilterOperator FilterOperator { get; set; } = DynamicExpressions.FilterOperator.Contains;
        }

        public static class Filter<T>
        {
            public static DynamicFilterBuilder<T> FilteredData(IEnumerable<FilterParams> filterParams, DynamicFilterBuilder<T> predicate)
            {
                IEnumerable<string> distinctColumns = filterParams.Where(x => !String.IsNullOrEmpty(x.ColumnName)).Select(x => x.ColumnName).Distinct();

                foreach (string colName in distinctColumns)
                {
                    var filterColumn = typeof(T).GetProperty(colName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                    if (filterColumn != null)
                    {
                        IEnumerable<FilterParams> filterValues = filterParams.Where(x => x.ColumnName.Equals(colName)).Distinct();

                        //if (filterValues.Count() > 1)
                        //{
                        //    IEnumerable<T> sameColData = Enumerable.Empty<T>();

                        //    foreach (var val in filterValues)
                        //    {
                        //        sameColData = sameColData.Concat(FilterData(val.FilterOperator, data, filterColumn, val.FilterValue));
                        //    }

                        //    data = data.Intersect(sameColData);
                        //}
                        //else
                        //{
                        predicate = FilterData(filterValues.FirstOrDefault().FilterOperator, predicate, filterColumn, filterValues.FirstOrDefault().FilterValue);
                        //}
                    }
                }
                return predicate;
            }

            private static DynamicFilterBuilder<T> FilterData(DynamicExpressions.FilterOperator filterOption, DynamicFilterBuilder<T> predicate, PropertyInfo filterColumn, string filterValue)
            {
                predicate = FilterDataStringDataType(filterOption, predicate, filterColumn, filterValue);
                predicate = FilterDataCustomGreaterThan(filterOption, predicate, filterColumn, filterValue);
                predicate = FilterDataCustomLessThan(filterOption, predicate, filterColumn, filterValue);
                predicate = FilterDataCustomEqualThan(filterOption, predicate, filterColumn, filterValue);
                return predicate;
            }

            private static DynamicFilterBuilder<T> FilterDataStringDataType(FilterOperator filterOperator, DynamicFilterBuilder<T> predicate, PropertyInfo filterColumn, string filterValue)
            {
                if (filterOperator == FilterOperator.StartsWith)
                {
                    predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.StartsWith, filterValue.ToString()));
                }
                else if (filterOperator == FilterOperator.EndsWith)
                {
                    predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.EndsWith, filterValue.ToString()));
                }
                else if (filterOperator == FilterOperator.Contains)
                {
                    predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.Contains, filterValue.ToString()));
                }
                else if (filterOperator == FilterOperator.NotContains)
                {
                    predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.NotContains, filterValue.ToString()));
                }
                else if (filterOperator == FilterOperator.IsEmpty)
                {
                    predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.IsEmpty, string.Empty));
                }
                else if (filterOperator == FilterOperator.IsNotEmpty)
                {
                    predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.IsNotEmpty, string.Empty));
                }

                return predicate;
            }

            private static DynamicFilterBuilder<T> FilterDataCustomGreaterThan(FilterOperator filterOption, DynamicFilterBuilder<T> predicate, PropertyInfo filterColumn, string filterValue)
            {
                int outValue;
                DateTime dateValue;
                if (filterOption == FilterOperator.GreaterThan)
                {
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.GreaterThan, outValue));
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.GreaterThan, dateValue));
                    }
                }
                else if (filterOption == FilterOperator.GreaterThanOrEqual)
                {
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.GreaterThanOrEqual, outValue));
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.GreaterThanOrEqual, dateValue));
                    }
                }

                return predicate;
            }

            private static DynamicFilterBuilder<T> FilterDataCustomLessThan(FilterOperator filterOption, DynamicFilterBuilder<T> predicate, PropertyInfo filterColumn, string filterValue)
            {
                int outValue;
                DateTime dateValue;
                if (filterOption == FilterOperator.LessThan)
                {
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.LessThan, outValue));
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.LessThan, dateValue));
                    }
                }
                else if (filterOption == FilterOperator.LessThanOrEqual)
                {
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.LessThanOrEqual, outValue));
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.LessThanOrEqual, dateValue));
                    }
                }
                return predicate;
            }

            private static DynamicFilterBuilder<T> FilterDataCustomEqualThan(FilterOperator filterOption, DynamicFilterBuilder<T> predicate, PropertyInfo filterColumn, string filterValue)
            {
                int outValue;
                DateTime dateValue;
                if (filterOption == FilterOperator.Equals)
                {
                    if (filterValue == string.Empty)
                    {
                        predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.Equals, string.Empty));
                    }
                    else
                    {
                        if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                        {
                            predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.Equals, outValue));
                        }
                        else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                        {
                            predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.Equals, dateValue));
                        }
                        else
                        {
                            predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.Equals, filterValue));
                        }
                    }
                }
                else if (filterOption == FilterOperator.DoesntEqual)
                {
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.DoesntEqual, outValue));
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.DoesntEqual, dateValue));
                    }
                    else
                    {
                        predicate = predicate.And(b => b.And(filterColumn.Name, FilterOperator.DoesntEqual, filterValue));
                    }
                }

                return predicate;
            }
        }
    }
}