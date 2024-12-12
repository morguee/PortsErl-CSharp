-module(my_port).
-export([start/0, call/2, stop/1]).

start() ->
    %% Update the path to your C# executable
    Command = "/home/morgan/Documents/Programming/PortsErl-CSharp/MyPortProgram/publish/MyPortProgram",
    Port = open_port({spawn, Command}, [{packet, 2}, binary, exit_status]),
    Port.

call(Port, {Module, Function, Args}) ->
    %% Format the message as "Module:Function(Args)"
    ArgsString = lists:flatten(io_lib:format("~s", [string:join([integer_to_list(A) || A <- Args], ",")])),
    Message = io_lib:format("~s:~s(~s)", [atom_to_list(Module), atom_to_list(Function), ArgsString]),
    BinaryMessage = list_to_binary(lists:flatten(Message)),
    % io:format("Sending message: ~p~n", [BinaryMessage]),
    port_command(Port, BinaryMessage),
    receive
        {Port, {data, Data}} ->
            case catch binary_to_integer(Data) of
                {'EXIT', _} -> {error, invalid_data};
                Int -> Int
            end;
        {Port, closed} ->
            {error, closed};
        {'EXIT', Port, Reason} ->
            {error, Reason}
    after 5000 ->
        {error, timeout}
    end.

stop(Port) ->
    port_close(Port).