﻿using SimplCommerce.Infrastructure.Models;
using SimplCommerce.Module.Core.Models;

namespace SimplCommerce.Module.Cms.Models
{
    public class MenuItem : EntityBase
    {
        public long? ParentId { get; set; }

        public MenuItem Parent { get; set; }

        public long MenuId { get; set; }

        public Menu Menu { get; set; }

        public long? EntityId { get; set; }

        public Entity Entity { get; set; }

        public string CustomLink { get; set; }

        public string Name { get; set; }
    }
}
