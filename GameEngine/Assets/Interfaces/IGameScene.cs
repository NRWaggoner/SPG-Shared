using UnityEngine;
using System;
using System.Collections;

namespace AuroraEndeavors.GameEngine
{
    public delegate void GameFinishedEventHandler(object sender, EventArgs e);

    public interface IRootableObject
    {
        void Initialize(GameObject parentObject);
    }


    public interface IGameObject : IRootableObject
    {
        // When an AE GameObject is created, it should initially be hidden.  This
        // method indicates that the GameManager is ready to show the object.  Typicall,
        // the GameObject does this by calling SetActive(False).
        //
        void Show();

        // This is an indicator that the GameEngine is ready to hide the object.
        // The GameObject should hide itself -- typically with SetActive(False).  
        //
        void Hide();
    }


    public interface IGameScene : IGameObject
    {
        // Event should be fired when the scene is complete and
        // the GameManager can transition to a new scene.
        //
        event GameFinishedEventHandler GameFinished;

        // This is the backing GameObject that is the at the root
        // of the scene.  
        //
        GameObject GameObject { get; }

        // Should return true when the object is being shown,
        // and after Begin has been called.
        //
        bool IsPlaying { get; }

        // Some game transitions need to show the scene prior to the logic
        // running.  For example, there is a curtain effect where a curtain
        // reveals the next scene.  The scene logic should start executing
        // until the curtain is hidden and gone.
        //
        void Begin();

        // After the scene has been played through, transitioned away from, and 
        // typically hidden; the GameEngine will ask the object to clean itself 
        // up.
        //
        void Destroy();
    }


}