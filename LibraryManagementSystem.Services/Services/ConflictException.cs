namespace LibraryManagementSystem.Services;

public sealed class ConflictException(string message) : Exception(message);