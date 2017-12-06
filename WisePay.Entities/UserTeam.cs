﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WisePay.Entities
{
    public class UserTeam
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}