namespace AllupMVCProject.Utilities.Exceptions;

public class RequiredPropertyException:Exception
{
    public string _PropertyName {  get; set; }  
    public RequiredPropertyException()
    {
        
    }
    public RequiredPropertyException(string message):base(message)
    {
        
    }
    public RequiredPropertyException(string PropertyName,string message):base(message)
    {
        _PropertyName = PropertyName;
    }
}
