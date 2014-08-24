using UnityEngine;
using System;
using System.Collections;
using AuroraEndeavors.Utilities;

namespace AuroraEndeavors.GameEngine
{
    public class CGameSettings : IGameSettings
    {
        #region Statics - Singleton Model
        public static CGameSettings Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new CGameSettings();
                return s_instance;
            }
        }
        private static CGameSettings s_instance = null;
        #endregion

        private CGameSettings()
        { }


        public void Initialize(string UserId)
        {
            string key = "";
            string temp = "";

            m_userId = UserId;


            m_dataCache = CGameManager.GameDevice.GetDataCacheManager();

            //
            // Restore GameDifficulty
            //
            key = getSettingKey(SettingType.Difficulty);
            temp = m_dataCache.GetString(key, GameDifficulty.Easy.ToString());
            m_difficulty = (GameDifficulty)Enum.Parse(typeof(GameDifficulty), temp);


            //
            // Restore SFX Volume
            //
            key = getSettingKey(SettingType.SFXVolume);
            m_SFXVolume = m_dataCache.GetFloat(key, .23f);

            //
            // Restore Music Volume
            //
            key = getSettingKey(SettingType.MusicVolume);
            m_musicVolume = m_dataCache.GetFloat(key, .82f);

            fireChangedEvent(SettingType.All);
        }


        public event GameSettingsChangedEventHandler GameSettingsChanged;
        private void fireChangedEvent(SettingType type)
        {
            if (GameSettingsChanged != null)
                GameSettingsChanged(this, type);
        }

        //
        // 
        //
        private string m_userId = "Default";
        public string UserId
        {
            get { return m_userId; }
            set
            {
                m_userId = value;
                fireChangedEvent(SettingType.UserId);
            }
        }



        public GameDifficulty Difficulty
        {
            get { return m_difficulty; }
            set
            {
                m_difficulty = value;
                m_dataCache.SetString(getSettingKey(SettingType.Difficulty), value.ToString());
                fireChangedEvent(SettingType.Difficulty);
            }
        }
        private GameDifficulty m_difficulty = GameDifficulty.Easy;

        public float MusicVolume
        {
            get { return m_musicVolume; }
            set
            {
                m_musicVolume = value;
                m_dataCache.SetFloat(getSettingKey(SettingType.MusicVolume), value);
                fireChangedEvent(SettingType.MusicVolume);
            }
        }
        private float m_musicVolume/* = .73f*/;

        public float SFXVolume
        {
            get { return m_SFXVolume; }
            set
            {
                m_SFXVolume = value;
                m_dataCache.SetFloat(getSettingKey(SettingType.SFXVolume), value);
                fireChangedEvent(SettingType.SFXVolume);
            }
        }
        private float m_SFXVolume/* = .92f */;



        public float ComputeDotScale(float CameraSize)
        {
            float tempScale = 5 / 1.2f;
            if (isPhone)
                tempScale = 5 / 1.8f;

            tempScale = CameraSize / tempScale;
            return tempScale;
        }

        public Vector2 GetScreenResolution()
        {
            return new Vector2(Screen.width, Screen.height);
        }



        public string GetDimensionString()
        {
            float aspect = CGameSettings.Instance.GetAspectRatio();
            if (UnityHelpers.compareFloats(aspect, 2048f / 1536f, .001f))
                return "2048x1536";
            else if (UnityHelpers.compareFloats(aspect, 960f / 640f, .001f))
                return "960x640";
            else if (UnityHelpers.compareFloats(aspect, 1136f / 640f, .001f))
                return "1136x640";
            else
            {
                Debug.LogWarning("Screen resolution was not detected based on aspect ratio.  Returning iPad dimensions.");
                return "2048x1536";
            }
        }

        public float GetAspectRatio()
        {
            float aspectRatio = (float)Screen.width / (float)Screen.height;
            if (aspectRatio != Camera.main.aspect)
                Debug.LogWarning("Screen calculation of aspect ratio(" + aspectRatio + ") is off from camera aspect ratio(" + Camera.main.aspect + ").");

            return aspectRatio;
        }


        public bool IsInputCaptured()
        {   
            bool retVal = false;
            lock (s_lock)
            {
                retVal = m_requestor != null;
            }
            return retVal;
        }

        public bool CaptureInput(System.Object requestor)
        {
            bool retVal = false;
            lock (s_lock)
            {
                if (m_requestor == null)
                {
                    m_requestor = requestor;
                    retVal = true;
                }
                else if (m_requestor == requestor)
                    retVal = true;
            }
            return retVal;
        }

        public void ReleaseInput(System.Object requestor)
        {
            lock (s_lock)
            {
                if (m_requestor == requestor)
                    m_requestor = null;
            }
        }
        private static System.Object s_lock = new System.Object();
        private System.Object m_requestor = null;







        public DateTime FirstRunDate
        {
            get
            {
                if (m_firstRunDate == DateTime.MaxValue)
                {
                    string temp = m_dataCache.GetString(getSettingKey(SettingType.FirstRunDate), DateTime.Now.ToString());
                    m_firstRunDate = DateTime.Parse(temp);
                }
                return m_firstRunDate;
            }
        }
        private DateTime m_firstRunDate = DateTime.MaxValue;

        public int DaysFromFirstRun
        {
            get { return (DateTime.Now - (new DateTime(FirstRunDate.Year, FirstRunDate.Month, FirstRunDate.Day))).Days; }
        }


        private DateTime LastRunDate
        {
            get
            {
                if (m_lastRunDate == DateTime.MaxValue)
                {
                    string temp = m_dataCache.GetString(getSettingKey(SettingType.LastRunDate), DateTime.Now.ToString());
                    m_lastRunDate = DateTime.Parse(temp);
                    m_dataCache.SetString(getSettingKey(SettingType.LastRunDate), DateTime.Now.ToString());
                }
                return m_lastRunDate;
            }
        }
        private DateTime m_lastRunDate = DateTime.MaxValue;

        public int DaysFromLastRun
        {
            get { return (DateTime.Now - (new DateTime(LastRunDate.Year, LastRunDate.Month, LastRunDate.Day))).Days; }
        }




        private bool isPhone
        {
            get
            {
                if (Screen.dpi != 0)
                {
                    float width = Screen.width / Screen.dpi;
                    float height = Screen.height / Screen.dpi;
                    double diagonal = Math.Sqrt(width * width + height * height);

                    if (diagonal <= 7d)
                        return true;
                }
                return false;
            }
        }

        private string getSettingKey(SettingType type)
        {
            string key = "GameSetting_" + m_userId + "_" + type;
            return key;
        }

        private IDataCacheManager m_dataCache = null;
    }
}