using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraEndeavors.GameEngine
{
    interface ITelemetryManager
    {
        void Awake();
        void ClickNextDot(int DotNumber);
        void ClickWhiteSpace();
        void ClickWrongDot(int DotNumber);
        

        void CreateUser();
        void DragEnd();
        void DragNextDot(int DotNumber);
        void DragStart();
        void DragWhiteSpace();
        void DragWrongDot(int DotNumber);
        void GameEnd(string name);
        void GameStart(string name);
        void HintDot(int DotNumber);
        void OnApplicationQuit();
        

        void SectionEnd(string name);
        void SectionStart(string name);
        void SessionEnd();
        void SessionPause();
        void SessionResume();
        void SessionStart();
        void StashData();
        void Update();
        void UploadData();
        string UserId { get; }
    }
}
