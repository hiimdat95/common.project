using System;
using System.Collections.Generic;

namespace ViewModels.Categories
{
    public class CategoryHierarchicalSearchRequest
    {
        public List<string> LstCategoryCode { get; set; }
        public bool IsGetValueAll { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsGetMoreDepthChildren { get; set; } = true;
    }
}