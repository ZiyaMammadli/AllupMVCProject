namespace AllupMVCProject.Utilities.Exceptions;

public class InvalidContentTypeException:Exception
{
    public string _PropertyName {  get; set; }  
    public InvalidContentTypeException() { }

    public InvalidContentTypeException(string message):base(message) { }

    public InvalidContentTypeException(string PropertyName,string message):base(message) 
    {
        _PropertyName = PropertyName;
    }    
  
}
