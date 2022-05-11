using System;
using System.Collections.Generic;
using ViewModels.CategoryItems;

namespace ViewModels.Categories
{
    public class CategoryResponseModel
    {
        public CategoryResponseModel()
        {
            CategoryItems = new List<CategoryItemResponseModel>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Order { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
        public List<CategoryItemResponseModel> CategoryItems { get; set; }
    }
}