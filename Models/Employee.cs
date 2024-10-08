using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Employee
    {
        public  int idEmployee { get; set; }

        [Display(Name ="Nombre Completo:"), Required, StringLength(100, MinimumLength = 10)]
        public string name { get; set; }

        [Range(18, 100), Display(Name ="Edad:"), Required, Column(TypeName = "int")]
        public int age { get; set; }

        [Display(Name ="Genero:"), Required]
        public string sex { get; set; }

        [Display(Name = "Puesto de Trabajo:"), Required, StringLength(50, MinimumLength = 5)]
        public string workDescription { get; set; }

        [Display(Name = "Sucursal:")]
        public Branch objBranch { get; set; }
        public DateTime createdDate { get; set; }

    }//End employee class
}//End models namespace
