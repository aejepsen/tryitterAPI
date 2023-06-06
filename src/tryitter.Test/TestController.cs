using System.Net.Http.Headers;
using System.Text;
using tryitter.Services;
using tryitter.Models;
using FluentAssertions;
using System.Net;

namespace tryitter.Test;

public class TestController : IClassFixture<TestTryitterContext<Program>>
{
  private readonly HttpClient _client;
  public TestController(TestTryitterContext<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task CreateStudent()
  {

    var student = new Student 
    { 
      Name = "maria aparecida", 
      Email = "cida@gmail.com", 
      Password = "12345678", 
      Status = "Aceleração C#" 
    };

    //Create Student  id:5
    
    var result = await _client.PostAsync(
      "/Student", 
      new StringContent(
        "{\"name\":\"maria aparecida\", \"email\":\"cida@gmail.com\",\"password\":\"12345678\",\"status\":\"Aceleração C#\"}",
        Encoding.UTF8, 
        "application/json")
      );

    result.StatusCode.Should().Be((HttpStatusCode)200);

    result.Content.ReadAsStringAsync().Result.Should().Be("student created");

    //Delete Student in inMemory DB

    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
      "Bearer", 
      new TokenGenerator().Generate(student)
    );
    await _client.DeleteAsync("/Student/5");
  }

  [Fact]
  public async Task CreateStudentFail()
  {
   
    var stringContent = new StringContent(
      "{\"name\":\"antonio\",\"email\":\"antonio@gmail.com\",\"password\":\"12345678\",\"status\":\"Aceleração C#\"}", 
      Encoding.UTF8, 
      "application/json"
    );

    var result = await _client.PostAsync("/Student", stringContent);    

    result.StatusCode.Should().Be((HttpStatusCode)400);
    result.Content.ReadAsStringAsync().Result.Should().Be("Email already exists");
  }

  [Fact]
  public async Task LoginStudent()
  {
    var student = new Student 
    { 
      Name = "pedro", 
      Email = "pedro@gmail.com", 
      Password = "12345678", 
      Status = "Aceleração C#" 
    };

    //Create Student  id:6

    await _client.PostAsync(
        "/Student", 
        new StringContent(
          "{\"name\":\"pedro\",\"email\":\"pedro@gmail.com\",\"password\":\"12345678\",\"status\":\"Aceleração C#\"}", 
          Encoding.UTF8, 
          "application/json")
      );

    //login

    var stringContent = new StringContent(
      "{\"email\":\"pedro@gmail.com\",\"password\":\"12345678\"}",
      Encoding.UTF8, 
      "application/json"
      );

    var login = await _client.PostAsync("/Login", stringContent);

    login.StatusCode.Should().Be((HttpStatusCode)200);

    login.Content.ReadAsStringAsync().Result.Should().NotBeEmpty();

    //Delete Student in inMemory DB 

    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
      "Bearer", 
      new TokenGenerator().Generate(student)
      );

