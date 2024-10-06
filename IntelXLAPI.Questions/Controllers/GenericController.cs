using IntelXLDataAccess.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IntelXLAdmin.Api.Controllers
{
    public class GenericController<T> : ControllerBase where T : class
    {       
        private readonly IntelxlContext _context;

        public GenericController(IntelxlContext context)
        {           
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }


        [HttpPost]
        public async Task<IActionResult> Create(T entity)
        {
            try
            {
                var entry = await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return Ok(entry.Entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task Update(int id, T entity)
        {
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }
       
        [HttpPut]
        public async Task<IActionResult> UpdateRange(IEnumerable<T> entities)
        {
            try
            {
                _context.UpdateRange(entities);
               var result= await _context.SaveChangesAsync();
                return Ok(result);
            }            
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
