namespace MySignalR.Models
{
    public class ChatViewModel
    {
        public ChatViewModel()
        {
            Rooms = new List<ChatRoom>();
        }

        public int MaxRooms { get; set; }

        public IList<ChatRoom> Rooms { get; set; }

        public string? UserId { get; set; }

        public bool AllowRooms => Rooms == null || Rooms.Count < MaxRooms;
    }
}
