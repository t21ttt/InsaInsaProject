using NewNew.Data;
using NewNew.Models.Domain;
using NewNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;

namespace NewNew.Controllers
{
    public class AuthorController : Controller
    {
        private readonly MVCDemoContext mvcDemoContext;

        public AuthorController(MVCDemoContext mvcDemoDbContext)
        {
            this.mvcDemoContext = mvcDemoDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var authors = await mvcDemoContext.author.ToListAsync();
            return View(authors);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddAuthorViewModel addAuthorViewModel)
        {

            var author = new Author()
            {
                authorFullName = addAuthorViewModel.authorFullName,
                email = addAuthorViewModel.email
            };

            await mvcDemoContext.author.AddAsync(author);
            await mvcDemoContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> View(int id)
        {
            var author = await mvcDemoContext.author.FindAsync(id);

            if (author== null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new UpdateAuthorViewModel()
            {
                authorId= author.authorId,
                authorFullName = author.authorFullName,
                email =author.email
               
            };

            return await Task.Run(() => View("View", viewModel));
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateAuthorViewModel model)
        {
            var author = await mvcDemoContext.author.FindAsync(model.authorId);

            if (author == null)
            {
                return RedirectToAction(nameof(Index));
            }

            author.authorFullName = model.authorFullName;
            author.email = model.email;
            


            mvcDemoContext.author.Update(author);
            await mvcDemoContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(UpdateAuthorViewModel model)
        {
            var author = await mvcDemoContext.author.FindAsync(model.authorId);

            if (author != null)
            {
                mvcDemoContext.author.Remove(author);
                await mvcDemoContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
