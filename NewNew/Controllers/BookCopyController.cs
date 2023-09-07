using NewNew.Data;
using NewNew.Models.Domain;
using NewNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewNew.Controllers
{
	public class BookCopyController : Controller
	{

		private readonly MVCDemoContext mvcDemoContext;

		public BookCopyController(MVCDemoContext mvcDemoDbContext)
		{
			this.mvcDemoContext = mvcDemoDbContext;
		}

        public async Task<IActionResult> Index()
        {
            var bCopy = await mvcDemoContext.BCopy
                        .Join(mvcDemoContext.publisher,
                              b => b.PublisherId,
                              c => c.PublisherId,
                              (b, c) => new { BookCopy = b, Publisher = c })
                        .Join(mvcDemoContext.Books,
                              bc => bc.BookCopy.bookId,
                              book => book.bookId,
                              (bc, book) => new BookCopy
                              {
                                  bookCopyId = bc.BookCopy.bookCopyId,
                                  yearOfPublished = bc.BookCopy.yearOfPublished,
                                  publisherName = bc.Publisher.publisherName,
                                  bookTitle = book.bookTitle
                              })
                        .ToListAsync();

            return View(bCopy);
        }
    

		[HttpGet]

		public IActionResult Add()
		{
			var books = mvcDemoContext.Books
				.Select(c => new SelectListItem
				{
					Value = c.bookId.ToString(),
					Text = c.bookTitle
				})
				.ToList();

			var publishers = mvcDemoContext.publisher
				.Select(c => new SelectListItem
				{
					Value = c.PublisherId.ToString(),
					Text = c.publisherName
				})
				.ToList();

			var addBookCopyViewModel = new AddBookCopyViewModel
			{
				Books = books,
				Publishers = publishers
			};

			return View(addBookCopyViewModel);
		}
        [HttpPost]
        public async Task<IActionResult> Add(AddBookCopyViewModel addBookCopyViewViewModel)
        {
            var bookCopy = new BookCopy()
            {
                bookCopyId=addBookCopyViewViewModel.bookCopyId,
                yearOfPublished = addBookCopyViewViewModel.yearOfPublished,
                PublisherId = addBookCopyViewViewModel.PublisherId,
                bookId = addBookCopyViewViewModel.bookId
            };

            await mvcDemoContext.BCopy.AddAsync(bookCopy);
            await mvcDemoContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> View(int id)
        {
            var copy = await mvcDemoContext.BCopy.FindAsync(id);

            if (copy == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new UpdateBookCopyViewModel()
            {
                bookCopyId = copy.bookCopyId,
				yearOfPublished=copy.yearOfPublished,
				PublisherId=copy.PublisherId,
				bookId=copy.bookId
               
            };

            return await Task.Run(() => View("View", viewModel));
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateBookCopyViewModel model)
        {
            var copy = await mvcDemoContext.BCopy.FindAsync(model.bookCopyId);

            if (copy == null)
            {
                return RedirectToAction(nameof(Index));
            }

            copy.yearOfPublished= model.yearOfPublished;
			copy.PublisherId = model.PublisherId ;
            copy.bookId = model.bookId;
            




            mvcDemoContext.BCopy.Update(copy);
            await mvcDemoContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
		public async Task<IActionResult> Delete(UpdateBookCopyViewModel model)
		{
			var bookCopy = await mvcDemoContext.BCopy.FindAsync(model.bookCopyId);
			if (bookCopy != null)
			{
				mvcDemoContext.BCopy.Remove(bookCopy);
				await mvcDemoContext.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}
	}
}

	