    await _client.DeleteAsync("/Student/6");

  }

  [Fact]
  public async Task LoginStudentFail()
  {
    //login

    var stringContent = new StringContent(
      "{\"email\":\"miguel@gmail\",\"password\":\"123456\"}", 
      Encoding.UTF8, 
      "application/json"
    );

    var login = await _client.PostAsync("/Login", stringContent);

    login.StatusCode.Should().Be((HttpStatusCode)400);
    login.Content.ReadAsStringAsync().Result.Should().Be("Student not found");
  }

  [Fact]
  public async Task GetAllStudents()
  {
    //login
    var result = await _client.GetAsync("/Student");

    result.StatusCode.Should().Be((HttpStatusCode)200);
    result.Content.ReadAsStringAsync().Result.Should().Be("[{\"studentId\":1,\"name\":\"allan\",\"email\":\"aejepsen@yahoo.com.br\",\"status\":\"Aceleração C#\"},{\"studentId\":2,\"name\":\"pedro paulo\",\"email\":\"pedro_paulo@gmail.com\",\"status\":\"Aceleração C#\"},{\"studentId\":3,\"name\":\"antonio\",\"email\":\"antonio@gmail.com\",\"status\":\"Aceleração C#\"},{\"studentId\":4,\"name\":\"vitor\",\"email\":\"vitor@gmail.com\",\"status\":\"Aceleração C#\"}]");
  }

  [Fact]
  public async Task GetStudentById()
  {

    var result = await _client.GetAsync("Student/1");

    result.StatusCode.Should().Be((HttpStatusCode)200);
    result.Content.ReadAsStringAsync().Result.Should().Be("{\"studentId\":1,\"name\":\"allan\",\"email\":\"aejepsen@yahoo.com.br\",\"status\":\"Aceleração C#\"}");
  }

  [Fact]
  public async Task DeleteStudentFail()
  {
    //DeleteStudent
    var result = await _client.DeleteAsync("Student/1");
    result.StatusCode.Should().Be((HttpStatusCode)401);
  }

  [Fact]
  public async Task UpdateStudent()
  {
    var student = new Student { 
      Name = "allan eric",
      Email = "aejepsen@yahoo.com.br", 
      Password = "12345678", 
      Status = "Aceleração C#" 
    };

    //UpdateStudent

    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
      "Bearer", 
      new TokenGenerator().Generate(student)
    );

    var result = await _client.PutAsync(
      "Student/1", 
      new StringContent(
        "{\"name\":\"allan eric\",\"email\":\"aejepsen@yahoo.com.br\",\"password\":\"12345678\",\"status\":\"Aceleração C#\"}", 
        Encoding.UTF8, 
        "application/json")
      );

    result.StatusCode.Should().Be((HttpStatusCode)200);
    result.Content.ReadAsStringAsync().Result.Should().Be("Student updated");
  }

  [Fact]
  public async Task UpdateStudentWithoutToken()
  {
    //UpdateStudent

    var stringContent = new StringContent(
      "{\"name\":\"allan eric\",\"email\":\"aejepsen@yahoo.com.br\",\"password\":\"12345678\",\"status\":\"Aceleração C#\"}", Encoding.UTF8, "application/json");

    var resultUpdateStudent = await _client.PutAsync("Student/1", stringContent);
    resultUpdateStudent.StatusCode.Should().Be((HttpStatusCode)401);
  }

  [Fact]
  public async Task CreatePost()
  {
    var student = new Student { 
      Name = "allan", 
      Email = "aejepsen@yahoo.com.br", 
      Password = "12345678", 
      Status = "Aceleração C#" 
    };

    //Create Post

    var stringContent = new StringContent(
      "{\"content\":\"postagem\",\"image\":\"string\",\"studentEmail\":\"aejepsen@yahoo.com.br\"}", 
      Encoding.UTF8, 
      "application/json"
    );
    //token for request

    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
      "Bearer", 
      new TokenGenerator().Generate(student)
    );

    var result = await _client.PostAsync(
      "/Post", 
      stringContent);

    result.StatusCode.Should().Be((HttpStatusCode)200);
    result.Content.ReadAsStringAsync().Result.Should().Be("Post Created");

    //Delete Post in inMemory DB
    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
      "Bearer", 
      new TokenGenerator().Generate(student)
    );

    var request = new HttpRequestMessage
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri("/Post/6"),
      Content = new StringContent(
        "{\"studentEmail\":\"aejepsen@yahoo.com.br\"}", 
        Encoding.UTF8, 
        "application/json"),
    };
    await _client.SendAsync(request).ConfigureAwait(false);
  }

  [Fact]
  public async Task UpdatePost()
  {
    var student = new Student { 
      Name = "allan", 
      Email = "aejepsen@yahoo.com.br", 
      Password = "12345678", 
      Status = "Aceleração C#" 
    };
    //Update Post

    var stringContent = new StringContent(
      "{\"content\":\"Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.-1\",\"image\":\"string\",\"studentEmail\":\"aejepsen@yahoo.com.br\"}", 
      Encoding.UTF8, 
      "application/json"
    );

    //token for request

    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
      "Bearer", 
      new TokenGenerator().Generate(student)
    );

    var result = await _client.PutAsync("/Post/1", stringContent);

    result.StatusCode.Should().Be((HttpStatusCode)200);
    result.Content.ReadAsStringAsync().Result.Should().Be("Post updated");
  }

  [Fact]
  public async Task UpdatePostWithoutToken()
  {
    //Update Post

    var stringContent = new StringContent(
      "{\"content\":\"Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.-1\",\"image\":\"string\",\"studentEmail\":\"aejepsen@yahoo.com.br\"}", 
      Encoding.UTF8, 
      "application/json"
    );

    var result = await _client.PutAsync("/Post/1", stringContent);
    result.StatusCode.Should().Be((HttpStatusCode)401);
  }

  [Fact]
  public async Task DeletePost()
  {
    var student = new Student { 
      Name = "pedro paulo", 
      Email = "pedro_paulo@gmail.com", 
      Password = "12345678", 
      Status = "Aceleração C#" 
    };

    var request = new HttpRequestMessage
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri("/Post/3"),
      Content = new StringContent(
        "{\"studentEmail\":\"pedro_paulo@gmail.com\"}", 
        Encoding.UTF8, 
        "application/json"
      ),
    };
    //token for request

    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
      "Bearer", 
      new TokenGenerator().Generate(student)
    );

    //Delete Post

    var result = await _client.SendAsync(request).ConfigureAwait(false);

    result.StatusCode.Should().Be((HttpStatusCode)200);
    result.Content.ReadAsStringAsync().Result.Should().Be("Post deleted");
  }

  [Fact]
  public async Task DeletePostWithoutToken()
  {
    //Delete Post
    var request = new HttpRequestMessage
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri("/Post/3"),
      Content = new StringContent(
        "{\"studentEmail\":\"pedro_paulo@gmail.com\"}", 
        Encoding.UTF8, 
        "application/json"
      ),
    };

    var result = await _client.SendAsync(request).ConfigureAwait(false);
    result.StatusCode.Should().Be((HttpStatusCode)401);
  }

  [Fact]
  public async Task GetPostById()
  {
    var result = await _client.GetAsync("Post/2");

    result.StatusCode.Should().Be((HttpStatusCode)200);
    result.Content.ReadAsStringAsync().Result.Should().Be("{\"postId\":2,\"content\":\"It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).\",\"creatAt\":\"2022-11-03T09:40:10\",\"updatetAt\":\"2022-12-01T10:37:22\",\"image\":null,\"studentId\":1}");
  }

  [Fact]
  public async Task GetAllPosts()
  {
    var result = await _client.GetAsync("Post");
    result.StatusCode.Should().Be((HttpStatusCode)200);
  }

  [Fact]
  public async Task GetPostsByStudentName()
  {
    var request = new HttpRequestMessage
    {
      Method = HttpMethod.Get,
      RequestUri = new Uri("/Post/StudentName"),
      Content = new StringContent(
        "{\"name\":\"antonio\"}", 
        Encoding.UTF8, 
        "application/json"
      ),
    };

    var result = await _client.SendAsync(request).ConfigureAwait(false);

    result.StatusCode.Should().Be((HttpStatusCode)200);
    result.Content.ReadAsStringAsync().Result.Should().Be("[{\"postId\":4,\"content\":\"There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.\",\"creatAt\":\"2023-01-03T10:45:10\",\"updatetAt\":\"2023-01-03T11:38:10\",\"image\":null,\"studentId\":3},{\"postId\":5,\"content\":\"The standard chunk of Lorem Ipsum used since the 1500s is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from de Finibus Bonorum et Malorum by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.\",\"creatAt\":\"2023-12-03T10:55:00\",\"updatetAt\":\"2023-12-06T08:38:07\",\"image\":null,\"studentId\":3}]");
  }
}
