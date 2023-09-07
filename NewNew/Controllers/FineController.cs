using NewNew.Data;
using NewNew.Models.Domain;
using NewNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewNew.Controllers
{
    public class FineController : Controller
    {
        private readonly MVCDemoContext mvcDemoContext;

        public FineController(MVCDemoContext mvcDemoDbContext)
        {
            this.mvcDemoContext = mvcDemoDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var fines = await mvcDemoContext.fines
                .Join(mvcDemoContext.bookBorrow,
                    f => f.borrowBookId,
                    bb => bb.borrowBookId,
                    (f, bb) => new { Fine = f, Borrow = bb })
                .Join(mvcDemoContext.bookBorrow,
                    fb => fb.Borrow.borrowBookId,
                    b => b.borrowBookId,
                    (fb, b) => new { FineBorrow = fb, Borrow = b })
                .Join(mvcDemoContext.BCopy,
                    fbb => fbb.FineBorrow.Borrow.bookCopyId,
                    bc => bc.bookCopyId,
                    (fbb, bc) => new { FineBorrowBc = fbb, BookCopy = bc })
                .Join(mvcDemoContext.Books,
                    fbbc => fbbc.BookCopy.bookId,
                    b => b.bookId,
                    (fbbc, b) => new Fine
                    {
                        FineId = fbbc.FineBorrowBc.FineBorrow.Fine.FineId,
                        FineAmount = fbbc.FineBorrowBc.FineBorrow.Fine.FineAmount,
                        FineDate = fbbc.FineBorrowBc.FineBorrow.Fine.FineDate,
                        borrowBookId = fbbc.FineBorrowBc.FineBorrow.Borrow.borrowBookId,
                        bookTitle = b.bookTitle
                    })
                .ToListAsync();

            return View(fines);
        }

        [HttpGet]
        public IActionResult Add()
        {
			var bookBorrow = mvcDemoContext.bookBorrow
			   .Select(c => new SelectListItem
			   {
				   Value = c.borrowBookId.ToString(),
				   Text = c.borrowBookDate.ToString()
			   })
			   .ToList();
			
			var addFineViewModel = new AddFineViewModel
			{
				bBorrow = bookBorrow
			};
			return View(addFineViewModel);
		}

        [HttpPost]
        public async Task<IActionResult> Add(AddBorrowBookViewModel addBorrowBookViewModel)
        {
            var borrowBook = new BookBorrow()
            {
                borrowBookDate = addBorrowBookViewModel.borrowBookDate,
                borrowDueDate = addBorrowBookViewModel.borrowDueDate,
                bookCopyId = addBorrowBookViewModel.bookCopyId,
                memberId = addBorrowBookViewModel.memberId,
                isReturned = addBorrowBookViewModel.isReturned
            };
            await mvcDemoContext.bookBorrow.AddAsync(borrowBook);
            await mvcDemoContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> View(int id)
		{
			var fine= await mvcDemoContext.fines.FindAsync(id);

			if (fine == null)
			{
				return RedirectToAction(nameof(Index));
			}

			var viewModel = new UpdateFineViewModel()
			{
			   
				FineAmount = fine.FineAmount,
				FineDate = fine.FineDate,
				borrowBookId = fine.borrowBookId
				
			};

			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Update(int id, UpdateFineViewModel viewModel)
		{


			var fine = await mvcDemoContext.fines.FindAsync(id);

			fine.FineAmount = viewModel.FineAmount;
			fine.FineDate= viewModel.FineDate;
			fine.borrowBookId = viewModel.borrowBookId;
			await mvcDemoContext.SaveChangesAsync();

			return RedirectToAction(nameof(Index));

		}

		[HttpPost]
        public async Task<IActionResult> Delete(UpdateFineViewModel model)
        {
            var fines = await mvcDemoContext.fines.FindAsync(model.FineId);

            mvcDemoContext.fines.Remove(fines);
            await mvcDemoContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}