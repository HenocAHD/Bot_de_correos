using Newtonsoft.Json;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using Linkedin.Net.Db;
using Linkedin.Net.Models;
using Linkedin.Net.Models.Interfaces;
using System.Text.Json.Serialization;

namespace Webhook_Api.Models
{
    public class webhook : IModel
    {
        private DateTime _create_at;

        public int id_webhook_data { get; set; }
        public string email { get; set; }
        [JsonProperty("event")]
        [JsonPropertyName("event")]
        public string evento { get; set; }
        public string ip { get; set; }
        public string sg_content_type { get; set; }
        public string sg_event_id { get; set; }
        public bool sg_machine_open { get; set; }
        public string sg_message_id { get; set; }
        public int timestamp { get; set; }
        public string useragent { get; set; }
        public bool required { get; set; }
        public DateTime create_at
        {
            get
            {
                if (_create_at == DateTime.MinValue)
                {
                    _create_at = DateTime.Now;
                }
                return _create_at;
            }
            set
            {
                _create_at = value;
            }
        }
        public bool Create(database database)
        {
            using (var db = database.connection)
            {
                var sql = "";
                if (id_webhook_data != 0)
                {
                    sql = $"INSERT INTO webhook_data (id_webhook_data, email, evento, ip, sg_content_type, sg_event_id, sg_machine_open, sg_message_id, timestamp, useragent, create_at) VALUES (@id_webhook_data, @email, @evento, @ip, @sg_content_type, @sg_event_id, @sg_machine_open, @sg_message_id, @timestamp, @useragent, DATE_FORMAT(@create_at,'%Y-%m-%d %H:%i:%s')); ";
                }
                else
                {
                    sql = $"INSERT INTO webhook_data (id_webhook_data, email, evento, ip, sg_content_type, sg_event_id, sg_machine_open, sg_message_id, timestamp, useragent, create_at) VALUES (DEFAULT, @email, @evento, @ip, @sg_content_type, @sg_event_id, @sg_machine_open, @sg_message_id, @timestamp, @useragent, DATE_FORMAT(@create_at,'%Y-%m-%d %H:%i:%s')); ";
                }
                var result = db.Execute(sql, this);
                return (result > 0);
            }

        }
        public bool Delete(database database)
        {
            using (var db = database.connection)
            {
                var sql = "DELETE FROM webhook_data WHERE id_webhook_data = @id_webhook_data;";
                var result = db.Execute(sql, this);
                return (result > 0);
            }
        }
        public bool Update(database database)
        {
            using (var db = database.connection)
            {
                var sql = "UPDATE webhook_data SET email = @email, evento = @evento, ip = @ip, sg_content_type = @sg_content_type, sg_event_id = @sg_event_id, sg_machine_open = @sg_machine_open, sg_message_id = @sg_message_id, timestamp = @timestamp, useragent = @useragent, required = @required WHERE id_webhook_data = @id_webhook_data;";
                var result = db.Execute(sql, this);
                return (result > 0);
            }
        }

        public static List<webhook> SelectWebhookNoRequired(database database)
        {
            using (var db = database.connection)
            {
                var sql = "select * from webhook_data where required = false;";
                var result = db.Query<webhook>(sql);
                return (result.ToList());
            }
        }

        public static webhook SelectById(database database, string id_webhook_data)
        {
            using (var db = database.connection)
            {
                var sql = $"select * from webhook_data where id_webhook_data = {id_webhook_data}";
                var result = db.QueryFirstOrDefault<webhook>(sql);
                return (result);
            }
        }
    }
}
