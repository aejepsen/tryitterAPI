using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ok
namespace tryitter.Models;
public class Post
{
  [Key]
  public int PostId { get; set; }
  [MaxLength(300, ErrorMessage = "Content precisa ter no maximo 300 caracteres")]
  public string Content { get; set; } = default!;
  public DateTime CreatAt { get; set; }
  public DateTime UpdatetAt { get; set; }
  public string? Image { get; set; }
  [ForeignKey("StudentId")]
  public int StudentId { get; set; }
  public Student? Student { get; set; }

}

public class PostResponse
{
  public int PostId { get; set; }
  public string Content { get; set; } = default!;
  public DateTime? CreatAt { get; set; }
  public DateTime UpdatetAt { get; set; }
  public string? Image { get; set; }
  [ForeignKey("StudentId")]
  public int? StudentId { get; set; }

  public PostResponse(int postId, string content, DateTime creatAt, DateTime updatetAt, string image, int studentId)
  {
    PostId = postId;
    Content = content;
    CreatAt = creatAt;
    UpdatetAt = updatetAt;
    Image = image;
    StudentId = studentId;
  }

}
public class PostRequest
{
  public string Content { get; set; }
  public string? Image { get; set; }
  public string StudentEmail { get; set; }
}