using DAOControllers.ManagerControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using System.Dynamic;
using System.Diagnostics;

namespace Enterprise.Controllers
{
    
    public class EmployeeController(ILogger<EmployeeController> logger,
            IGenericRepository<Employee> employee,
            IGenericRepository<Branch> branch) : Controller
    {

        private readonly ILogger<EmployeeController> _logger = logger;
        private readonly IGenericRepository<Employee> _employeeRepository = employee;
        private readonly IGenericRepository<Branch> _branchRepository = branch;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Employee> _listEmployee = await _employeeRepository.getAll(); ;
            List<Branch> _listBranch = await _branchRepository.getAll();

   
            if (_listEmployee.Count > 0)
            {
                Console.WriteLine(Ok(_listEmployee));
            }

            EmployeeBranchViewModel model = new()
            {
                employees = _listEmployee,
                branches = _listBranch
            };

            Console.WriteLine(_listEmployee.Count);

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string employeeBranch, string employee)
        {
            List<Employee> _listEmployee = [];
            List<Branch> _listBranch = await _branchRepository.getAll();

            if (employee == null && employeeBranch == null)
            {
                _listEmployee = await _employeeRepository.getAll();
            }

            if (employeeBranch != null)
            {
                int idBranch = Convert.ToInt32(employeeBranch);
                _listEmployee = await _employeeRepository.allMatchedBy(idBranch);
            }

            if (employee != null)
            {
                _listEmployee = await _employeeRepository.allMatchedWith(employee);
            }

            if (employee != null && employeeBranch != null)
            {
                int idBranch = Convert.ToInt32(employeeBranch);
                _listEmployee = await _employeeRepository.allMatches(idBranch, employee);
            }

            if (_listEmployee.Count > 0)
            {
                Console.WriteLine(Ok(_listEmployee));
            }
            else
            {
                Console.WriteLine(NotFound(_listEmployee));
            }

            EmployeeBranchViewModel model = new()
            {
                employees = _listEmployee,
                branches = _listBranch
            };

            return View("Index", model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Branch branch = new();
            ViewBag.listBranch = new SelectList( await _branchRepository.getAll(),"idBranch", "description", branch);

            return View("Create");
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost([Bind("name", "age", "sex", "workDescription", "objBranch")] Employee employee)
        {
            try
            {

                int id = await _employeeRepository.getMaxId();
                int idBranch = Convert.ToInt32(Request.Form["objBranch"]);
                Branch branch = await _branchRepository.getById(idBranch);

                if (id != 0)
                {
                    employee.idEmployee = id;
                    employee.objBranch = branch;
                }

                if (ModelState.IsValid)
                {
                    int result = await _employeeRepository.create(employee);

                    if (result != 0)
                    {
                        Console.WriteLine(Ok(employee));
                        return RedirectToAction(nameof(Index));
                    }
                    Console.WriteLine(BadRequest(employee));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            
            ViewBag.listBranch = new SelectList(await _branchRepository.getAll(), "idBranch", "description", employee.objBranch);

            return View("Create",employee);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            Employee employee = await _employeeRepository.getById(id);
            List<Branch> listBranch = await _branchRepository.getAll();
            ViewBag.listBranch = new SelectList(listBranch, "idBranch", "description", employee.objBranch.description);

            if (employee == null)
            {
                return View("~/Views/NotFound/NotFound.cshtml");
            }

            return View("Edit", employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("idEmployee","name", "age", "sex", "workDescription", "objBranch")] Employee employee)
        {
            if (id != employee.idEmployee)
            {
                return View("~/Views/NotFound/NotFound.cshtml");
            }
            
            if (ModelState.IsValid)
            {
                int idBranch = Convert.ToInt32(Request.Form["objBranch"]);
                Branch branch = await _branchRepository.getById(idBranch);
                employee.objBranch = branch;

                try
                {
                    bool result = await _employeeRepository.edit(employee);

                    if (result)
                    {
                        Console.WriteLine(Ok(employee));
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Console.WriteLine(BadRequest(employee));
                        return View("~/Views/NotFound/NotFound.cshtml");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                    return View("~/Views/NotFound/NotFound.cshtml");
                }
            }

            return View("Edit", employee);
        }

        public async Task<IActionResult> Details(int id)
        {
            Employee employee = await _employeeRepository.getById(id);
            if (employee == null)
            {
                return View("~/Views/NotFound/NotFound.cshtml");
            }

            return View("Details", employee);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Employee employee = await _employeeRepository.getById(id);

            if (employee == null)
            {
                return View("~/Views/NotFound/NotFound.cshtml");
            }

            return View("Delete", employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idEmployee)
        {

            Employee employee = await _employeeRepository.getById(idEmployee);
            if (employee == null)
            {
                return View("~/Views/NotFound/NotFound.cshtml");
            }
            else
            {
                try
                {
                    bool response = await _employeeRepository.delete(idEmployee);

                    if (response)
                    {
                        Console.WriteLine(Ok(employee));
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Console.WriteLine(BadRequest(employee));
                        return View("~/Views/NotFound/NotFound.cshtml");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                    return View("~/Views/NotFound/NotFound.cshtml");
                }
            }

        }


    }//End employee controller
}//End namespace enterprise
