using ChatUser;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

class Program
{
    static string userName;
    private const string host = "127.0.0.1";
    private const int port = 8888;
    static string receiverName;
    static TcpClient client;
    static NetworkStream stream;
    static void Main(string[] args)
    {
        Console.Write("Введите свое имя: ");
        userName = Console.ReadLine();
        Console.Write("Введите имя получателя: ");
        receiverName = Console.ReadLine();

        client = new TcpClient();
        try
        {
            client.Connect(host, port);
            stream = client.GetStream();

            var message = new Message()
            {
                SenderName = userName,
                ReceiverName = receiverName,
                Content = userName
            };

            var jsonMessage = JsonConvert.SerializeObject(message);
            byte[] data = Encoding.UTF8.GetBytes(jsonMessage);
            stream.Write(data, 0, data.Length);

            Thread receiveThread = new Thread(ReceiveMessage);
            receiveThread.Start();
            Console.WriteLine($"Добро пожаловать {userName}");
            SendMessage();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Disconnect();
        }
    }

    static void SendMessage()
    {
        Console.WriteLine("Введите сообщение: ");
        while (true)
        {
            string content = Console.ReadLine();
            var message = new Message()
            {
                SenderName = userName,
                ReceiverName = receiverName,
                Content = content
            };

            var jsonMessage = JsonConvert.SerializeObject(message);
            byte[] data = Encoding.UTF8.GetBytes(jsonMessage);
            stream.Write(data, 0, data.Length);
        }
    }

    static void ReceiveMessage()
    {
        while (true)
        {
            try
            {
                byte[] data = new byte[64];
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);

                string message = builder.ToString();
                Console.WriteLine(message);
            }
            catch
            {
                Console.WriteLine("Подключение прервано!"); //соединение было
                Console.ReadLine();
                Disconnect();
            }
        }
    }

    static void Disconnect()
    {
        if (stream != null)
            stream.Close();
        if (client != null)
            client.Close();

        Environment.Exit(0);
    }
}