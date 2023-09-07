using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NewNew.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using NewNew.Models.Domain;
using MailKit.Security;

public class NotificationService : BackgroundService
{
    private readonly ILogger<NotificationService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IConfiguration _configuration;

    private DateTime lastExecutionDate; // Track the last execution date

    public NotificationService(ILogger<NotificationService> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _configuration = configuration;
        lastExecutionDate = DateTime.MinValue; // Initialize the last execution date
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var currentDate = DateTime.Today;

                if (currentDate > lastExecutionDate)
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<MVCDemoContext>();

                        // Get all overdue books
                        var overdueBooks = context.bookBorrow
                            .Where(b => b.borrowDueDate < currentDate && !b.isReturned)
                            .ToList();

                        foreach (var borrowBook in overdueBooks)
                        {
                            // Find the member associated with the BorrowBook
                            var member = await context.Members.FindAsync(borrowBook.memberId);

                            if (member == null)
                            {
                                // Member not found, handle error
                                continue;
                            }

                            // Calculate the number of days the book is overdue
                            var daysOverdue = (currentDate - borrowBook.borrowDueDate).Days;

                            // Calculate the late fee
                            var lateFeePerDay = 6m; // Replace with your own late fee policy
                            var lateFee = daysOverdue * lateFeePerDay;

                            var message = new MimeMessage();
                            message.From.Add(new MailboxAddress("Addis Ababa University", _configuration["EmailSettings:Username"]));
                            message.To.Add(new MailboxAddress(member.memberFullName, member.email));
                            message.Subject = "Overdue Book Notification";
                            message.Body = new TextPart("plain")
                            {
                                Text = $"Dear {member.memberFullName},\n\nWe wanted to remind you that you have an overdue book that was due on" +
                                $" {borrowBook.borrowDueDate.ToShortDateString()}. As of today, the book is {daysOverdue} days overdue. Please return the" +
                                $" book as soon as possible to avoid any further fees or consequences.\n\nIn accordance with our library policies, we will be" +
                                $" charging a late fee of {lateFee:C} for each day that the book is overdue. If the book is not returned within [Number of Days" +
                                $" Before Further Action], we will have to take further action, which may include suspending your library privileges or billing you" +
                                $" for the cost of the book.\n\nIf you have any questions or concerns, please don't hesitate to contact us at [Library Contact Information]." +
                                $" We appreciate your cooperation and look forward to hearing from you soon.\n\nBest regards,\n\nTefera Molla\n[Addis Ababa University ]"
                            };

                            // Send the email asynchronously
                            try
                            {
                                using (var client = new SmtpClient())
                                {
                                    await client.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:SmtpPort"]), SecureSocketOptions.StartTls);
                                    await client.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
                                    await client.SendAsync(message);
                                    await client.DisconnectAsync(true);
                                }
                            }
                            catch (Exception ex)
                            {
                                // Handle or log any exceptions that occurred during the email sending process
                                Console.WriteLine($"An error occurred while sending the email: {ex.Message}");
                            }










                            //// Build the email message
                         //var message = new MimeMessage();
                            //message.From.Add(new MailboxAddress("Library", _configuration["EmailSettings:Username"]));
                            //message.To.Add(new MailboxAddress(member.memberFullName, member.email));
                            //message.Subject = "Overdue Book Notification";
                            //message.Body = new TextPart("plain")
                            //{
                            //    Text = $"Dear {member.memberFullName},\n\nWe wanted to remind you that you have an overdue book that was due on {borrowBook.borrowDueDate.ToShortDateString()}. As of today, the book is {daysOverdue} days overdue. Please return the book as soon as possible to avoid any further fees or consequences.\n\nIn accordance with our library policies, we will be charging a late fee of {lateFee:C} for each day that the book is overdue. If the book is not returned within [Number of Days Before Further Action], we will have to take further action, which may include suspending your library privileges or billing you for the cost of the book.\n\nIf you have any questions or concerns, please don't hesitate to contact us at [Library Contact Information]. We appreciate your cooperation and look forward to hearing from you soon.\n\nBest regards,\n\n[Your Name]\n[Library Name]"
                            //};

                            //// Send the email asynchronously
                            //using (var client = new SmtpClient())
                            //{
                            //    await client.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:SmtpPort"]));
                            //    await client.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
                            //    await client.SendAsync(message);
                            //    await client.DisconnectAsync(true);
                            //}

                            // Check if a fine already exists for the current date and borrow book ID
                            // Check if a fine already exists for the current date and borrow book ID
                            var existingFine = context.fines.FirstOrDefault(f => f.borrowBookId == borrowBook.borrowBookId && f.FineDate == currentDate);

                            if (existingFine == null)
                            {
                                // Add a new fine
                                var fine = new Fine
                                {
                                    FineAmount = (int)lateFee,
                                    FineDate = currentDate,
                                    borrowBookId = borrowBook.borrowBookId
                                };
                                context.fines.Add(fine);
                            }
                            else
                            {
                                // Update the existing fine
                                existingFine.FineAmount = (int)lateFee;
                            }

                            borrowBook.isNotified = true;
                        }

                        await context.SaveChangesAsync();
                    }

                    lastExecutionDate = currentDate; // Update the last execution date
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending overdue book notifications.");
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken); // Delay the execution for one day
        }
    }
}
