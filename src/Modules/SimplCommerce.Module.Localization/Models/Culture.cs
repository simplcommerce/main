﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SimplCommerce.Infrastructure.Models;

namespace SimplCommerce.Module.Localization.Models
{
    public class Culture : EntityBaseWithTypedId<string>
    {
        public Culture(string id)
        {
            Id = id;
        }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public IList<Resource> Resources { get; set; }
    }
}
