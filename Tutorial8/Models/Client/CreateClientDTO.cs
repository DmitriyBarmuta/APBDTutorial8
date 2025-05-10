namespace Tutorial8.Models.Client;

using System.ComponentModel.DataAnnotations;

public class CreateClientDTO
{
    [Required]
    [StringLength(120)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(120)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Phone]
    public string Telephone { get; set; }

    [StringLength(11, MinimumLength = 11, ErrorMessage = "PESEL must be 11 characters")]
    public string Pesel { get; set; }
}