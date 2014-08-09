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

        List<IInAppProduct> GetAllProducts();

        List<IInAppProduct> GetUnlockProducts();

        ITelemetryManager GetTelemetryManager();
        ITransitionScenes GetSceneTransitioner();



        IGameScene CreateGameScene(string resourcePath);

        IGameMenu CreateGameMenu();


    }
}
