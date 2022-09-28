using Snovio;

var sno = new SnovioTools("e1254fd5a3dc7facd8fc51dc8b0bf154", "61628d97decab9d4c528946bffcebec0");
var sn = sno.getProspectFromUrL("https://www.linkedin.com/in/davesaenz/").GetAwaiter().GetResult();

if (sn.data.emails.Count > 0)
{
    Console.WriteLine(sn.data.emails[0].email);
    Console.WriteLine(sn.data.emails.Count);
}
else
{
    Console.WriteLine(sn.data.emails.Count);
    Console.WriteLine("no hay email");
}