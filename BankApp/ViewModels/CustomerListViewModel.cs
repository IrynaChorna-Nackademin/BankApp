using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.ViewModels
{
    
    public class CustomerListViewModel
    {
        public string SearchWord { get; set; }
        public PagingViewModel PagingViewModel { get; set; } = new PagingViewModel();
        
        public class CustomersViewModel
        {
           
            public int CustomerId { get; set; }
            public string Givenname { get; set; }
            public string Surname { get; set; }
            public string Streetaddress { get; set; }
            public string City { get; set; }
            public string NationalId { get; set; }
          
        }
        public List<CustomersViewModel> Customers { get; set; } = new List<CustomersViewModel>();
    }
}
