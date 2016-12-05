namespace AI
{
    public static class WitModels
    {
        public class Response
        {
            public string msg_id { get; set; }
            public string _text { get; set; }
            public Entities entities { get; set; }
        }

        public class Entities
        {
            public Number[] number { get; set; }
        }

        public class Number
        {
            public int confidence { get; set; }
            public string type { get; set; }
            public int value { get; set; }
        }
    }
}