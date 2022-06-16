﻿using System;

namespace CourseWork.Infrastructure.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
