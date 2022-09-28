using System.Collections.Generic;
using Snovio.Structs;
namespace Snovio.Structs;

public struct CurrentJob
{
    public string companyName { get; set; }
    public string position { get; set; }
    public string socialLink { get; set; }
    public string site { get; set; }
    public string locality { get; set; }
    public string state { get; set; }
    public string city { get; set; }
    public string street { get; set; }
    public string street2 { get; set; }
    public string postal { get; set; }
    public string founded { get; set; }
    public string startDate { get; set; }
    public string endDate { get; set; }
    public string size { get; set; }
    public string industry { get; set; }
    public string companyType { get; set; }
    public string country { get; set; }
}

public struct Data
{
    public string id { get; set; }
    public string name { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string sourcePage { get; set; }
    public string source { get; set; }
    public string industry { get; set; }
    public string country { get; set; }
    public string locality { get; set; }
    public List<string> skills { get; set; }
    public object links { get; set; }
    public List<CurrentJob> currentJob { get; set; }
    public List<PreviousJob> previousJob { get; set; }
    public List<string> social { get; set; }
    public List<Email> emails { get; set; }

}

public struct PreviousJob
{
    public string companyName { get; set; }
    public string position { get; set; }
    public string socialLink { get; set; }
    public string site { get; set; }
    public string locality { get; set; }
    public string state { get; set; }
    public string city { get; set; }
    public string street { get; set; }
    public string street2 { get; set; }
    public string postal { get; set; }
    public string founded { get; set; }
    public string startDate { get; set; }
    public string endDate { get; set; }
    public string size { get; set; }
    public string industry { get; set; }
    public string companyType { get; set; }
    public string country { get; set; }
}
public struct ProspectStruct
{
    public bool success { get; set; }
    public Data data { get; set; }
}