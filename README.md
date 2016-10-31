0. Games are supposed to share plugin interface to be (un)loaded dynamically

1. Любые сборки, реализующий IModule (интерфейс находится в Prism.Modularity, сборка Prism.Wpf), в Initialize могут регистрировать
   свои внутренние типы в сервислокаторе - так оно и сделано. ShellBootstrapper должен лишь добавить модуль сборки в CreateModuleCatalog();

2. Регистрацию модулевых вьюх делает UI, дабы модули не обязаны были знать структуру гуя.
   Забота модуля - показать наружу через IUiModule тип рутового контрола, который в себе зарегистрирует UI.

3. Initialize viewmodels automatically by using Catel controls - they call viewmodel.InitializeAsync() internally when view is loaded.