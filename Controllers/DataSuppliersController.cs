using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASJ.Models;

namespace ASJ.Controllers
{
    public class DataSuppliersController : Controller
    {
        private readonly ASJDbContext _context;

        public DataSuppliersController(ASJDbContext context)
        {
            _context = context;
        }

        // GET: DataSuppliers
        public async Task<IActionResult> Index()
        {
            return View(await _context.DataSuppliers.ToListAsync());
        }

        // GET: DataSuppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataSupplier = await _context.DataSuppliers
                .SingleOrDefaultAsync(m => m.DataSupplierId == id);
            if (dataSupplier == null)
            {
                return NotFound();
            }

            return View(dataSupplier);
        }

        // GET: DataSuppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DataSuppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DataSupplierId,Name,Title,Address,City,State,Zip,Phone,Fax,email")] DataSupplier dataSupplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dataSupplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dataSupplier);
        }

        // GET: DataSuppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }

            var dataSupplier = await _context.DataSuppliers.SingleOrDefaultAsync(m => m.DataSupplierId == id);
            if (dataSupplier == null)
            {
                return NotFound();
            }
            return View(dataSupplier);
        }

        // POST: DataSuppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DataSupplierId,Name,Title,Address,City,State,Zip,Phone,Fax,email,Organization,Instrument")] DataSupplier dataSupplier)
        {
            if (id != dataSupplier.DataSupplierId)
            {
                _context.Add(dataSupplier);
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dataSupplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataSupplierExists(dataSupplier.DataSupplierId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
            }
            return View(dataSupplier);
        }

        // GET: DataSuppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataSupplier = await _context.DataSuppliers
                .SingleOrDefaultAsync(m => m.DataSupplierId == id);
            if (dataSupplier == null)
            {
                return NotFound();
            }

            return View(dataSupplier);
        }

        // POST: DataSuppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataSupplier = await _context.DataSuppliers.SingleOrDefaultAsync(m => m.DataSupplierId == id);
            _context.DataSuppliers.Remove(dataSupplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DataSupplierExists(int id)
        {
            return _context.DataSuppliers.Any(e => e.DataSupplierId == id);
        }
    }
}
