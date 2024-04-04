namespace AllupMVCProject.Utilities.Exceptions
{
    public class NotFoundException:Exception
    {
        public string _PropertyName { get; set; }
        public NotFoundException()
        {
            
        }
        public NotFoundException(string message):base(message) 
        {
            
        }
        public NotFoundException(string PropertyName,string message):base(message)
        {
            _PropertyName = PropertyName;
        }
    }
}
