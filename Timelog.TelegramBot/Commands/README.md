## Соглашение по обработчикам комманд

* Наименование классов, содержащих обработчики команд, должно заканчиваться на Commands
* Методы-обработчики команд должны иметь сигнатуру делегата CommandExecuteHandler и аннотироватся аттрибутом CommandBind.
Пример:
'''
	[CommandBind("/start")]
    public async Task StartCommand(ITelegramBotClient botClient, UpdateRequest updateRequest)
    {
        await botClient.SendTextMessageAsync(updateRequest.TelegramChatId, "Добро пожаловать!");
    }
'''
* Методы-валидаторы комманд должны иметь сигнатуру делегата CommandValidateHandler и аннотироватся аттрибутом CommandValidate.
Пример:
'''
    [CommandValidate("/project")]
    public async Task<bool> ValidateProjectCommandAsync(UpdateRequest commandRequest)
    {
        return await Task.FromResult<bool>(ValidateProjectCommand(commandRequest));
    }
'''