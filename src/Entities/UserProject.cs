namespace Harkh_backend.src.Entities;

public class UserProject
{
    public Guid Id { get; set; } // primary key
    public Guid UserId { get; set; } // foreign key
    public Guid ProjectId { get; set; } // foreign key


}
