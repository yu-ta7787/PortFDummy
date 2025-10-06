using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortFDummy.Services
{
    using System.Collections.Generic;
    using PortFDummy.Models;

    public interface IGameProvider
    {
        List<Game> LoadGames();// ゲーム一覧を取得
    }
}