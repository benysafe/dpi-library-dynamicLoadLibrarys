using System.Reflection;

namespace DinamicLoad
{
    public class DinamicLoad
    {
        public static T Assembly_Load_method<T>(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }
            //Obtiene los ensamblados cargados dentro del dominio de aplicacion del hilo actual
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            //obtener el ensamblado de la biblioteca cargada y agrega al dominio de aplicacion del hilo actual 
            Assembly assemblyToLoad = Assembly.LoadFrom(Path.GetFullPath(path));
            AppDomain.CurrentDomain.Load(assemblyToLoad.GetName());

            List<Type> typesExported = assemblyToLoad.GetExportedTypes().ToList();
            if (!typesExported.Any(eleTypes => eleTypes.GetTypeInfo().GetInterfaces().ToList().Any(eleInter => eleInter.FullName.Equals(typeof(T).FullName))))
            {
                throw new Exception($"no se encontro implementacion de la interfas '{typeof(T).FullName}', en el ensamblado {path}");
            }

            string typeFullName = typesExported.Find(eleTypes => eleTypes.GetTypeInfo().GetInterfaces().ToList().Any(eleInter => eleInter.FullName.Equals(typeof(T).FullName))).FullName;

            LoadReferencedAssemblies(assemblyToLoad);
            // Retorna una nueva instancia de <T> dado su nombre
            return (T)Activator.CreateInstance(assemblyToLoad.GetType(typeFullName));
        }

        private static void LoadReferencedAssemblies(Assembly assemblyToLoad)
        {
            AssemblyName[] referencedAssemblies = assemblyToLoad.GetReferencedAssemblies();
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (AssemblyName currentAssemblyName in referencedAssemblies)
            {
                var loadedAssembly = loadedAssemblies.FirstOrDefault(loadedAssembly => loadedAssembly.FullName == currentAssemblyName.FullName);

                if (loadedAssembly == null)
                {
                    Assembly referencedAssembly = null;
                    try
                    {
                        //First try to load using the assembly name just in case its a system dll    
                        referencedAssembly = Assembly.Load(currentAssemblyName);
                    }
                    catch (FileNotFoundException ex)
                    {
                        try
                        {
                            referencedAssembly = Assembly.LoadFrom(Path.Join(Path.GetDirectoryName(assemblyToLoad.Location), currentAssemblyName.Name + ".dll"));
                        }
                        catch (Exception)
                        {
                        }
                    }

                    if (referencedAssembly != null)
                    {
                        LoadReferencedAssemblies(referencedAssembly);
                        AppDomain.CurrentDomain.Load(referencedAssembly.GetName());
                    }
                }
            }
        }

    }
}