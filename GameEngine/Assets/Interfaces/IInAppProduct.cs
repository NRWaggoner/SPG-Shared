using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraEndeavors.GameEngine
{
    public delegate void OnProductChanged(IInAppProduct sender);

    public enum BypassType
    {
        OneTime,
        Permanent
    };

    public interface IInAppProduct
    {
        event OnProductChanged ProductChanged;


        void BypassPurchase(BypassType bypassType);
        void InitiatePurchase();


        string LocalId { get; }
        bool IsInitialized { get; }
        bool? IsPurachased { get; }

        string ResourcePath { get; }

    }
}
