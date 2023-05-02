using JsonFlatFileDataStore;
using Microsoft.AspNetCore.Mvc;
using  testapi.Models;

namespace testapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{   

    private readonly ILogger<UsersController> _logger; 
   private readonly IDocumentCollection<UserModel> _users;


    public UsersController(ILogger<UsersController> logger)
    {
        _logger = logger;
        var store = new  DataStore("db.json");
        _users = store.GetCollection<UserModel>();
    }   

   [HttpPost]
    public IActionResult Post([FromBody] UserModel user){
         var haveUser = _users.AsQueryable().FirstOrDefault(u=>u.Id == user.Id);

         if(haveUser != null)
           return Conflict();
         
        _users.InsertOne(user);
        return Ok(user);
    }
   
    [HttpGet]
    public IEnumerable<UserModel> Get(){
      return _users.AsQueryable().ToList();
    }

     [HttpGet("{id:int}")]
    public IActionResult Get(int id){
      var haveUser = _users.AsQueryable().FirstOrDefault(u=>u.Id == id);
      if(haveUser == null)
         return NotFound();

      return Ok(haveUser);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id,[FromBody] UserModel user){
     
     var haveUser = _users.AsQueryable().FirstOrDefault(u=>u.Id == id);
     if(haveUser == null)
         return NotFound();
   
      user.Id = id;
     _users.UpdateOne(x=>x.Id == haveUser.Id,user);
       return Ok(user);
    } 

}
