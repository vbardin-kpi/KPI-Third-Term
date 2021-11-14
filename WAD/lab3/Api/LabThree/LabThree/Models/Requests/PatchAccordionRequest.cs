namespace LabThree.Models.Requests;

public class PatchAccordionRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}