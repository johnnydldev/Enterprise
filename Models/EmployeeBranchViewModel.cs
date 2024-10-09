using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class EmployeeBranchViewModel
    {
        public List<Employee> employees {  get; set; }
        public List<Branch> branches { get; set; }
        public string employeeBranch {  get; set; }
        public string employee {  get; set; }

    }
}
