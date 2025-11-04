using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstRestProject.Controllers
{
    //[Route("api/[controller]")]

    [Route("api/MyFirstAPI")]
    [ApiController]
    public class MyFirstAPIController : ControllerBase
    {
        [HttpGet("MyName",Name = "MyName" )]
        public string GetMyName()
        {
            return "My Name Aziza";
        }

        [HttpGet("YourName", Name = "YourName")]
        public string GetYourName()
        {
            return "Your Name Ali";
        }

        [HttpGet("Sum{Num1}/ {Num2}")]
        public int Sum2Num(int Num1, int Num2)
        {
            return Num1 + Num2;
        }


        [HttpGet("Multi{Num1}/ {Num2}")]
        public int Multi2Num(int Num1, int Num2)
        {
            return Num1 + Num2;
        }
    }
}
