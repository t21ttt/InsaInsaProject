using NewNew.Data;
using NewNew.Models.Domain;
using NewNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;



namespace NewNew.Controllers
{
    public class BookCategoryController : Controller
    {
        private readonly MVCDemoContext mvcDemoContext;

        public BookCategoryController(MVCDemoContext mvcDemoDbContext)
        {
            this.mvcDemoContext = mvcDemoDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var bookCategories = await mvcDemoContext.bookCategory.ToListAsync();
            return View(bookCategories);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBookCategoryViewModel addBookCategoryViewModel)
        {
            // Check if the category already exists
            bool categoryExists = await mvcDemoContext.bookCategory.AnyAsync(c => c.categoryName == addBookCategoryViewModel.categoryName);

            if (categoryExists)
            {
                ViewBag.Message = "The category is already recorded.";
            }
            else
            {
                var bookCategory = new BookCategory()
                {
                    categoryName = addBookCategoryViewModel.categoryName
                };

                await mvcDemoContext.bookCategory.AddAsync(bookCategory);
                await mvcDemoContext.SaveChangesAsync();

                ViewBag.Message = "The new category has been successfully added.";
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> View(int id)
        {
            var bcategory = await mvcDemoContext.bookCategory.FindAsync(id);

            if ( bcategory== null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new UpdateBookCategoryViewModel()
            {
               bookCategroryId = bcategory.bookCategroryId,
                categoryName = bcategory.categoryName,
               

            };

            return await Task.Run(() => View("View", viewModel));
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateBookCategoryViewModel model)
        {
            var bcategory= await mvcDemoContext.bookCategory.FindAsync(model.bookCategroryId);

            if (bcategory == null)
            {
                return RedirectToAction(nameof(Index));
            }

            bcategory.categoryName = model.categoryName;
           



            mvcDemoContext.bookCategory.Update(bcategory);
            await mvcDemoContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateBookCategoryViewModel model)
        {

            var bookCategory = await mvcDemoContext.bookCategory.FindAsync(model.bookCategroryId);
            if (bookCategory != null)
            {
                mvcDemoContext.bookCategory.Remove(bookCategory);
                 await mvcDemoContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
