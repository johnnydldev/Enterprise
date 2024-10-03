using Microsoft.AspNetCore.Mvc;
using System.Data;
using Enterprise.Data;
using System.Configuration;
using System.Diagnostics;
using DAOControllers.ManagerControllers;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.Controllers
{
    public class BranchController : Controller
    {

        private readonly ILogger<BranchController> _logger;
        private readonly IGenericRepository<Branch> _branchRepository;

        public BranchController(ILogger<BranchController> logger,
            IGenericRepository<Branch> branch)
        {
            _logger = logger;
            _branchRepository = branch;
        }

        public async Task<IActionResult> Index()
        {
            List<Branch> _listBranch = await _branchRepository.getAll();
            
            if (_listBranch.Any())
            {
               Console.WriteLine(Ok(_listBranch));
            }

            return View("Index", _listBranch);
        }

        [HttpGet("Branch/showBranches")]
        public async Task<IActionResult> showBranches()
        {
            List<Branch> _listBranch = await _branchRepository.getAll();
            return Ok(_listBranch);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("description")] Branch branch)
        {
            int id = await _branchRepository.getMaxId();

            if (id != 0)
            {
                branch.idBranch = id; 
            }

            if (ModelState.IsValid)
            {
                int result = await _branchRepository.create(branch);

                if (result != 0)
                {
                    Console.WriteLine(Ok(branch));
                    return RedirectToAction(nameof(Index));
                }
                Console.WriteLine(BadRequest(branch));
            }

            return View(branch);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            Branch branch = await _branchRepository.getById(id);
            if (branch == null)
            {
                return NotFound();
            }

            return View("Edit", branch);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("idBranch","description")] Branch branch)
        {
            if (id != branch.idBranch)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool response = await _branchRepository.edit(branch);

                    if (response)
                    {
                        Console.WriteLine(Ok(branch));
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Console.WriteLine(BadRequest(branch));
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                    return NotFound();
                }
            }

            return View("Edit", branch);
        }

        public async Task<IActionResult> Details(int id)
        {
            Branch branch = await _branchRepository.getById(id);
            if (branch == null)
            {
                return NotFound();
            }

            return View("Details", branch);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Branch branch = await _branchRepository.getById(id);
            if (branch == null)
            {
                return NotFound();
            }

            return View("Delete", branch);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idBranch)
        {
            Branch branch = await _branchRepository.getById(idBranch);
            if (branch == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    bool response = await _branchRepository.delete(idBranch);

                    if (response)
                    {
                        Console.WriteLine(Ok(branch));
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Console.WriteLine(BadRequest(branch));
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                    return NotFound();
                }
            }

            

            return View("Delete", branch);
        }


    }//End branch controller class
}//End namespace enterprise controllers
