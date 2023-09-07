using NewNew.Data;
using NewNew.Models.Domain;
using NewNew.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewNew.Controllers
{
    public class ReservationController : Controller
    {
        private readonly MVCDemoContext mvcDemoContext;

        public ReservationController(MVCDemoContext mvcDemoDbContext)
        {
            this.mvcDemoContext = mvcDemoDbContext;
        }

        public async Task<IActionResult> Index()
        {
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
        public async Task<IActionResult> Add(AddReservationViewModel addReservationViewModel)
        {
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

        public async Task<IActionResult> View(int id)
        {
            var reserv = await mvcDemoContext.reservation.FindAsync(id);

            if (reserv== null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new UpdateReservationViewModel()
            {
               ReservationId = reserv.ReservationId,
               memberId=reserv.memberId,
               bookId=reserv.bookId,
               reservatonDate = reserv.reservatonDate
              

            };

            return await Task.Run(() => View("View", viewModel));
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateReservationViewModel model)
        {
            var reserv = await mvcDemoContext.reservation.FindAsync(model.ReservationId);

            if (reserv == null)
            {
                return RedirectToAction(nameof(Index));
            }

            reserv.memberId = model.memberId;
            reserv.bookId = model.bookId;
            reserv.reservatonDate = model.reservatonDate;
           



            mvcDemoContext.reservation.Update(reserv);
            await mvcDemoContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
            public async Task<IActionResult> Delete(UpdateReservationViewModel model)
            {
                var reservation= await mvcDemoContext.reservation.FindAsync(model.ReservationId);
                if (reservation != null)
                {
                    mvcDemoContext.reservation.Remove(reservation);
                    await mvcDemoContext.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }



        }
    }

