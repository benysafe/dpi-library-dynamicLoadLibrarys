using InterfacePrint;
namespace ShowText
{
    public class ShowText : IPrint
    {
        public string PrintText()
        {
            return ("String from inside library loaded dynamicaly");
        }
    }
}