using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplication.Models
{
    public partial class PermenentOrder
    {
        [Key]
        public int OrderId { get; set; }
        public int AccountId { get; set; }
        [Required]
        [StringLength(50)]
        public string BankTo { get; set; }
        [Required]
        [StringLength(50)]
        public string AccountTo { get; set; }
        [Column(TypeName = "decimal(13, 2)")]
        public decimal? Amount { get; set; }
        [Required]
        [StringLength(50)]
        public string Symbol { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty(nameof(Accounts.PermenentOrder))]
        public virtual Accounts Account { get; set; }
    }
}
