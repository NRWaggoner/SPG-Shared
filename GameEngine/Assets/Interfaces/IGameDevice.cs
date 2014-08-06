using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraEndeavors.GameEngine
{

    public interface IGameDevice
    {

        IBilling GetBilling();
        ICoRoutineRunner GetCoRoutineRunner();
        IDataCacheManager GetDataCacheManager();
        IMainBackButton GetMainBackButton();
        IInAppProduct GetProduct(string Id);
        ITelemetryManager GetTelemetryManager();
        ITransitionScenes GetSceneTransitioner();



        IGameScene CreateGameScene(string resourcePath);

        IGameMenu CreateGameMenu();


    }
}
