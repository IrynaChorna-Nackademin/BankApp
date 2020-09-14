using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplication.Models
{
    public partial class Dispositions
    {
        public Dispositions()
        {
            Cards = new HashSet<Cards>();
        }

        [Key]
        public int DispositionId { get; set; }
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty(nameof(Accounts.Dispositions))]
        public virtual Accounts Account { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customers.Dispositions))]
        public virtual Customers Customer { get; set; }
        [InverseProperty("Disposition")]
        public virtual ICollection<Cards> Cards { get; set; }
    }
}
