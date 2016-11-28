** Common **

  *0.* Games are supposed to share plugin interface to be (un)loaded dynamically

  *1.* Any assembly that implement `IModule` (located in Prism.Modularity => Pris.Wpf) can register their public interfaces to internal 
       classes in a service locator. ShellBootstrapper must just add assembly's module to his `CreateModuleCatalog()`.

  *2.* Freengy.UI registers pieces of UI (like Freengy.GameList) in ShellBootstrapper. In order to be registered, UI module must expose 
       its main view type through IUiModule implementation.

  *3.* Initialize viewmodels automatically by using Catel controls - they call `viewmodel.InitializeAsync()` internally when view is loaded.


** Plugin System **

  *4.* Game plugin must provide a <Plugin Name>.config file to make it possible for Freengy host to discover main plugin view without loading
       assembly. SampleGame.dll.config is an example. Config must have an entry containing main view's full type name. This entry must be
       application-scope.
       <setting name="MainGameView" .. >
           <value>Freengy.SampleGame.Views.SampleGameUi</value>
       </setting>
       Plugin also can ensure config validity by implementing IModule (put some validation logic in `Initialize()`).


** Dependency Injection **

  *5.* I do not use explicit dependency injection, though it may be considered not a good practice. But all resolved services and types
       are guaranteed to be pre-registered by their assemblies at startup. Yes, unit-testing may become some more implicit and cofusing - 
       not injecting mocks directly, but registering them in service locator.


** Database **

  *6.* Modules's settingish poco classes are located in Freengy.Settings - somewhat not very good, but I failed to pass theese pocos to
       ORM when they are in their logical modules.

  *7.* Freengy's databse is created by running \output\tools\FluentMigrator.1.6.2\RunMigration.bat script.
       In debug mode solution has to be built before using script, as it uses output assemblies.