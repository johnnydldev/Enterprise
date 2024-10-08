using DAOControllers.ManagerControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using System.Dynamic;
using System.Diagnostics;

namespace Enterprise.Controllers
{
    
    public class EmployeeController : Controller
    {

        private readonly ILogger<EmployeeController> _logger;
        private readonly IGenericRepository<Employee> _employeeRepository;
        private readonly IGenericRepository<Branch> _branchRepository;

        public EmployeeController(ILogger<EmployeeController> logger,
            IGenericRepository<Employee> employee,
            IGenericRepository<Branch> branchRepository)
        {
            _logger = logger;
            _employeeRepository = employee;
            _branchRepository = branchRepository;
        }
        public async Task<IActionResult> Index()
        {
            List<Employee> _listEmployee = await _employeeRepository.getAll();

            if (_listEmployee.Any())
            {
                Console.WriteLine(Ok(_listEmployee));
            }

            return View("Index", _listEmployee);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Branch branch = new Branch();
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


            return View("Delete", employee);
        }



    }//End employee controller
}//End namespace enterprise
