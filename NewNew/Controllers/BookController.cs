using NewNew.Data;
using NewNew.Models;
using NewNew.Models.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NewNew.Controllers
{
    public class BookController : Controller
    {
        private readonly MVCDemoContext mvcDemoContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<BookController> _logger;

        public BookController(ILogger<BookController> logger, MVCDemoContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            mvcDemoContext = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var books = await mvcDemoContext.Books.ToListAsync();
        //    return View(books);
        //}
        public async Task<IActionResult> Index()
        {
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
       //View the details of the book

        public async Task<IActionResult> Detail(int id)
        {
            //ViewData["Layout"] = "_Layout1";
            var book = await mvcDemoContext.Books.FirstOrDefaultAsync(x => x.bookId == id);
            var bookList = new List<Book> { book };
            return View(bookList);

        }


        [HttpGet]
        public IActionResult Add()
        {
            var categories = mvcDemoContext.bookCategory
                .Select(c => new SelectListItem
                {
                    Value = c.bookCategroryId.ToString(),
                    Text = c.categoryName
                })
                .ToList();
            var author = mvcDemoContext.author
                .Select(c => new SelectListItem
                {
                    Value = c.authorId.ToString(),
                    Text = c.authorFullName
                });

            var addBookViewModel = new AddBookViewModel
            {
                Categories = categories,
                Authors= author
            };

            return View(addBookViewModel);
        }

       
        [HttpPost]
        public async Task<IActionResult> Add(AddBookViewModel addBookViewModel, IFormFile bookImage)
        {
            // Save the book details to the database
///         string fileName = await UploadFileAsync(bookImage);

            var book = new Book()
            {
                bookTitle = addBookViewModel.bookTitle,
                bookISBN = addBookViewModel.bookISBN,
                bookDiscreption = addBookViewModel.bookDiscreption,
                bookAmount = addBookViewModel.bookAmount,
                bookCategroryId = addBookViewModel.bookCategroryId,
            
            };

            await mvcDemoContext.Books.AddAsync(book);
            await mvcDemoContext.SaveChangesAsync();
            var bookId = book.bookId;
            var BAuthor = new BookAuthor()
            {
                authorId = addBookViewModel.authorId,
                bookId = bookId
            };
            await mvcDemoContext.Authors.AddAsync(BAuthor);
            await mvcDemoContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateBookViewModel viewModel, IFormFile bookImage)
        {
            var book = await mvcDemoContext.Books.FindAsync(id);

            if (book != null)
            {
                //// Save the new image file to the server
                //string fileName = null;
                //if (bookImage != null)
                //{
                //    fileName = await UploadFileAsync(bookImage);
                //}

                // Update the book properties
                book.bookTitle = viewModel.bookTitle;
                book.bookISBN = viewModel.bookISBN;
                book.bookDiscreption = viewModel.bookDiscreption;
                book.bookAmount = viewModel.bookAmount;
                book.bookCategroryId = viewModel.bookCategoryId;
               // book.ProfileImage = fileName;

                await mvcDemoContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        //private async Task<string> UploadFileAsync(IFormFile bookImage)
        //{
        //    if (bookImage != null && bookImage.Length > 0)
        //    {
        //        string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "Images");
        //        Directory.CreateDirectory(uploadDir); // Create the "Images" directory if it doesn't exist

        //        string fileName = Guid.NewGuid().ToString() + "-" + Path.GetFileName(bookImage.FileName);
        //        string filePath = Path.Combine(uploadDir, fileName);

        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await bookImage.CopyToAsync(fileStream);
        //        }

        //        return fileName;
        //    }

        //    return null;
        //}
        public async Task<IActionResult> View(int id)
        {
            var book = await mvcDemoContext.Books.FindAsync(id);

            if (book == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new UpdateBookViewModel()
            {
                BookId = book.bookId,
                bookTitle=book.bookTitle,
                bookISBN=book.bookISBN,
                bookDiscreption = book.bookDiscreption,
                bookAmount =book.bookAmount,
                bookCategoryId=book.bookCategroryId




            };

            return await Task.Run(() => View("View", viewModel));
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateBookViewModel model)
        {
            var book= await mvcDemoContext.Books.FindAsync(model.BookId);

            if (book == null)
            {
                return RedirectToAction(nameof(Index));
            }

            book.bookTitle = model.bookTitle;
            book.bookISBN = model.bookISBN;
            book.bookDiscreption = model.bookDiscreption;
            book.bookAmount = model.bookAmount;
          book.bookCategroryId = model.bookCategoryId;




            mvcDemoContext.Books.Update(book);
            await mvcDemoContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateBookViewModel model)
        {
            var book = await mvcDemoContext.Books.FindAsync(model.BookId);
            if (book != null)
            {
                mvcDemoContext.Books.Remove(book);
                await mvcDemoContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}