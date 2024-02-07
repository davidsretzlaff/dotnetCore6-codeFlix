﻿using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix.Catalog.UnitTests.Common
{

    public abstract class BaseFixture
    {
        public Faker Faker { get; set; }

        protected BaseFixture() => Faker = new Faker("pt_BR");

        public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
    }
}
