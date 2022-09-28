using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Quartz;
using PuppeteerSharp;
using Linkedin.Net.Models;
using Linkedin.Net.Db;
using PuppeteerSharp.Input;
using Serilog;

namespace MailBot.Browser.controllers
{
    internal class BrowserJob : IJob
    {
        //propiedades
        public string pathcookiesfile
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "cookies", Environment.GetEnvironmentVariable("usermail") + ".json");
            }
        }

        public bool cookieExist { get; set; }

        //procesos
        public void SaveCookie(Page page, Mutex mutex)
        {

            try
            {
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "cookies")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "cookies"));
                }

                if (mutex.WaitOne())
                {
                    if (!ValidateCookie())
                    {
                        var cookie = page.GetCookiesAsync().GetAwaiter().GetResult();
                        File.WriteAllText(pathcookiesfile, JsonSerializer.Serialize(cookie));
                    }
                }
                //var cookiesave =accounts.UpdateCookie( database, Linkedin.Net.Cookies.Cookie.CookieHeader(Path.Combine("cookies", account.account_mail + ".json")),account.account_id.ToString());
                //if (cookiesave){Console.WriteLine("-> Cookie actualizada");}else{Console.WriteLine("-> Error Cookie no actualizada");}
            }
            catch (Exception ex)
            {
                Log.Fatal("No se pudo guardar la cookie");
                Log.Fatal(ex.Message);

            }
            finally { mutex.ReleaseMutex(); }
        }

        public bool ValidateCookie()
        {

            try
            {
                if (cookieExist)
                {
                    var cookie = JsonSerializer.Deserialize<CookieParam[]>(File.ReadAllText(pathcookiesfile));
                    var defaultDouble = 1.0;
                    var isValidConvert = double.TryParse(cookie.ToList().Find(x => x.Name == "JSESSIONID").Expires.ToString(), out defaultDouble);

                    if (isValidConvert == false) return false;
                    var dateExpire = stacks.UnixTimeStampToDateTime(double.Parse(cookie.ToList().Find(x => x.Name == "JSESSIONID").Expires.ToString()));

                    return ((dateExpire - DateTime.Now).TotalDays < 1);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return cookieExist;
        }

        public void SetCookie(Page page, Mutex mutex)
        {
            try
            {
                if (mutex.WaitOne())
                {
                    cookieExist = File.Exists(pathcookiesfile);
                    if (File.Exists(pathcookiesfile))
                    {
                        CookieParam cookieParam = new CookieParam();
                        page.SetCookieAsync(JsonSerializer.Deserialize<CookieParam[]>(File.ReadAllText(pathcookiesfile))).Wait();
                        Log.Information("-> cookies aplicadas");
                    }
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        //Ejecucion
        public async Task Execute(IJobExecutionContext context)
        {
            var cliente = clients.SelectById(app.app.getMutexDatabase, context.MergedJobDataMap["client_id"].ToString());

            PuppeteerSharp.Browser browser;

            //creamos el navegador
            Log.Information("Descargando el navegador");
            if (app.app.getMutexDatabase.WaitOne())
            {
                new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision).Wait();
                app.app.getMutexDatabase.ReleaseMutex();
            }
            Log.Information("Navegador descargado");

            //Launcher the browser
            browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = false, DefaultViewport = new ViewPortOptions { Width = 600, Height = 1200 }, Args = new[] { $"--proxy-server={Environment.GetEnvironmentVariable("proxy_ip")}:{Environment.GetEnvironmentVariable("proxy_port")}" } });

            //create a new page and go to linkedin
            Page page = await browser.NewPageAsync();
            await page.AuthenticateAsync(new Credentials { Username = Environment.GetEnvironmentVariable("proxy_user"), Password = Environment.GetEnvironmentVariable("proxy_password") });

            SetCookie(page, app.app.getMutexCookie);

            try
            {
                //ingresamos a Linkedin
                var resgoto = await page.GoToAsync("https://www.linkedin.com/login/", 120000, new[] { WaitUntilNavigation.DOMContentLoaded });
                await Task.Delay(5000);
            }
            catch (NavigationException ex)
            {
                Log.Fatal("La tarea no se pudo completar, el tiempo de espera ha excedido el limite establecido");
                Log.Fatal($"Error: {ex.Message}");
                return;
            }

            if (!ValidateCookie())
            {
                //investigar el porque no funciona la validacion
                Log.Information("-> Ejecutando login using mail n user");
                try
                {
                    await page.TypeAsync("#username", Environment.GetEnvironmentVariable("usermail"));
                    await page.TypeAsync("#password", Environment.GetEnvironmentVariable("passwordmail"));
                    await page.ClickAsync("button[type='submit']", new ClickOptions { Delay = new Random().Next(1, 3) * new Random().Next(10, 20) });
                    await Task.Delay(10000);

                    await page.WaitForNavigationAsync();
                }
                catch (Exception ex)
                {
                    Log.Fatal("-> Error ingresando usuario");
                    Log.Fatal(ex.Message);
                }
            }

            await Task.Delay(1000);

            if (page.Url != "https://www.linkedin.com/feed/") return;

            Log.Information("Guardando la cookie");
            SaveCookie(page, app.app.getMutexCookie);
            Log.Information("Cookie guardada");

            try
            {
                //ingresamos a Linkedin
                Log.Information("Obteniendo la nueva url");
                var resgoto = await page.GoToAsync(cliente.client_url.ToString(), 120000, new[] { WaitUntilNavigation.DOMContentLoaded });
                //await page.WaitForNavigationAsync();
                Console.WriteLine(page.Url);
                cliente.client_url = page.Url;
                cliente.client_url_update = true;

                cliente.Update(app.app.getMutexDatabase);
                await Task.Delay(5000);
                Log.Information("Url actualizada con exito");
            }
            catch (NavigationException ex)
            {
                Log.Fatal("La tarea no se pudo completar, el tiempo de espera ha excedido el limite establecido");
                Log.Fatal($"Error: {ex.Message}");
                return;
            }

            Log.Information("Tarea Terminada");
            await browser.CloseAsync();
        }

    }
}
