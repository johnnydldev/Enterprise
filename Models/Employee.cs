using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Employee
    {
        public  int idEmployee { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public string sex { get; set; }
        public string workDescription { get; set; }
        public Branch objBranch { get; set; }
        public DateTime createdDate { get; set; }

    }//End employee class
}//End models namespace
