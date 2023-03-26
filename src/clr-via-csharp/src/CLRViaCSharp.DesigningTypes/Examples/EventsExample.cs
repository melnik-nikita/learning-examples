using CLRViaCSharp.Common.Interfaces;

namespace CLRViaCSharp.DesigningTypes.Examples;

public class EventsExample : IExample
{
    /// <inheritdoc />
    public static void Run()
    {
        Console.WriteLine("Events example start");

        var mailManager = new MailManager();

        var fax = new Fax(mailManager);

        mailManager.SimulateNewMail("Nick", "Veronika", "Birthday plans");

        Console.WriteLine("Events example End");
    }
}

// Create a class and define method that type, that exposes the event, will call when event occurs
internal sealed class Fax
{
    public Fax(MailManager mailManager)
    {
        mailManager.NewMail += FaxMsg;
    }

    private void FaxMsg(Object sender, NewMailEventArgs @event)
    {
        Console.WriteLine("Faxing mail message: ");
        Console.WriteLine("\tFrom={0}, To={1}, Subject={2}", @event.From, @event.To, @event.Subject);
    }

    public void Unregister(MailManager mailManager)
    {
        mailManager.NewMail -= FaxMsg;
    }
}

internal class MailManager
{
    // Step 2: Define the event member
    public event EventHandler<NewMailEventArgs> NewMail;

    // Step 3: Define a method responsible for raising the event to notify registered objects that the event has occurred
    protected virtual void OnNewMail(NewMailEventArgs e)
    {
        var temp = Volatile.Read(ref NewMail);

        if (temp is not null)
        {
            temp(this, e);
        }
    }

    // Step 4: Define a method that translates the input into the desired event
    public void SimulateNewMail(String from, String to, String subject)
    {
        NewMailEventArgs e = new (from, to, subject);
        OnNewMail(e);
    }
}

// Step 1: Define a type that will hold any additional info that should be sent to receivers of the event notification
internal sealed class NewMailEventArgs : EventArgs
{
    private readonly String _from;
    private readonly String _to;
    private readonly String _subject;

    public string From => _from;
    public string To => _to;
    public string Subject => _subject;

    public NewMailEventArgs(string from, string to, string subject)
    {
        _from = from;
        _to = to;
        _subject = subject;
    }
}
