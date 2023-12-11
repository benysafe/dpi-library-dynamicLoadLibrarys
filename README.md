# dpi-library-dynamicLoadLibrarys
Function library to perform dynamic loading of libraries from the local path of the provided file; Additionally, it finds and loads all dependencies used by the loaded library.

The loaded library should inherit from a interface, that interface should be include on your source project and used to get a instance object with calling the function:

     T Assembly_Load_method<T>(string path)
    
This function need as parameter the full path of library to load and 'T' shuld be sustituted by the interface name instanced.

For more detail check the Console-Test project.
