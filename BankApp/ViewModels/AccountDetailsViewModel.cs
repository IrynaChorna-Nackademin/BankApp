using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.ViewModels
{
    public class AccountDetailsViewModel
    {
        public AccountDetailsViewModel()
        {
            Transactions = new List<TransactionsViewModel>();
        }

        [Key]
        public int AccountId { get; set; }


        [Column(TypeName = "decimal(13, 2)")]
        public decimal AccountBalance { get; set; }

        public int AccountIdFrom { get; set; }

        [Column(TypeName = "decimal(13, 2)")]
        public decimal AccountBalanceFrom { get; set; }
        public List<TransactionsViewModel> Transactions { get; set; }
        public TransactionsViewModel Transaction { get; set; } = new TransactionsViewModel();

        public string SearchAccount { get; set; } = null;
    }

    public class TransactionsViewModel 
    {
        [Key]
        public int TransactionId { get; set; }
        public int AccountId { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        //[Required]
        [StringLength(50)]
        public string Type { get; set; }
        
        [BindProperty]
        [StringLength(50)]
        public string Operation { get; set; }

        [Column(TypeName = "decimal(13, 2)")]
        [Range(0d, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(13, 2)")]
        public decimal Balance { get; set; }
        
        [StringLength(50)]
        public string Symbol { get; set; }
        
        [StringLength(50)]
        public string Bank { get; set; }
        
        [StringLength(50)]
        public string Account { get; set; }
    }
}
