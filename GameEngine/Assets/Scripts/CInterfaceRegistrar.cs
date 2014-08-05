using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraEndeavors.GameEngine
{
    public class CInterfaceRegistrar
    {
        public static CInterfaceRegistrar Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new CInterfaceRegistrar();
                return s_instance;
            }
        }
        private static CInterfaceRegistrar s_instance = null;


        public void RegisterInstance<T>(Object instance)
        {
            if (m_instanceMap.ContainsKey(typeof(T)))
                throw new ArgumentException("Type(" + typeof(T).Name + ") has already been registered.");
            if (instance == null)
                throw new ArgumentException("Cannot register a null object.");

            m_instanceMap.Add(typeof(T), instance);
        }

        public bool IsRegistered<T>()
        {
            return m_instanceMap.ContainsKey(typeof(T));
        }
        
        public T GetInstance<T>()
        {
            if (!m_instanceMap.ContainsKey(typeof(T)))
                throw new ArgumentException("Type(" + typeof(T).Name + ") has never been registered.");

            return (T)m_instanceMap[typeof(T)];
        }




        private CInterfaceRegistrar()
        { }

        private Dictionary<Type, Object> m_instanceMap = new Dictionary<Type, object>();



    }
}
