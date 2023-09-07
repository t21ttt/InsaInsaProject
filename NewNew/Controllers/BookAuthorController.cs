using NewNew.Data;
using NewNew.Models.Domain;
using NewNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace NewNew.Controllers
{
    public class BookAuthorController : Controller
    {
        private readonly MVCDemoContext mvcDemoContext;

        public BookAuthorController(MVCDemoContext mvcDemoDbContext)
        {
            this.mvcDemoContext = mvcDemoDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var bookAuthors = await mvcDemoContext.Authors.ToListAsync();
            return View(bookAuthors);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBookAuthorViewModel addBookAuthorViewModel)
        {
            var bookAuthor = new BookAuthor()
            {
                bookId = addBookAuthorViewModel.bookId,
                authorId = addBookAuthorViewModel.authorId
            };

            await mvcDemoContext.Authors.AddAsync(bookAuthor);
            await mvcDemoContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> View(Guid id)
        {
            var bookAuthor = await mvcDemoContext.Authors.FindAsync(id);

            if (bookAuthor == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new UpdateBookAuthorViewModel()
            {
                authorId = bookAuthor.authorId,
                bookId = bookAuthor.bookId
            };

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateBookCategoryViewModel model)
        {
            var BCategory = await mvcDemoContext.bookCategory.FindAsync(model.bookCategroryId);

            if (BCategory != null)
            {
                BCategory.categoryName = model.categoryName;

                await mvcDemoContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Add");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateBookCategoryViewModel model)
        {
            var BCategory = await mvcDemoContext.bookCategory.FindAsync(model.bookCategroryId);

            if (BCategory != null)
            {
                mvcDemoContext.bookCategory.Remove(BCategory);
                await mvcDemoContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
