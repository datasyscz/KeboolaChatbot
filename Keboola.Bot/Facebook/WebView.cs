namespace Keboola.Bot.Facebook
{
    public class Messenger
    {
        public MessengerChannelData ChannelData { get; set; }
    }

    public class MessengerChannelData
    {
        public string notification_type { get; set; }
        public MessengerAttachment attachment { get; set; }
    }

    public class MessengerAttachment
    {
        public string type { get; set; }
        public MessengerPayload payload { get; set; }
    }

    public class MessengerPayload
    {
        public string template_type { get; set; }
        public MessengerElement[] elements { get; set; }
    }

    public class MessengerElement
    {
        public string title { get; set; }
        public string subtitle { get; set; }
        public string item_url { get; set; }
        public string image_url { get; set; }
        public MessengerButton[] buttons { get; set; }
    }

    public class MessengerButton
    {
        public string type { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string payload { get; set; }
    }

    public class TestFB
    {
    }

    public class Rootobject
    {
        public string notification_type = "NO_PUSH";
        public Attachment attachment { get; set; }
    }

    public class Message
    {
        public Attachment attachment { get; set; }
    }

    public class Attachment
    {
        public string type { get; set; }
        public Payload payload { get; set; }
    }

    public class Payload
    {
        public string template_type { get; set; }
        public Element[] elements { get; set; }
    }

    public class Element
    {
        public string title { get; set; }
        public string item_url { get; set; }
        public string image_url { get; set; }
        public string subtitle { get; set; }
        public Button[] buttons { get; set; }
    }

    public class Button
    {
        internal string fallback_url;
        public bool messenger_extensions;

        public string type { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string webview_height_ratio { get; set; }
    }
}