using NewNew.Data;
using NewNew.Models.Domain;
using NewNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewNew.Controllers
{
    public class BorrowBookController : Controller
    {

        private readonly MVCDemoContext mvcDemoContext;

        public BorrowBookController(MVCDemoContext mvcDemoDbContext)
        {
            this.mvcDemoContext = mvcDemoDbContext;
        }

        public async Task<IActionResult> BorrowBookIndex()
        {
            var borrow = await mvcDemoContext.bookBorrow
                        .Join(mvcDemoContext.Members,
                              b => b.memberId,
                              c => c.memberId,
                              (b, c) => new { bookBorrow = b, Members = c })
                        .Join(mvcDemoContext.BCopy,
                              bc => bc.bookBorrow.bookCopyId,
                              bCopy => bCopy.bookCopyId,
                              (bc, bCopy) => new { Borrow = bc.bookBorrow, Member = bc.Members, BookCopy = bCopy })
                        .Join(mvcDemoContext.Books,
                              bcbc => bcbc.BookCopy.bookId,
                              book => book.bookId,
                              (bcbc, book) => new BookBorrow
                              {
                                  borrowBookId = bcbc.Borrow.borrowBookId,
                                  memberFullName = bcbc.Member.memberFullName,
                                  bookTitle = book.bookTitle,
                                  borrowBookDate = bcbc.Borrow.borrowBookDate,
                                  borrowDueDate = bcbc.Borrow.borrowDueDate,
                                  isReturned = bcbc.Borrow.isReturned,
                                  isNotified = bcbc.Borrow.isNotified
                              })
                        .ToListAsync();

            return View(borrow);
        }

        [HttpGet]
        public IActionResult BorrowBookAdd()
        {
            ViewData["Layout"] = "_Layout1";
            ViewData["Layout"] = "_Layout1";
            var members = mvcDemoContext.Members
                .Where(c => c.Status == MemberStatus.Active)
                .Select(c => new SelectListItem
                {
                    Value = c.memberId.ToString(),
                    Text = c.memberFullName
                })
                .ToList();
            var bCopy = mvcDemoContext.BCopy
                .Select(c => new SelectListItem
                {
                    Value = c.bookCopyId.ToString(),
                    Text = c.yearOfPublished.ToString()
                })
                .ToList();
        


            var addBorrowBookViewModel = new AddBorrowBookViewModel
            {
                Members = members,
                Bcopy = bCopy
            };

            return View(addBorrowBookViewModel);

        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBorrowBookViewModel addBorrowBookViewModel)
        {

            ViewData["Layout"] = "_Layout1";

            var member = mvcDemoContext.Members.FirstOrDefault(c => c.memberId == addBorrowBookViewModel.memberId);
            if (member == null || member.Status != MemberStatus.Active)
            {
                // Member not found or not active, handle the error accordingly
                return RedirectToAction("Error");
            }

            else
            {
                var borrowBook = new BookBorrow()
                {
                    borrowBookDate = addBorrowBookViewModel.borrowBookDate,
                    borrowDueDate = addBorrowBookViewModel.borrowDueDate,
                    bookCopyId = addBorrowBookViewModel.bookCopyId,
                    memberId = addBorrowBookViewModel.memberId,
                    isReturned = addBorrowBookViewModel.isReturned
                };

                mvcDemoContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT bookBorrow ON;");
                await mvcDemoContext.bookBorrow.AddAsync(borrowBook);
                await mvcDemoContext.SaveChangesAsync();
                mvcDemoContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT bookBorrow OFF;");
            }

         


            return RedirectToAction("BorrowBookIndex");
        }


        public async Task<IActionResult> View(int id)
        {
            var borrow = await mvcDemoContext.bookBorrow.FindAsync(id);

            if (borrow == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new UpdateBookBorrowViewModel
            {
                borrowBookId = borrow.borrowBookId,
                borrowBookDate = borrow.borrowBookDate,
                borrowDueDate = borrow.borrowDueDate,
                bookCopyId = borrow.bookCopyId,
                memberId = borrow.memberId,
                isReturned = borrow.isReturned,
                isNotified=borrow.isNotified
           

            };

            return await Task.Run(() => View("View", viewModel));
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateBookBorrowViewModel model)
        {
            var borrow = await mvcDemoContext.bookBorrow.FindAsync(model.borrowBookId);

            if (borrow == null)
            {
                return RedirectToAction(nameof(Index));
            }

            borrow.borrowBookDate = model.borrowBookDate;
            borrow.borrowDueDate = model.borrowDueDate;
            borrow.bookCopyId = model.bookCopyId;
            borrow.memberId = model.memberId;
            borrow.isReturned = model.isReturned;
            borrow.isNotified = model.isNotified;
            


            
            mvcDemoContext.bookBorrow.Update(borrow);
            await mvcDemoContext.SaveChangesAsync();

            return RedirectToAction(nameof(BorrowBookIndex));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(UpdateBookBorrowViewModel model)
        {
            var BBorrow = await mvcDemoContext.bookBorrow.FindAsync(model.borrowBookId);
            if (BBorrow != null)
            {
                mvcDemoContext.bookBorrow.Remove(BBorrow);
                await mvcDemoContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(BorrowBookIndex));
        }
    }
}
