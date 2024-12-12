# Connecting Erlang and CSharp using Ports

**Must have .NET8.0 and OTP 25+

Edit line 6 in my_port.erl with your path to CSharp executable 
Mine is this:
```
/home/morgan/Documents/Programming/PortsErl-CSharp/MyPortProgram/publish/MyPortProgram
```
Also might need to ensure that it has execute permission
```
chmod +x /home/morgan/Documents/Programming/PortsErl-CSharp/MyPortProgram/publish/MyPortProgram
```
Start the beam and communicate with the csharp port:
```
erl
```
```
c(my_port).
```
Expectation: {ok,my_port}
```
Port = my_port:start().
```
Expectation: #Port<0.4>  ...or something similar
If path to exe is wrong: sh: 1: exec: /home/morgan/...: not found
```
my_port:call(Port, {csharp, multiply, [9, 9]}).
```
Expectation: 81
```
my_port:call(Port, {csharp, add, [9, 9]}).
```
Expectation: 18
```
my_port:call(Port, {csharp, subtract, [9, 9]}).
```
Expectation: 0

CTRL + C to exit or:
```
my_port:stop(Port).
```
Expectation: true

**If you make edits to the Program.cs file do this:
```
cd MyPortProgram
dotnet publish -c Release -r linux-x64 --self-contained false -p:PublishSingleFile=true -o publish
```
```
cd ..
```