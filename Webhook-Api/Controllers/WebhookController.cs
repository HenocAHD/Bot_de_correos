using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Linkedin.Net.Db;
using System.Text.Json.Nodes;
using Webhook_Api.Models;

namespace Webhook_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        // POST api/<SendgridWebhook>
        [HttpPost]
        public void Post([FromBody] JsonArray value)
        {
            try
            {
                var vlue2 = value.ToString();
                var vlue3 = "{\"content\": " + vlue2 + "}";
                var webhookreceiver = JsonSerializer.Deserialize<WebhookList>(vlue3);

                foreach (var item in webhookreceiver.content)
                {
                    item.Create(database.getdatabase());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                NotFound();
            }


        }

        [HttpGet]
        public List<webhook> Get()
        {
            try
            {
                return webhook.SelectWebhookNoRequired(database.getdatabase());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                NotFound();
                return new List<webhook>();
            }
        }

        [HttpGet("{id}")]
        public webhook Get(string id)
        {
            return webhook.SelectById(database.getdatabase(), id);
        }

        [HttpPut]
        public void PutRequired([FromBody] string id)
        {
            try
            {

                var element = webhook.SelectById(database.getdatabase(), id);
                if (element != null)
                {
                    element.required = true;
                    element.Update(database.getdatabase());
                }

                Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                NotFound();
            }

        }
    }
}
