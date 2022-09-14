using System.Collections.Generic;
namespace Snovio.Structs;

public struct DataEmails
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public List<Email> emails { get; set; }
}

public struct Email
{
    public string email { get; set; }
    public string emailStatus { get; set; }
}

public struct Params
{
    public string domain { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
}

public struct ProspectEmailsStruct
{
    public bool success { get; set; }
    public DataEmails data { get; set; }
    public Params @params { get; set; }
    public Status status { get; set; }
}

public struct Status
{
    public string identifier { get; set; }
    public string description { get; set; }
}