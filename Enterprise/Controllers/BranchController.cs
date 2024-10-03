using Microsoft.AspNetCore.Mvc;
using System.Data;
using Enterprise.Data;
using System.Configuration;
using System.Diagnostics;
using DAOControllers.ManagerControllers;
using Models;

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
    
    
    }//End branch controller class
}//End namespace enterprise controllers
