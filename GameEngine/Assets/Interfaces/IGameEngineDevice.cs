using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraEndeavors.GameEngine
{

    interface IGameDevice
    {
        IInAppProduct GetProduct(string Id);
        IGameScene CreateGameScene(string resourcePath);

        IGameMenu CreateGameMenu();
    }
}
