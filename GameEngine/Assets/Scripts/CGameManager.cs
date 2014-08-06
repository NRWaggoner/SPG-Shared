using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace AuroraEndeavors.GameEngine
{

    public class CGameManager : MonoBehaviour, ICoordinateTransitions
    {

        #region Statics
        public static void Initialize(IGameDevice device)
        {
            s_gameDevice = device;
        }

        private static IGameDevice s_gameDevice = null;

        public static void initializeProducts()
        {
            if (s_initializingProducts)
                return;

            IDataCacheManager dataCache = s_gameDevice.GetDataCacheManager();

            s_initializingProducts = true;

            if (s_sceneList != null)
                s_sceneList.Clear();

            //
            // Generate the list of products... eventually,
            // this should be replaced by a call to the 
            // web-service.
            //
            s_products = new List<IInAppProduct>()
                {
                    s_gameDevice.GetProduct("vuBDb3UeuT"),
                    s_gameDevice.GetProduct("MfJDQYprYJ"),
                    s_gameDevice.GetProduct("McCdr6eS0J"),
                    s_gameDevice.GetProduct("lQ5JMb3eV0")
                };


            //
            // Following section of code picks a random scene from the 
            // 4 above to make free on the first run and caches the result.
            // On subsequent runs, the cached value is simply used.
            //
            string cacheKey = "CGAMEMANGAER_FreeSceneId";
            string localId = dataCache.GetString(cacheKey, string.Empty);
            IInAppProduct bypassProduct = null;

            //
            // If the productId is empty, it is our first run.
            //
            if (String.IsNullOrEmpty(localId))
            {
                int index = UnityEngine.Random.Range(0, s_products.Count);
                bypassProduct = s_products[index];
                dataCache.SetString(cacheKey, s_products[index].LocalId);
            }
            else
            {
                foreach (IInAppProduct product in s_products)
                {
                    if (product.LocalId == localId)
                    {
                        bypassProduct = product;
                        break;
                    }
                }
            }
            bypassProduct.BypassPurchase();
            s_products.Remove(bypassProduct);

            s_gameDevice.GetCoRoutineRunner().RunCoRoutine(checkProducts);
        }
        private static bool s_initializingProducts = false;


        private static IEnumerator checkProducts(System.Object Obj)
        {
            List<IInAppProduct> unPurchasedProducts = new List<IInAppProduct>();
            while (s_products.Count > 0)
            {
                List<IInAppProduct> processedProducts = new List<IInAppProduct>();
                foreach (IInAppProduct product in s_products)
                {
                    if (product.IsInitialized)
                    {
                        if (product.IsPurachased == true)
                            AddScene(product.ResourcePath);
                        else if (product.IsPurachased == false)
                            unPurchasedProducts.Add(product);

                        if (product.IsPurachased != null)
                            processedProducts.Add(product);

                    }
                }
                foreach (IInAppProduct removeItem in processedProducts)
                    s_products.Remove(removeItem);


                yield return null;
            }
            s_products.AddRange(unPurchasedProducts);
            s_initializingProducts = false;
        }
        private static List<IInAppProduct> s_products;


        public static void AddScene(string path)
        {
            if (s_sceneList == null)
                s_sceneList = new List<String>();
            s_sceneList.Add(path);
            s_gameIndex = s_sceneList.Count - 1;
        }


        static IGameScene getNextScene()
        {
            if (s_sceneList == null || s_sceneList.Count < 1)
                throw new Exception("No Game Scenes to Play");
            
            IGameScene retVal = s_gameDevice.CreateGameScene(s_sceneList[s_gameIndex]);

            s_gameIndex++;
            if (s_gameIndex >= s_sceneList.Count)
                s_gameIndex = 0;

            return retVal;
        }
        private static int s_gameIndex = 0;

        static List<string> s_sceneList;
        
        static bool s_intialized = false;
        static List<CGameManager> s_gameManagers = new List<CGameManager>();
        
        #endregion

        public Camera BackgroundCamera;
        public GameObject MenuBackground;
        public GameObject SceneBackground;
        public List<IGameScene> Scenes;

        CGameManager()
        {
            s_gameManagers.Add(this);
        }
        public void OnGameSettingsChanged(object sender, SettingType type)
        {
            if (type == SettingType.MusicVolume || type == SettingType.All)
                m_audioSrc.volume = CGameSettings.Instance.MusicVolume * .4f;
        }

        void Awake()
        {
            if (s_instance != null)
                throw new ArgumentException("Can only instantiate a single CGameManager");
            s_instance = this;
        }

        public static CGameManager Instance
        { get { return s_instance; } }
        private static CGameManager s_instance = null;

        // Use this for initialization
        void Start()
        {
            //
            // Set up the music.
            // 
            m_audioSrc = GetComponent<AudioSource>();


            //
            // Initialize the settings as soon as all required objects are created.
            //
            //       Note, audioSrc must be created first as the event handler uses 
            //       is to store/set the actual volume.
            //
            CGameSettings.Instance.GameSettingsChanged += OnGameSettingsChanged;

            // Get the process of logging in and getting telemetry kicked off setup first.
            //
            m_telemetryMgr = s_gameDevice.GetTelemetryManager();
            CGameSettings.Instance.Initialize(m_telemetryMgr.UserId);




            // Initialize Billing
            //
            m_billing = s_gameDevice.GetBilling();
            m_billing.Initialize(this.gameObject);

            m_audioSrc.Play();
            if (!s_intialized)
            {

                s_intialized = true;




                initializeProducts();
            }







            IMainBackButton button = s_gameDevice.GetMainBackButton();
            button.Initialize(this.gameObject);
            button.ItemChanged += OnBackPressed;




            m_menu = s_gameDevice.CreateGameMenu();
            
            m_menu.MenuActivated += new GameEngine.OnMenuActivated(OnMenuActivated);
            m_menu.Show();

            //
            // First calculate the sizes for the menu
            //
            CalculateCameraViewport(CameraPositionType.Menu, .9f);

            //
            // Next calculate the sizes for the scene
            //
            if (Camera.main.aspect == 4f / 3f)
                CalculateCameraViewport(CameraPositionType.Scene, 1f);
            else
                CalculateCameraViewport(CameraPositionType.Scene, .96f);


            Camera.main.aspect = 4f / 3f;
            Camera.main.rect = new Rect(m_mainCameraLeft[CameraPositionType.Menu],
                                        m_mainCameraTop[CameraPositionType.Menu],
                                        m_mainCameraWidth[CameraPositionType.Menu],
                                        m_mainCameraHeight[CameraPositionType.Menu]);



            // Calculate the full size of the "inner view"
            //
            m_initialCameraPos = Camera.main.transform.position;
            m_verticalSize = Camera.main.orthographicSize * 2;
            m_horizontalSize = Camera.main.aspect * m_verticalSize;

            m_sceneTransitioner = s_gameDevice.GetSceneTransitioner();
            m_sceneTransitioner.Initialize(m_horizontalSize, m_verticalSize, this);

            m_woodenBackground = GameObject.CreatePrimitive(PrimitiveType.Quad);
            m_woodenBackground.name = "Background";
            m_woodenBackground.transform.localScale = new Vector3(m_horizontalSize, m_verticalSize, 0);
            m_woodenBackground.transform.position = new Vector3(0, 0, 5000);
            m_woodenBackground.renderer.material = Resources.Load<Material>("Menu/_res/" + "wood1");


            m_telemetryMgr.SessionStart();

            m_telemetryMgr.StashData();
        }





        private void CalculateCameraViewport(CameraPositionType type, float height)
        {
            m_mainCameraHeight[type] = height;
            float temp1 = m_mainCameraHeight[type] * 4f / 3f;
            m_mainCameraWidth[type] = temp1 / BackgroundCamera.aspect;

            m_mainCameraLeft[type] = (1f - m_mainCameraWidth[type]) / 2f;
            m_mainCameraTop[type] = (1f - m_mainCameraHeight[type]) / 2f;
        }

        private void initializeCurtain(float VerticleSize, float HorizontalSize)
        {

        }

        void OnApplicationQuit()
        {
            m_telemetryMgr.SessionEnd();
            m_telemetryMgr.StashData();
        }

        void OnApplicationPause()
        {
            if (isPaused)
                m_telemetryMgr.SessionResume();
            else
            {
                m_telemetryMgr.SessionPause();
                m_telemetryMgr.StashData();
            }
            isPaused = !isPaused;
        }
        private bool isPaused = false;

        public void ShowGame()
        {
            Camera.main.rect = new Rect(m_mainCameraLeft[CameraPositionType.Scene],
                             m_mainCameraTop[CameraPositionType.Scene],
                             m_mainCameraWidth[CameraPositionType.Scene],
                             m_mainCameraHeight[CameraPositionType.Scene]);

            MenuBackground.SetActive(false);
            SceneBackground.SetActive(true);
            m_menu.Hide();
            SwapInNextScene();
            m_currentGameScene.Show();
            m_currentGameScene.Begin();
        }

        private void OnBackPressed(System.Object sender)
        {
            ShowMenu();
        }
        public void ShowMenu()
        {
            Camera.main.rect = new Rect(m_mainCameraLeft[CameraPositionType.Menu],
                             m_mainCameraTop[CameraPositionType.Menu],
                             m_mainCameraWidth[CameraPositionType.Menu],
                             m_mainCameraHeight[CameraPositionType.Menu]);
            Camera.main.orthographicSize = m_verticalSize / 2;
            Camera.main.transform.position = m_initialCameraPos;

            MenuBackground.SetActive(true);
            SceneBackground.SetActive(false);
            m_menu.Show();
            m_sceneTransitioner.Hide();
            m_currentGameScene.Hide();
        }

        private void OnMenuActivated(object sender, MenuActions action)
        {
            switch (action)
            {
                case MenuActions.Play:
                    ShowGame();

                    break;
            }
        }

        private void OnGameFinished(object sender, EventArgs e)
        {
            m_sceneTransitioner.StartTransition();
        }
        

        public void SwapInNextScene()
        {
            //
            // Swap out the scenes
            //
            if (m_currentGameScene != null)
            {
                m_currentGameScene.Hide();
                m_currentGameScene.Destroy();
            }

            m_currentGameScene = getNextScene(); ;
            m_currentGameScene.GameFinished += new GameFinishedEventHandler(this.OnGameFinished);
            m_currentGameScene.Show();

        }

        public void StartNextScene()
        {
            m_currentGameScene.Begin();
        }



        public void PauseMusic()
        {
            if (m_audioSrc != null)
                m_audioSrc.Pause();
        }
        public void PlayMusic()
        {
            if (m_audioSrc != null)
                m_audioSrc.Play();
        }

        #region private variables
        private AudioSource m_audioSrc = null;
        private ITelemetryManager m_telemetryMgr = null;
        private IBilling m_billing = null;
        private ITransitionScenes m_sceneTransitioner = null;

        private Vector3 m_initialCameraPos;
        private float m_verticalSize;
        private float m_horizontalSize;
        


        private IGameMenu m_menu = null;

        IGameScene CurrentScene
        {
            get { return m_currentGameScene; }
            set
            {
                m_currentGameScene = value;
            }
        }
        IGameScene m_currentGameScene;



        //
        // Variables for the background
        //
        private GameObject m_woodenBackground = null;



        //
        // Variables controlling the target size for the main camera
        //
        enum CameraPositionType
        {
            Menu,
            Scene
        }

        Dictionary<CameraPositionType, float> m_mainCameraLeft = new Dictionary<CameraPositionType, float>(2);
        Dictionary<CameraPositionType, float> m_mainCameraTop = new Dictionary<CameraPositionType, float>(2);
        Dictionary<CameraPositionType, float> m_mainCameraHeight = new Dictionary<CameraPositionType, float>(2);
        Dictionary<CameraPositionType, float> m_mainCameraWidth = new Dictionary<CameraPositionType, float>(2);

        #endregion
    }
}