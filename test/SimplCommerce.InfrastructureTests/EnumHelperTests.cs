﻿using SimplCommerce.Infrastructure;
using Xunit;

namespace SimplCommerce.InfrastructureTests
{
    public class EnumHelperTests
    {
        enum Importance
        {
            None,
            Trivial,
            Regular,
            Important,
            Critical
        };

        [Fact]
        public void GetDisplayNameImportanceEnumCriticalShouldReturnsCritical()
        {
            var foo = Importance.Critical.GetDisplayName();
            Assert.Equal("Critical", foo);
        }
    }
}
