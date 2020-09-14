using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplication.Models
{
    public partial class Accounts
    {
        public Accounts()
        {
            Dispositions = new HashSet<Dispositions>();
            Loans = new HashSet<Loans>();
            PermenentOrder = new HashSet<PermenentOrder>();
            Transactions = new HashSet<Transactions>();
        }

        [Key]
        public int AccountId { get; set; }
        [Required]
        [StringLength(50)]
        public string Frequency { get; set; }
        [Column(TypeName = "date")]
        public DateTime Created { get; set; }
        [Column(TypeName = "decimal(13, 2)")]
        public decimal Balance { get; set; }

        [InverseProperty("Account")]
        public virtual ICollection<Dispositions> Dispositions { get; set; }
        [InverseProperty("Account")]
        public virtual ICollection<Loans> Loans { get; set; }
        [InverseProperty("Account")]
        public virtual ICollection<PermenentOrder> PermenentOrder { get; set; }
        [InverseProperty("AccountNavigation")]
        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}
