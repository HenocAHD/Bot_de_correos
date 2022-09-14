using System;
using System.IO;
using System.Text.Json;
using Snovio.Structs;
namespace Snovio.Objects;

public class AccessToken
{
    public string _directory = Path.Join(Directory.GetCurrentDirectory(), "Token");
    public AccessTokenStruct _token { get; set; } 
    public string _directoryToken()
    {
        return Path.Join(_directory, "Token.json");
    }

    public void validationDirectory()
    {
            
        if (!Directory.Exists(_directory))
        {
            Console.WriteLine("-> El directorio no exixte" +
                              "\n-> Creando el directorio...");
            Directory.CreateDirectory(_directory);
        }
            
    }
        
    public bool validationToken()
    {
        if (!File.Exists(_directoryToken()))
        {
            Console.WriteLine("El token no existe");
            return false;
        }
        else
        {
            this._token = JsonSerializer.Deserialize<AccessTokenStruct>(File.ReadAllText(_directoryToken()));
            bool tokenExpired = (_token.expires - DateTime.Now).TotalSeconds <= 0;
            if (tokenExpired)
            {
                Console.WriteLine("El token expiro");
                return false;
            }

            return true;
        }
    }

    public bool saveToken(AccessTokenStruct token)
    {
        try
        {
            token.expires=DateTime.Now.AddSeconds(token.expires_in);
            var json = JsonSerializer.Serialize(token);
            File.WriteAllText(_directoryToken(), json);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
