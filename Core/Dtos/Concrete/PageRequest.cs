﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.Concrete;
public class PageRequest
{
    public string Filter { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

