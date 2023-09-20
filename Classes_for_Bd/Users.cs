using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ya_.Classes_for_Bd
{
    public  class Users
    {
        public int Id { get; set; }
        public int Role_id { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
    }
}
