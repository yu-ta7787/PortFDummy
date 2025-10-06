using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortFDummy.Models
{
    public class Game
    {
        public int AppId { get; set; }
        public string Name { get; set; } = "";
        public int PlaytimeMinutes { get; set; }
        public DateTime? LastPlayed { get; set; }
        public string ImageUri { get; set; } = "";

        public string HeaderImageUrl =>
           $"https://cdn.cloudflare.steamstatic.com/steam/apps/{AppId}/header.jpg";// Steamのヘッダー画像URL
    }
}
