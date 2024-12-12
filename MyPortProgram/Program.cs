using System;
using System.IO;
using System.Text;

namespace MyPortProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read from stdin, write to stdout
            var stdin = Console.OpenStandardInput();
            var stdout = Console.OpenStandardOutput();

            var lengthBuffer = new byte[2];

            while (true)
            {
                // Read 2 bytes for the length prefix
                int bytesRead = ReadFully(stdin, lengthBuffer, 0, 2);
                if (bytesRead == 0)
                {
                    // End of stream
                    break;
                }
                if (bytesRead < 2)
                {
                    // Incomplete length prefix
                    break;
                }

                // Get the message length (big-endian)
                int messageLength = (lengthBuffer[0] << 8) | lengthBuffer[1];

                if (messageLength == 0)
                {
                    continue;
                }

                var messageBuffer = new byte[messageLength];
                bytesRead = ReadFully(stdin, messageBuffer, 0, messageLength);

                if (bytesRead < messageLength)
                {
                    // Incomplete message
                    break;
                }

                // Convert the message to a string
                string message = Encoding.UTF8.GetString(messageBuffer);
                
                // Process the message
                string response = ProcessMessage(message);

                // Encode the response
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                int responseLength = responseBytes.Length;

                // Write the length prefix (big-endian)
                byte[] responseLengthBuffer = new byte[2];
                responseLengthBuffer[0] = (byte)((responseLength >> 8) & 0xFF);
                responseLengthBuffer[1] = (byte)(responseLength & 0xFF);

                stdout.Write(responseLengthBuffer, 0, 2);
                stdout.Write(responseBytes, 0, responseLength);
                stdout.Flush();
            }
        }

        static int ReadFully(Stream stream, byte[] buffer, int offset, int count)
        {
            int totalRead = 0;
            while (totalRead < count)
            {
                int bytesRead = stream.Read(buffer, offset + totalRead, count - totalRead);
                if (bytesRead == 0)
                {
                    break;
                }
                totalRead += bytesRead;
            }
            return totalRead;
        }

        static string ProcessMessage(string message)
        {
            // Check for commands
            if (message.StartsWith("REVERSE "))
            {
                string toReverse = message.Substring(8);
                char[] charArray = toReverse.ToCharArray();
                Array.Reverse(charArray);
                return new string(charArray);
            }

            // Default behavior: echo the message back with a prefix
            return $"Received: {message}";
        }
    }
}