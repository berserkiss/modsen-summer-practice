namespace Common.Events;

public record UserRegisteredEvent(
    Guid UserId,
    string Email,
    DateTime Timestamp);