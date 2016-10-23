0. Games are supposed to share plugin interface to be (un)loaded dynamically
1. ћодули, реализующий IModule (интерфейс находитс€ в Prism.Modularity, сборка Prism.Wpf), в Initialize могут регистрировать
   свои внутренние типы в сервислокаторе
2. –егистрацию модулевых вьюх делает UI, дабы модули не об€заны были знать структуру гу€.
   «абота модул€ - показать наружу через IUiModule тип рутового контрола, который в себе зарегистрирует UI.