namespace Templater.Builder;

public class KeyExistingException : Exception
{
    public string Key { get; init; }

    public KeyExistingException(string message) 
        : base(message) { }
}