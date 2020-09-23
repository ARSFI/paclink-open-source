namespace Paclink
{
    public interface IMessage
    {
        // Common properties and methods...
        string Mime { get; set; }
        string Body { get; set; }

        void AddAttachment(Attachment objAttachment);
    }

}
