﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class FAQ
    {
        public FAQ()
        {

        }
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Order { get; set; }
    }
}
