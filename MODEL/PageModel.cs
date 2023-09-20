using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ya_.MODEL
{
    public class PageModel
    {
        public int CustomerCount { get; set; }

        public string ProductStatus { get; set; }  
        
        public DateTime OrderDate {  get; set; }

        public decimal TransactionValue {  get; set; }

        public TimeSpan ShipmentDelivery {  get; set; }

        public bool Location {  get; set; }
    }
}
