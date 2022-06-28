using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FunctionsRestController
{
    public class UsersContoller
    {
        private readonly UserRepository _userRepository;

        public UsersContoller(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [FunctionName("Post")]
        public async Task<IActionResult> Post(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Users")] HttpRequest req,
            ILogger log)
        {
            UserModel result = default;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var user = JsonConvert.DeserializeObject<UserModel>(requestBody);
            if (user is null)
            {
                return new BadRequestObjectResult("User was null");
            }
            result = await _userRepository.AddUser(user);

            return new OkObjectResult(result);
        }
        [FunctionName("Put")]
        public async Task<IActionResult> Put(
           [HttpTrigger(AuthorizationLevel.Function, "put", Route = "Users/{id:int}")] HttpRequest req, int id,
           ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var user = JsonConvert.DeserializeObject<UserModel>(requestBody);
            log.LogInformation($"Put method was used to invoke the function ({req.Method})");
            await _userRepository.UpdateUser(user);
            return new OkResult();
        }
        [FunctionName("Delete")]
        public async Task<IActionResult> Delete(
           [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Users/{id:int}")] HttpRequest req, int id,
           ILogger log)
        {

            await _userRepository.Delete(id);
            return new OkResult();
        }
        [FunctionName("Get")]
        public async Task<IActionResult> Get(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Users")] HttpRequest req,
           ILogger log)
        {
            return new OkObjectResult(await _userRepository.GetUsers());
        }

        [FunctionName("GetById")]
        public async Task<IActionResult> GetById(
          [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Users/{id:int}")] HttpRequest req, int id,
          ILogger log)
        {
            var result = await _userRepository.GetUserById(id);
            if (result.Id == 0) return new NotFoundResult();
            return new OkObjectResult(result);
        }



        //[FunctionName("Users")]
        //public async Task<IActionResult> Run(
        //    [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put","delete", Route = "Users/{id:int?}")] HttpRequest req, int? id,
        //    ILogger log)
        //{
        //    log.LogInformation($"User Controller processed a {req.Method} request.");

        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    var user = JsonConvert.DeserializeObject<UserModel>(requestBody);

        //    if (req.Method != HttpMethod.Get.Method && (!id.HasValue && user is null)) return new BadRequestResult();



        //    if (req.Method == HttpMethod.Post.Method)
        //    {
        //        log.LogInformation($"POST method was used to invoke the function ({req.Method})");
        //        var result = await _userRepository.AddUser(user);
        //        return new OkObjectResult(result);
        //    }
        //    else if (req.Method == HttpMethod.Put.Method)
        //    {
        //        log.LogInformation($"Put method was used to invoke the function ({req.Method})");
        //        await _userRepository.UpdateUser(user);
        //        return new OkResult();
        //    }
        //    else if (req.Method == HttpMethod.Delete.Method)
        //    {
        //        log.LogInformation($"Delete method was used to invoke the function ({req.Method})");
        //        if (id.HasValue)
        //        {
        //            await _userRepository.Delete(id.Value);
        //            return new OkResult();
        //        }
        //        else
        //        {
        //            await _userRepository.Delete(user.Id);
        //            return new OkResult();
        //        }
        //    }
        //    else if (req.Method == HttpMethod.Get.Method)
        //    {
        //        log.LogInformation($"GET method was used to invoke the function ({req.Method})");
        //        if(id.HasValue)
        //        {
        //            var result = await _userRepository.GetUserById(id.Value);
        //            if (result.Id == 0) return new NotFoundResult();
        //            return new OkObjectResult(result);
        //        }
        //        else
        //        {
        //            return new OkObjectResult(await _userRepository.GetUsers());
        //        }
        //    }
        //    else
        //    {
        //        log.LogInformation($"method was ({req.Method})");
        //    }

        //    return new OkObjectResult("");
        //}
    }
}
