﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Backend.Data.Addresses
{
    public class Street
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
    }
}