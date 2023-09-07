using NewNew.Data;
using NewNew.Models.Domain;
using NewNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace NewNew.Controllers
{
    public class HomeMemberController : Controller
    {

        private readonly MVCDemoContext mvcDemoContext;

        public HomeMemberController(MVCDemoContext mvcDemoDbContext)
        {
            this.mvcDemoContext = mvcDemoDbContext;
        }

        public async Task<IActionResult> Detail(int id)
        {
            ViewData["Layout"] = "_Layout1";
            var book = await mvcDemoContext.Books.FirstOrDefaultAsync(x => x.bookId == id);
            var bookList = new List<Book> { book };
            return View(bookList);
          
        }

        public async Task<IActionResult> MemberIndex()
        {
            ViewData["Layout"] = "_Layout1";
            var members = await mvcDemoContext.Members.ToListAsync();
            return View(members);
        }
        public IActionResult Index()
        {
            ViewData["Layout"] = "_Layout1";

            return View();
        }

        public IActionResult MemberAdd()
        {
            ViewData["Layout"] = "_Layout1";
            return View();
        }
      [HttpPost]
   
        public async Task<IActionResult>MemberAdd(AddMemberViewModel addMemberViewModel)
        {
            ViewData["Layout"] = "_Layout1";
            var member = new Member()
            {
                memberFullName = addMemberViewModel.memberFullName,
                gender = addMemberViewModel.gender,
                email = addMemberViewModel.email,
                phoneNumber = addMemberViewModel.phoneNumber,
                memberPassword = addMemberViewModel.memberPassword,
            };
            await mvcDemoContext.Members.AddAsync(member);
            await mvcDemoContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> View(int id)
        {
            ViewData["Layout"] = "_Layout1";
            var member= await mvcDemoContext.Members.FindAsync(id);

            if (member == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new UpdateMemberViewModel()
            {
                memberId = member.memberId,
                memberFullName = member.memberFullName,
                gender = member.gender,
                email = member.email,
                phoneNumber = member.phoneNumber,
                memberPassword = member.memberPassword,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> View(int id, UpdateMemberViewModel model)
        {
            ViewData["Layout"] = "_Layout1";
            var member = await mvcDemoContext.Members.FindAsync(id);

            if (member == null)
            {
                return RedirectToAction(nameof(Index));
            }

            member.memberFullName = model.memberFullName;
            member.gender = model.gender;
            member.email = model.email;
            member.phoneNumber = model.phoneNumber;
            member.memberPassword = model.memberPassword;

            await mvcDemoContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


       public async Task<IActionResult> BookIndex()
        {
            ViewData["Layout"] = "_Layout1";
            var books = await mvcDemoContext.Books
                        .Join(mvcDemoContext.bookCategory,
                              b => b.bookCategroryId,
                              c => c.bookCategroryId,
                              (b, c) => new Book
                              {
                                  bookId = b.bookId,
                                  bookTitle=b.bookTitle,
                                 
                                  bookDiscreption = b.bookDiscreption,
                                  bookAmount=b.bookAmount,
                                  categoryName = c.categoryName
                              })
                        .ToListAsync();
            return View(books);
        }
        public async Task<IActionResult> BookCopyIndex()
        {
            ViewData["Layout"] = "_Layout1";
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

        public IActionResult FineIndex()
        {
            ViewData["Layout"] = "_Layout1";
            return View();
        }
        public async Task<IActionResult> ReservationIndex()
        {
            ViewData["Layout"] = "_Layout1";
            var reservation = await mvcDemoContext.reservation
                            .Join(mvcDemoContext.Members,
                                  b => b.memberId,
                                  c => c.memberId,
                                  (b, c) => new { Reservation = b, Member = c })
                            .Join(mvcDemoContext.Books,
                                  bc => bc.Reservation.bookId,
                                  book => book.bookId,
                                  (bc, book) => new Reservation
                                  {
                                      ReservationId = bc.Reservation.ReservationId,
                                      memberFullName = bc.Member.memberFullName,
                                      bookTitle = book.bookTitle,
                                      reservatonDate = bc.Reservation.reservatonDate
                                  })
                            .ToListAsync();

            return View(reservation);
        }
        public IActionResult ReservationAdd()
        {
            ViewData["Layout"] = "_Layout1";
            var books= mvcDemoContext.Books
                .Select(c => new SelectListItem
                {
                    Value = c.bookId.ToString(),
                    Text = c.bookTitle
                })
                .ToList();

            var members = mvcDemoContext.Members
                .Select(c => new SelectListItem
                {
                    Value = c.memberId.ToString(),
                    Text = c.memberFullName
                })
                .ToList();

            var addReservationViewModel = new AddReservationViewModel
            {
                Books = books,
                Members = members
            };

            return View(addReservationViewModel);
           
        }
        [HttpPost]
        public async Task<IActionResult> ReservationAdd(AddReservationViewModel addReservationViewModel)
        {
            ViewData["Layout"] = "_Layout1";
            var reservation = new Reservation()
            {
                // Remove the ReservationId property assignment
                memberId = addReservationViewModel.memberId,
                bookId = addReservationViewModel.bookId,
                reservatonDate = addReservationViewModel.reservatonDate
            };

            // Remove the ReservationId parameter from the AddAsync method
            await mvcDemoContext.reservation.AddAsync(reservation);
            await mvcDemoContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ReservationView(string id)
        {
            ViewData["Layout"] = "_Layout1";
            var reservation = await mvcDemoContext.reservation.FindAsync(id);

            if (reservation == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new UpdateReservationViewModel()
            {
                ReservationId = reservation.ReservationId,
                memberId = reservation.memberId,
                bookId = reservation.bookId,
                reservatonDate = reservation.reservatonDate

            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, UpdateReservationViewModel viewModel)
        {

            ViewData["Layout"] = "_Layout1";
            var reserve = await mvcDemoContext.reservation.FindAsync(id);

            reserve.ReservationId = viewModel.ReservationId;
            reserve.memberId = viewModel.memberId;
            reserve.memberId = viewModel.memberId;
            reserve.bookId = viewModel.bookId;
            reserve.reservatonDate = viewModel.reservatonDate;
            await mvcDemoContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }





        //This borrowing book

        public async Task<IActionResult> BorrowBookIndex()
        {
            ViewData["Layout"] = "_Layout1";
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

        public IActionResult Add()
        {
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

            var borrowBook = new BookBorrow()
            {
                borrowBookDate = addBorrowBookViewModel.borrowBookDate,
                borrowDueDate = addBorrowBookViewModel.borrowDueDate,
                bookCopyId = addBorrowBookViewModel.bookCopyId,
                memberId = addBorrowBookViewModel.memberId,
                isReturned = false
            };

            mvcDemoContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT bookBorrow ON;");
            await mvcDemoContext.bookBorrow.AddAsync(borrowBook);
            await mvcDemoContext.SaveChangesAsync();
            mvcDemoContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT bookBorrow OFF;");

            return RedirectToAction("BorrowBookIndex");
        }

        public async Task<IActionResult> BorrowBookView(int id)
        {
            ViewData["Layout"] = "_Layout1";
            var borrow = await mvcDemoContext.bookBorrow.FindAsync(id);

            if (borrow == null)
            {
                return RedirectToAction(nameof(BorrowBookIndex));
            }

            var viewModel = new UpdateBookBorrowViewModel()
            {
                borrowBookId = borrow.borrowBookId,
                borrowBookDate = borrow.borrowBookDate,
                borrowDueDate = borrow.borrowDueDate,
                bookCopyId = borrow.bookCopyId,
                memberId = borrow.memberId,
                isReturned = borrow.isReturned

            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, UpdateBookBorrowViewModel viewModel)
        {
            ViewData["Layout"] = "_Layout1";

            var borrow = await mvcDemoContext.bookBorrow.FindAsync(id);

            borrow.borrowBookId = viewModel.borrowBookId;
            borrow.borrowBookDate = viewModel.borrowBookDate;
            borrow.borrowDueDate = viewModel.borrowDueDate;
            borrow.bookCopyId = viewModel.bookCopyId;
            borrow.memberId = viewModel.memberId;
            borrow.isReturned = viewModel.isReturned;
            await mvcDemoContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateBookBorrowViewModel model)
        {
            ViewData["Layout"] = "_Layout1";
            var BBorrow = await mvcDemoContext.bookBorrow.FindAsync(model.borrowBookId);
            if (BBorrow != null)
            {
                mvcDemoContext.bookBorrow.Remove(BBorrow);
                await mvcDemoContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }





        //This is fine payment code snippt
        public async Task<IActionResult> FinePaymentIndex()
        {
            ViewData["Layout"] = "_Layout1";
            var finePaymment = await mvcDemoContext.finePayments.ToListAsync();
            return View(finePaymment);
        }
        [HttpGet]
        public IActionResult FinePaymentAdd()
        {
            ViewData["Layout"] = "_Layout1";
            var member = mvcDemoContext.Members
                .Select(c => new SelectListItem
                {
                    Value = c.memberId.ToString(),
                    Text = c.memberFullName
                })
                .ToList();

            var fine = mvcDemoContext.fines
                .Select(c => new SelectListItem
                {
                    Value = c.FineId.ToString(),
                    Text = c.FineAmount.ToString()
                })
                .ToList();

            var addFinePaymentViewModel = new AddFinePaymentViewModel
            {
                Members = member,
                fines = fine
            };

            return View(addFinePaymentViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddFine(AddFinePaymentViewModel addFinePaymentViewModel)
        {
            ViewData["Layout"] = "_Layout1";
            var finePayment = new FinePayment()
            {
                finePaymentAmount = addFinePaymentViewModel.finePaymentAmount,
                finePaymentDate = addFinePaymentViewModel.finePaymentDate,
                memberId = addFinePaymentViewModel.memberId,
                FineId = addFinePaymentViewModel.FineId
            };

            await mvcDemoContext.finePayments.AddAsync(finePayment);
            await mvcDemoContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> FinePaymentView(string id)
        {
            ViewData["Layout"] = "_Layout1";
            var finePayment = await mvcDemoContext.finePayments.FindAsync(id);

            if (finePayment == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new UpdateFinePaymentViewModel()
            {

                finePaymentId = finePayment.finePaymentId,
                finePaymentAmount = finePayment.finePaymentAmount,
                memberId = finePayment.memberId,
                finePaymentDate = finePayment.finePaymentDate


            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, UpdateFinePaymentViewModel viewModel)
        {
            ViewData["Layout"] = "_Layout1";

            var payment = await mvcDemoContext.finePayments.FindAsync(id);

            payment.finePaymentAmount = viewModel.finePaymentAmount;
            payment.memberId = viewModel.memberId;
            payment.finePaymentDate = viewModel.finePaymentDate;
            await mvcDemoContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateFinePaymentViewModel model)
        {
            ViewData["Layout"] = "_Layout1";
            var payment = await mvcDemoContext.finePayments.FindAsync(model.finePaymentId);
            if (payment != null)
            {
                mvcDemoContext.finePayments.Remove(payment);
                await mvcDemoContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        //This is the code sinnept of SignUp
        public IActionResult SignUp()
        {
            ViewData["Layout"] = "_Layout1";
            return View();
        }

        [HttpPost]
        public IActionResult Signup(SignupViewModel model)
        {
            { 
            ViewData["Layout"] = "_Layout1";
            if (ModelState.IsValid)
            {
                // Create a new instance of your database context
                using (var dbContext = new MVCDemoContext())
                {
                    // Create a new user object and populate its properties with the form data
                    var member = new Member
                    {
                        memberFullName = model.memberFullName,
                        gender = model.gender,
                        email = model.email,
                        phoneNumber = model.phoneNumber,
                        memberPassword = model.memberPassword
                    };

                    // Save the user to the database
                    mvcDemoContext.Members.Add(member);
                    mvcDemoContext.SaveChanges();
                }

                // Redirect to the login page after successful signup
                return RedirectToAction("Login", "HomeMember");
            }
                return View(model);
        }
        }


            //This is the code of Login
            public IActionResult Login()
        {
            ViewData["Layout"] = "_Layout1";
            return View();
        }
        

        //This is the code of logout
        public ActionResult Logout()
        {
            ViewData["Layout"] = "_Layout1";
            return RedirectToAction("userlogin", "HomeMember"); // Assuming "Login" is the login action method in the "AccountController"
        }



    }



    // Other action methods here
}

