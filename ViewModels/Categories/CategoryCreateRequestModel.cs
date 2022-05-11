using System;

namespace ViewModels.Categories
{
    public class CategoryCreateRequestModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int Order { get; set; } = 0;
    }
}