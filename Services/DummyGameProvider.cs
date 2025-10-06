namespace PortFDummy.Services
{
    using System;
    using System.Collections.Generic;
    using PortFDummy.Models;

    public class DummyGameProvider : IGameProvider
    {
        public List<Game> LoadGames() => new()// ダミーのゲームデータを返す
{
    new Game{ AppId=2246340, Name="MONSTER HUNTER WILDS", PlaytimeMinutes=5, LastPlayed=null },
    new Game{ AppId=582010, Name="MONSTER HUNTER: WORLD", PlaytimeMinutes=3420, LastPlayed=DateTime.Today.AddDays(-1) },
    new Game{ AppId=814380, Name="SEKIRO: SHADOWS DIE TWICE", PlaytimeMinutes=600, LastPlayed=DateTime.Today.AddDays(-15) },
    new Game{ AppId=1462040, Name="FINAL FANTASY VII REMAKE INTERGRADE", PlaytimeMinutes=450, LastPlayed=DateTime.Today.AddDays(-5) },
    new Game{ AppId=637650, Name="FINAL FANTASY XV WINDOWS EDITION", PlaytimeMinutes=720, LastPlayed=DateTime.Today.AddDays(-40) },
    new Game{ AppId=39210, Name="FINAL FANTASY XIV ONLINE", PlaytimeMinutes=950, LastPlayed=DateTime.Today.AddDays(-20) },
    new Game{ AppId=1971650, Name="OCTOPATH TRAVELER II", PlaytimeMinutes=240, LastPlayed=DateTime.Today.AddDays(-60) },
    new Game{ AppId=1295510, Name="DRAGON QUEST XI S", PlaytimeMinutes=480, LastPlayed=DateTime.Today.AddDays(-100) },
    new Game{ AppId=524220, Name="NieR:Automata™", PlaytimeMinutes=300, LastPlayed=DateTime.Today.AddDays(-10) },
    new Game{ AppId=1174180, Name="Red Dead Redemption 2", PlaytimeMinutes=2400, LastPlayed=DateTime.Today.AddDays(-5) },
    new Game{ AppId=582010,  Name="MONSTER HUNTER: WORLD",      PlaytimeMinutes=3420, LastPlayed=DateTime.Today.AddDays(-1) },
    new Game{ AppId=1172470, Name="APEX LEGENDS",               PlaytimeMinutes=1800, LastPlayed=DateTime.Today.AddDays(-10) },
    new Game{ AppId=39210,   Name="FINAL FANTASY XIV",          PlaytimeMinutes=950,  LastPlayed=DateTime.Today.AddDays(-20) },
    new Game{ AppId=1245620, Name="ELDEN RING",                 PlaytimeMinutes=870,  LastPlayed=DateTime.Today.AddDays(-7) },
    new Game{ AppId=814380,  Name="SEKIRO: SHADOWS DIE TWICE",  PlaytimeMinutes=600,  LastPlayed=DateTime.Today.AddDays(-15) },
    new Game{ AppId=648800,  Name="RAFT",                       PlaytimeMinutes=30,   LastPlayed=null },
    new Game{ AppId=367520,  Name="HOLLOW KNIGHT",              PlaytimeMinutes=45,   LastPlayed=null },
    new Game{ AppId=646570,  Name="SLAY THE SPIRE",             PlaytimeMinutes=90,   LastPlayed=null },
    new Game{ AppId=391540,  Name="UNDERTALE",                  PlaytimeMinutes=50,   LastPlayed=null },
    new Game{ AppId=268910,  Name="CUPHEAD",                    PlaytimeMinutes=75,   LastPlayed=null },
    new Game{ AppId=578080,  Name="PUBG: BATTLEGROUNDS",        PlaytimeMinutes=250,  LastPlayed=DateTime.Today.AddDays(-30) },
    new Game{ AppId=413150,  Name="STARDEW VALLEY",             PlaytimeMinutes=310,  LastPlayed=DateTime.Today.AddDays(-60) },
    new Game{ AppId=105600,  Name="TERRARIA",                   PlaytimeMinutes=480,  LastPlayed=DateTime.Today.AddDays(-90) },
    new Game{ AppId=294100,  Name="RIMWORLD",                   PlaytimeMinutes=150,  LastPlayed=DateTime.Today.AddDays(-120) },
    new Game{ AppId=242760,  Name="THE FOREST",                 PlaytimeMinutes=200,  LastPlayed=DateTime.Today.AddDays(-5) },
    new Game{ AppId=271590,  Name="Grand Theft Auto V",         PlaytimeMinutes=5200, LastPlayed=DateTime.Today.AddDays(-2) },
    new Game{ AppId=381210,  Name="Dead by Daylight",           PlaytimeMinutes=1240, LastPlayed=DateTime.Today.AddDays(-14) },
    new Game{ AppId=730,     Name="Counter-Strike 2",           PlaytimeMinutes=1400, LastPlayed=DateTime.Today.AddDays(-3) },
    new Game{ AppId=570,     Name="Dota 2",                     PlaytimeMinutes=2200, LastPlayed=DateTime.Today.AddDays(-1) },
    new Game{ AppId=1174180, Name="Red Dead Redemption 2",      PlaytimeMinutes=2400, LastPlayed=DateTime.Today.AddDays(-5) },
    new Game{ AppId=1091500, Name="Cyberpunk 2077",             PlaytimeMinutes=1500, LastPlayed=DateTime.Today.AddDays(-4) },
    new Game{ AppId=1240440, Name="Persona 5 Royal",            PlaytimeMinutes=360,  LastPlayed=DateTime.Today.AddDays(-12) },
    new Game{ AppId=292030,  Name="The Witcher 3: Wild Hunt",   PlaytimeMinutes=1800, LastPlayed=DateTime.Today.AddDays(-50) },
    new Game{ AppId=548430,  Name="Deep Rock Galactic",         PlaytimeMinutes=400,  LastPlayed=DateTime.Today.AddDays(-22) },
    new Game{ AppId=739630,  Name="Phasmophobia",               PlaytimeMinutes=260,  LastPlayed=DateTime.Today.AddDays(-11) },
    new Game{ AppId=504230, Name="Celeste", PlaytimeMinutes=45, LastPlayed=null },
    new Game{ AppId=1057090, Name="Ori and the Will of the Wisps", PlaytimeMinutes=60, LastPlayed=null },
    new Game{ AppId=1145360, Name="Hades", PlaytimeMinutes=90, LastPlayed=null },
    new Game{ AppId=739630, Name="Phasmophobia", PlaytimeMinutes=30, LastPlayed=null },
};


    }
}
