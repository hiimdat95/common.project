﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utilities.Common
{
    public class FilterUtility
    {
        public enum FilterOptions
        {
            StartsWith = 1,
            EndsWith = 2,
            Contains = 3,
            DoesNotContain = 4,
            IsEmpty = 5,
            IsNotEmpty = 6,
            IsGreaterThan = 7,
            IsGreaterThanOrEqualTo = 8,
            IsLessThan = 9,
            IsLessThanOrEqualTo = 10,
            IsEqualTo = 11,
            IsNotEqualTo = 12
        }

        public class FilterParams
        {
            public string ColumnName { get; set; } = string.Empty;
            public string FilterValue { get; set; } = string.Empty;
            public FilterOptions FilterOption { get; set; } = FilterOptions.Contains;
        }

        public static class Filter<T>
        {
            public static IEnumerable<T> FilteredData(IEnumerable<FilterParams> filterParams, IEnumerable<T> data)
            {
                IEnumerable<string> distinctColumns = filterParams.Where(x => !String.IsNullOrEmpty(x.ColumnName)).Select(x => x.ColumnName).Distinct();

                foreach (string colName in distinctColumns)
                {
                    var filterColumn = typeof(T).GetProperty(colName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                    if (filterColumn != null)
                    {
                        IEnumerable<FilterParams> filterValues = filterParams.Where(x => x.ColumnName.Equals(colName)).Distinct();

                        if (filterValues.Count() > 1)
                        {
                            IEnumerable<T> sameColData = Enumerable.Empty<T>();

                            foreach (var val in filterValues)
                            {
                                sameColData = sameColData.Concat(FilterData(val.FilterOption, data, filterColumn, val.FilterValue));
                            }

                            data = data.Intersect(sameColData);
                        }
                        else
                        {
                            data = FilterData(filterValues.FirstOrDefault().FilterOption, data, filterColumn, filterValues.FirstOrDefault().FilterValue);
                        }
                    }
                }
                return data;
            }

            private static IEnumerable<T> FilterData(FilterOptions filterOption, IEnumerable<T> data, PropertyInfo filterColumn, string filterValue)
            {
                data = FilterDataStringDataType(filterOption, data, filterColumn, filterValue);
                data = FilterDataCustomGreaterThan(filterOption, data, filterColumn, filterValue);
                data = FilterDataCustomLessThan(filterOption, data, filterColumn, filterValue);
                data = FilterDataCustomEqualThan(filterOption, data, filterColumn, filterValue);
                return data;
            }

            private static IEnumerable<T> FilterDataStringDataType(FilterOptions filterOption, IEnumerable<T> data, PropertyInfo filterColumn, string filterValue)
            {
                if (filterOption == FilterOptions.StartsWith)
                {
                    data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower().StartsWith(filterValue.ToString().ToLower())).ToList();
                }
                else if (filterOption == FilterOptions.EndsWith)
                {
                    data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower().EndsWith(filterValue.ToString().ToLower())).ToList();
                }
                else if (filterOption == FilterOptions.Contains)
                {
                    data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower().Contains(filterValue.ToString().ToLower())).ToList();
                }
                else if (filterOption == FilterOptions.DoesNotContain)
                {
                    data = data.Where(x => filterColumn.GetValue(x, null) == null ||
                                       (filterColumn.GetValue(x, null) != null && !filterColumn.GetValue(x, null).ToString().ToLower().Contains(filterValue.ToString().ToLower()))).ToList();
                }
                else if (filterOption == FilterOptions.IsEmpty)
                {
                    data = data.Where(x => filterColumn.GetValue(x, null) == null ||
                                         (filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString() == string.Empty)).ToList();
                }
                else if (filterOption == FilterOptions.IsNotEmpty)
                {
                    data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString() != string.Empty).ToList();
                }
                return data;
            }

            private static IEnumerable<T> FilterDataCustomGreaterThan(FilterOptions filterOption, IEnumerable<T> data, PropertyInfo filterColumn, string filterValue)
            {
                int outValue;
                DateTime dateValue;
                if (filterOption == FilterOptions.IsGreaterThan)
                {
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) > outValue).ToList();
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) > dateValue).ToList();
                    }
                }
                else if (filterOption == FilterOptions.IsGreaterThanOrEqualTo)
                {
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) >= outValue).ToList();
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) >= dateValue).ToList();
                    }
                }

                return data;
            }

            private static IEnumerable<T> FilterDataCustomLessThan(FilterOptions filterOption, IEnumerable<T> data, PropertyInfo filterColumn, string filterValue)
            {
                int outValue;
                DateTime dateValue;
                if (filterOption == FilterOptions.IsLessThan)
                {
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) < outValue).ToList();
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) < dateValue).ToList();
                    }
                }
                else if (filterOption == FilterOptions.IsLessThanOrEqualTo)
                {
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) <= outValue).ToList();
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) <= dateValue).ToList();
                    }
                }
                return data;
            }

            private static IEnumerable<T> FilterDataCustomEqualThan(FilterOptions filterOption, IEnumerable<T> data, PropertyInfo filterColumn, string filterValue)
            {
                int outValue;
                DateTime dateValue;
                if (filterOption == FilterOptions.IsEqualTo)
                {
                    if (filterValue == string.Empty)
                    {
                        data = data.Where(x => filterColumn.GetValue(x, null) == null
                                        || (filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower() == string.Empty)).ToList();
                    }
                    else
                    {
                        if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                        {
                            data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) == outValue).ToList();
                        }
                        else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                        {
                            data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) == dateValue).ToList();
                        }
                        else
                        {
                            data = data.Where(x => filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower() == filterValue.ToLower()).ToList();
                        }
                    }
                }
                else if (filterOption == FilterOptions.IsNotEqualTo)
                {
                    if ((filterColumn.PropertyType == typeof(Int32) || filterColumn.PropertyType == typeof(Nullable<Int32>)) && Int32.TryParse(filterValue, out outValue))
                    {
                        data = data.Where(x => Convert.ToInt32(filterColumn.GetValue(x, null)) != outValue).ToList();
                    }
                    else if ((filterColumn.PropertyType == typeof(Nullable<DateTime>)) && DateTime.TryParse(filterValue, out dateValue))
                    {
                        data = data.Where(x => Convert.ToDateTime(filterColumn.GetValue(x, null)) != dateValue).ToList();
                    }
                    else
                    {
                        data = data.Where(x => filterColumn.GetValue(x, null) == null ||
                                         (filterColumn.GetValue(x, null) != null && filterColumn.GetValue(x, null).ToString().ToLower() != filterValue.ToLower())).ToList();
                    }
                }

                return data;
            }
        }
    }
}