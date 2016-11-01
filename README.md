0. Games are supposed to share plugin interface to be (un)loaded dynamically

1. Любые сборки, реализующий IModule (интерфейс находится в Prism.Modularity, сборка Prism.Wpf), в Initialize могут регистрировать
   свои внутренние типы в сервислокаторе - так оно и сделано. ShellBootstrapper должен лишь добавить модуль сборки в CreateModuleCatalog();

2. Регистрацию модулевых вьюх делает UI, дабы модули не обязаны были знать структуру гуя.
   Забота модуля - показать наружу через IUiModule тип рутового контрола, который в себе зарегистрирует UI.

3. Initialize viewmodels automatically by using Catel controls - they call viewmodel.InitializeAsync() internally when view is loaded.

4. Game plugin must provide a <Plugin Name>.config file to make it possible for Freengy host to discover main plugin view without loading
   assembly. SampleGame.dll.config is an example. Config must have an entry containing main view's full type name. This entry must be
   application-scope.
   <setting name="MainGameView" .. >
       <value>Freengy.SampleGame.Views.SampleGameUi</value>
   </setting>
   Plugin also can ensure config validity by implementing IModule (put some validation logic in Initialize()).