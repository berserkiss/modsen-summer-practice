using EmailService.Services;

Console.WriteLine("Email Service started. Press Ctrl+C to exit.");

using var service = new EmailService.Services.EmailService();
service.StartListening();

// Ожидание Ctrl+C
var resetEvent = new ManualResetEvent(false);
Console.CancelKeyPress += (_, _) => resetEvent.Set();
resetEvent.WaitOne();