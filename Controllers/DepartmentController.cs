using DepartmentAPI.Data;
using DepartmentAPI.Models;
using DepartmentAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DepartmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _repository;
        private readonly ILogger<DepartmentController> _logger;
        private readonly AppDbContext _context;

        // Single constructor with dependency injection for both repository and logger
        public DepartmentController(IDepartmentRepository repository, ILogger<DepartmentController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Fetches all departments.
        /// </summary>
        /// <returns>List of departments.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all departments");
            var departments = await _repository.GetAllAsync();  // Assuming GetAllAsync exists
            return Ok(departments);
        }

        /// <summary>
        /// Fetches department by ID.
        /// </summary>
        /// <returns>Department by ID.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _repository.GetByIdAsync(id);  // Assuming GetByIdAsync exists
            if (department == null)
            {
                _logger.LogWarning("Department with ID {id} not found", id);
                return NotFound();
            }
            return Ok(department);
        }

        /// <summary>
        /// Add a new department.
        /// </summary>
        /// <param name="department">Department object to add.</param>
        /// <returns>Created department.</returns>
        [HttpPost]
        public async Task<IActionResult> Add(Department department)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid department model.");
                return BadRequest(ModelState);
            }

            await _repository.AddAsync(department);  // Assuming AddAsync exists
            _logger.LogInformation("Department with ID {id} created.", department.DeptID);
            return CreatedAtAction(nameof(GetById), new { id = department.DeptID }, department);
        }

        /// <summary>
        /// Update an existing department.
        /// </summary>
        /// <param name="department">Updated department object.</param>
        /// <returns>No content on success.</returns>
        [HttpPut]
        public async Task<IActionResult> Update(Department department)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid department model.");
                return BadRequest(ModelState);
            }

            var existingDepartment = await _repository.GetByIdAsync(department.DeptID);  // Ensure the department exists
            if (existingDepartment == null)
            {
                _logger.LogWarning("Department with ID {id} not found to update.", department.DeptID);
                return NotFound();
            }

            await _repository.UpdateAsync(department);  // Assuming UpdateAsync exists
            _logger.LogInformation("Department with ID {id} updated.", department.DeptID);
            return NoContent();
        }

        /// <summary>
        /// Delete a department by ID.
        /// </summary>
        /// <param name="id">Department ID to delete.</param>
        /// <returns>No content on success.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _repository.GetByIdAsync(id);
            if (department == null)
            {
                _logger.LogWarning("Department with ID {id} not found to delete.", id);
                return NotFound();
            }

            await _repository.DeleteAsync(id);  // Assuming DeleteAsync exists
            _logger.LogInformation("Department with ID {id} deleted.", id);
            return NoContent();
        }
    }
}
