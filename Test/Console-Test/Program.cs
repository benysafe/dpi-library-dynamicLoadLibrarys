using DynamicLoad;
using InterfacePrint;

internal class Program
{
    // Full path of library to load dynamically
    const string pathLibrary = "...\\dpi-library-dynamicLoadLibrarys\\Test\\ShowText\\bin\\Debug\\net6.0\\ShowText.dll";
    private static void Main(string[] args)
    {
        //Load ShowText library an creat a object 'IPrint' 
        IPrint printTest = DynamicLoad.DynamicLoad.Assembly_Load_method<IPrint>(pathLibrary);
        //Print in console the string conteined on ShowText library
        Console.WriteLine(printTest.PrintText());
        //Wait for press any key to close
        Console.ReadLine();
    }
}