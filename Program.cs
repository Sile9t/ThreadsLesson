using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ThreadsLesson
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string site = "ya.ru";
            //get available site addresses to ping them
            var IPAddresses = Dns.GetHostAddresses(site, AddressFamily.InterNetwork);
            foreach (var item in IPAddresses)
                Console.WriteLine(item);
            
            var pings = new Dictionary<IPAddress, long>();
            var threads = new List<Thread>();
            //adding available addresses with reply time in dictionary
            foreach (var address in IPAddresses)
            {
                var thread = new Thread(() =>
                {
                    var ping = new Ping();
                    var reply = ping.Send(address);
                    pings.Add(address, reply.RoundtripTime);
                    Console.WriteLine($"{address} : {reply.RoundtripTime}");
                });
                threads.Add(thread);
                thread.Start();
            }
            //pinging addresses
            foreach (var thread in threads)
                thread.Join();
            var minPing = pings.Min(x => x.Value);
            Console.WriteLine($"Min ping = {minPing}");
        }
    }
}
