using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraEndeavors.GameEngine
{
    interface IDataCache
    {
        bool GetBool(string key, bool? defaultValue);
        void SetBool(string key, bool defValue);


        string GetString(string key, string defaultValue);
        void SetString(string key, string defValue);


        int GetInt(string key, int? defaultValue);
        void SetInt(string key, int defValue);


        float GetFloat(string key, float? defaultValue);
        void SetFloat(string key, float defValue);

    }
}
