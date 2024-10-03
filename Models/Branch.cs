using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Branch
    {
        [Display(Name = "Id")]
        public int idBranch { get; set; }

        [Display(Name ="Descripción"), Required, StringLength(50)]
        public string description { get; set; }
        public DateTime createdDate {  get; set; }
        
    }//End branch class
}//End models namespace
