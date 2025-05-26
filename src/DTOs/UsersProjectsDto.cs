namespace Harkh_backend.src.DTOs
{
    public class UsersProjectsCreateDto
    {
        public Guid? UserId { get; set; }
        public Guid ProjectId { get; set; }
    }
    public class UsersProjectsReadDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
    }
}