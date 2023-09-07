//using System;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using LibraryManagementSystem.Data;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using MimeKit;
//using MailKit.Net.Smtp;
//using LibraryManagementSystem.Models.Domain;

//public class NotificationService : BackgroundService
//{
//    private readonly ILogger<NotificationService> _logger;
//    private readonly IServiceScopeFactory _serviceScopeFactory;

//    public NotificationService(ILogger<NotificationService> logger, IServiceScopeFactory serviceScopeFactory)
//    {
//        _logger = logger;
//        _serviceScopeFactory = serviceScopeFactory;
//    }

//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        while (!stoppingToken.IsCancellationRequested)
//        {
//            try
//            {
//                using (var scope = _serviceScopeFactory.CreateScope())
//                {
//                    var context = scope.ServiceProvider.GetRequiredService<MVCDemoContext>();

//                    // Get all overdue books
//                    var overdueBooks = context.bookBorrow
//                        .Where(b => b.borrowDueDate < DateTime.Today && !b.isReturned)
//                        .ToList();

//                    foreach (var borrowBook in overdueBooks)
//                    {
//                        // Find the member associated with the BorrowBook
//                        var member = await context.Members.FindAsync(borrowBook.memberId);

//                        if (member == null)
//                        {
//                            // Member not found, handle error
//                            continue;
//                        }

//                        // Calculate the number of days the book is overdue
//                        var daysOverdue = (DateTime.Today - borrowBook.borrowDueDate).Days;

//                        // Calculate the late fee
//                        var lateFeePerDay = 0.50m; // Replace with your own late fee policy
//                        var lateFee = daysOverdue * lateFeePerDay;

//                        // Build the email message
//                        var message = new MimeMessage();
//                        message.From.Add(new MailboxAddress("Library", "teferamollawerkineh@gmail.com"));
//                        message.To.Add(new MailboxAddress(member.memberFullName, member.email));
//                        message.Subject = "Overdue Book Notification";
//                        message.Body = new TextPart("plain")
//                        {
//                            Text = $"Dear {member.memberFullName},\n\nWe wanted to remind you that you have an overdue book that was due on {borrowBook.borrowDueDate.ToShortDateString()}. As of today, the book is {daysOverdue} days overdue. Please return the book as soon as possible to avoid any further fees or consequences.\n\nIn accordance with our library policies, we will be charging a late fee of {lateFee:C} for each day that the book is overdue. If the book is not returned within [Number of Days Before Further Action], we will have to take further action, which may include suspending your library privileges or billing you for the cost of the book.\n\nIf you have any questions or concerns, please don't hesitate to contact us at [Library Contact Information]. We appreciate your cooperation and look forward to hearing from you soon.\n\nBest regards,\n\n[Your Name]\n[Library Name]"
//                        };

//                        // Send the email asynchronously
//                        using (var client = new SmtpClient())
//                        {
//                            await client.ConnectAsync("smtp.gmail.com", 465, true);
//                            await client.AuthenticateAsync("teferamollawerkineh@gmail.com", "ohkvhybmohzkpjim");
//                            await client.SendAsync(message);
//                            await client.DisconnectAsync(true);
//                        }

//                        // Update the late fee for the member
//                        var fine = new Fine
//                        {
//                            FineAmount = (int)lateFee,
//                            FineDate = DateTime.Today,
//                            borrowBookId = borrowBook.borrowBookId
//                        };
//                        context.fines.Add(fine);

//                        borrowBook.isNotified = true;
//                        await context.SaveChangesAsync();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while sending overdue book notifications.");
//            }

//            // Wait for 24 hours before checking again
//            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
//        }
//    }
//}
