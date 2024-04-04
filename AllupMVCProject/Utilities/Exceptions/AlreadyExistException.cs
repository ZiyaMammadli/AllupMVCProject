namespace AllupMVCProject.Utilities.Exceptions
{
    public class AlreadyExistException:Exception
    {
        public string _PropertyName {  get; set; }  
        public AlreadyExistException()
        {
            
        }
        public AlreadyExistException(string message):base(message)
        {
            
        }
        public AlreadyExistException(string PropertyName, string message):base(message)
        {
            _PropertyName = PropertyName;
        }
    }
}
