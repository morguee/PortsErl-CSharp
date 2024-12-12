# Connecting Erlang and CSharp using Ports

**Must have .NET8.0 and OTP 25+

Edit line 6 in my_port.erl with your path to CSharp executable 
Mine is this:
```
/home/morgan/Documents/Programming/PortsErlToC#/MyPortProgram/publish/MyPortProgram
```
Also might need to ensure that it has execute permission
```
chmod +x /home/morgan/Documents/Programming/PortsErlToC#/MyPortProgram/publish/MyPortProgram
```
Start the beam and communicate with the csharp port:
```
erl
```
```
c(my_port).
```
```
Port = my_port:start().
```
```
Response = my_port:call(Port, <<"Hello, C#">>).
```
```
my_port:stop(Port).
```