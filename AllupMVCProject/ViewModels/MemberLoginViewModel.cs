using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace AllupMVCProject.ViewModels;

public class MemberLoginViewModel
{
    [DataType(DataType.Text)]
    [StringLength(25)]
    public string UserName { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
