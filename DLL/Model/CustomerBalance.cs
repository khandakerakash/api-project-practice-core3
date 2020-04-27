﻿using System.ComponentModel.DataAnnotations;

namespace DLL.Model
{
    public class CustomerBalance
    {
        public long CustomerBalanceId { get; set; }
        public decimal Balance { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}