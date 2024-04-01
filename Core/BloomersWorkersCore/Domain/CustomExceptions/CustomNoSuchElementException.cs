using BloomersWorkersCore.Domain.Enums;

namespace BloomersWorkersCore.Domain.CustomExceptions;

public class CustomNoSuchElementException : Exception
{
    public Page.TypeEnum pages { get; set; }
    public string message { get; }
    public string identifier { get; }

    public CustomNoSuchElementException(string message, string identifier, Page.TypeEnum pages)
        : base($"{message} - {identifier}")
    {
        this.pages = pages;
        this.message = message;
        this.identifier = identifier;
    }
}