using NewNew.Data;
using NewNew.Models.Domain;
using NewNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NewNew.Controllers
{
    public class PublisherController : Controller
    {
        private readonly MVCDemoContext mvcDemoContext;

        public PublisherController(MVCDemoContext mvcDemoDbContext)
        {
            this.mvcDemoContext = mvcDemoDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var publisher = await mvcDemoContext.publisher.ToListAsync();
            return View(publisher);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPublisherViewModel addPublisherViewModel)
        {
            var publisher = new Publisher()
            {
                publisherName = addPublisherViewModel.publisherName,
                publisherAddress = addPublisherViewModel.publisherAddress,
            };
            await mvcDemoContext.publisher.AddAsync(publisher);
            await mvcDemoContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> View(int id)
        {
            var publisher = await mvcDemoContext.publisher.FindAsync(id);

            if (publisher == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new UpdatePublisherViewModel()
            {
                PublisherId = publisher.PublisherId,
                publisherName=publisher.publisherName,
                publisherAddress=publisher.publisherAddress
            

            };

            return await Task.Run(() => View("View", viewModel));
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdatePublisherViewModel model)
        {
            var publisher= await mvcDemoContext.publisher.FindAsync(model.PublisherId);

            if (publisher == null)
            {
                return RedirectToAction(nameof(Index));
            }

            publisher.publisherName= model.publisherName;
            publisher.publisherAddress = model.publisherAddress;



            mvcDemoContext.publisher.Update(publisher);
            await mvcDemoContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdatePublisherViewModel model)
        {
            var publisher= await mvcDemoContext.publisher.FindAsync(model.PublisherId);
            if (publisher != null)
            {
                mvcDemoContext.publisher.Remove(publisher);
                await mvcDemoContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
