using System.ComponentModel.DataAnnotations;

namespace AllupMVCProject.Areas.Admin.ViewModels;

public class AdminViewModel
{
    [DataType(DataType.Text)]
    [StringLength(25)]
    public string UserName { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
