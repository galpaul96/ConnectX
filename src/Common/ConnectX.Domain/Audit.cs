namespace ConnectX.Domain
{
    public class Audit
    {
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
