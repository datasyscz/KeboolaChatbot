namespace DatabaseModel
{
    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        ///     Bot framework id
        /// </summary>
        public string FrameworkId { get; set; }
    }
}