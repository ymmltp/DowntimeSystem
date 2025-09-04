
// See https://aka.ms/new-console-template for more information
string basicURL = "http://cnwuxm1tes05";
string url = "/eCalling/InsertData2?comfrom=Downtime System&department=ME&machine=Bay54M010&ErrorCode=频繁抛料&ErrorDescription=呼叫ME，频繁抛料&Server=CNWUXPRD0752&isDowntime=true";
RestClient client = new RestClient(basicURL);
string result = client.PostWithOutBody(url);
Console.WriteLine(result);

