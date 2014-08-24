using UnityEngine;
using System;
using System.Collections;

namespace AuroraEndeavors.GameEngine
{
    public enum GameDifficulty
    {
        Easy     = 1,
        Medium   = 2,
        Hard     = 3,
        VeryHard = 4
    }

    public enum SettingType
    {
        UserId,
        Difficulty,
        MusicVolume,
        SFXVolume,
        All,
        FirstRunDate,
        LastRunDate
    }

    public delegate void GameSettingsChangedEventHandler(object sender, SettingType e);

    public interface IGameSettings
    {
        event GameSettingsChangedEventHandler GameSettingsChanged;

        GameDifficulty Difficulty
        { get; }
                
        String UserId
        { get; }

        float MusicVolume
        { get; }

        float SFXVolume
        { get; }

        DateTime FirstRunDate
        { get; }

        bool IsInputCaptured();
        bool CaptureInput(System.Object requestor);
        void ReleaseInput(System.Object requestor);
    }
}