-module(my_port).
-export([start/0, call/2, stop/1]).

start() ->
    %% Update the path to your C# executable
    Command = "/home/morgan/Documents/Programming/PortsErlToC#/MyPortProgram/publish/MyPortProgram",
    Port = open_port({spawn, Command}, [{packet, 2}, binary, exit_status]),
    Port.

call(Port, Message) when is_binary(Message) ->
    port_command(Port, Message),
    receive
        {Port, {data, Data}} ->
            Data;
        {Port, closed} ->
            {error, closed};
        {'EXIT', Port, Reason} ->
            {error, Reason}
    after 5000 ->
        {error, timeout}
    end.

stop(Port) ->
    port_close(Port).