namespace AllupMVCProject.Utilities.Exceptions
{
    public class SizeOfFileException:Exception
    {
        public string _PropertName {  get; set; }
        public SizeOfFileException()
        {
            
        }
        public SizeOfFileException(string message):base(message)
        {
            
        }
        public SizeOfFileException(string PropertyName,string message):base(message)
        {
            _PropertName = PropertyName;
        }
    }
}
