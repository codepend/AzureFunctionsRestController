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



















        [FunctionName("Users")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put","delete", Route = "Users/{id:int?}")] HttpRequest req, int? id,
            ILogger log)
        {
            log.LogInformation($"User Controller processed a {req.Method} request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var user = JsonConvert.DeserializeObject<UserModel>(requestBody);

            if (req.Method != HttpMethod.Get.Method && (!id.HasValue && user is null)) return new BadRequestResult();



            if (req.Method == HttpMethod.Post.Method)
            {
                log.LogInformation($"POST method was used to invoke the function ({req.Method})");
                var result = await _userRepository.AddUser(user);
                return new OkObjectResult(result);
            }
            else if (req.Method == HttpMethod.Put.Method)
            {
                log.LogInformation($"Put method was used to invoke the function ({req.Method})");
                await _userRepository.UpdateUser(user);
                return new OkResult();
            }
            else if (req.Method == HttpMethod.Delete.Method)
            {
                log.LogInformation($"Delete method was used to invoke the function ({req.Method})");
                if (id.HasValue)
                {
                    await _userRepository.Delete(id.Value);
                    return new OkResult();
                }
                else
                {
                    await _userRepository.Delete(user.Id);
                    return new OkResult();
                }
            }
            else if (req.Method == HttpMethod.Get.Method)
            {
                log.LogInformation($"GET method was used to invoke the function ({req.Method})");
                if(id.HasValue)
                {
                    var result = await _userRepository.GetUserById(id.Value);
                    if (result.Id == 0) return new NotFoundResult();
                    return new OkObjectResult(result);
                }
                else
                {
                    return new OkObjectResult(await _userRepository.GetUsers());
                }
            }
            else
            {
                log.LogInformation($"method was ({req.Method})");
            }

            return new OkObjectResult("");
        }
    }
}
