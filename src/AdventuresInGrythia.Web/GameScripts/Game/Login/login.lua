--local output = Output;
--local formatter = Formatter;
--function execute()
--	local str = formatter.Write("Welcome to <#yellow>Arcana Aeterna<#> <#cyan>LUA<#> style!\n-=-=-=-=-=-=-=-=-=-=");
--	output.SendText(str);
--end

function getTitle()
	return "Welcome to <#cyan>Adventures in Grythia<#>.";
end

function motd()
	motd = "-=-=-=-=-=-=-=-=-=-=-\n" .. "Message of the Day...\n" .. "-=-=-=-=-=-=-=-=-=-=-\n";
	return motd;
end
