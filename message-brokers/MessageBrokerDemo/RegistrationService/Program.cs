using RegistrationService.Services; 

Console.WriteLine("Registration Service started");


using var service = new RegistrationService.Services.RegistrationService();

while (true)
{
    Console.Write("Enter user email (or 'exit' to quit): ");
    var email = Console.ReadLine();
    
    if (email?.ToLower() == "exit") break;
    
    if (!string.IsNullOrWhiteSpace(email))
    {
        service.RegisterUser(email);
    }
}