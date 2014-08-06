using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraEndeavors.GameEngine
{
    public delegate void OnInitialized();
    public delegate void OnTransactionsRestored();
    public delegate void OnStartingPurchase();
    public delegate void OnItemPurchaseChange(string Id);

    public interface IBilling
    {
        event OnInitialized Initalized;
        event OnItemPurchaseChange ItemPurchaseFailure;
        event OnItemPurchaseChange ItemPurchaseSuccess;
        event OnStartingPurchase StartingPurchase;
        event OnTransactionsRestored TransactionsRestored;

        void InitiatePurchase(IInAppProduct product);
        
        bool IsInitialized { get; }
        bool IsItemPurchased(IInAppProduct product);
        
        void RestoreTransactions();
        
    }
}
