0. Games are supposed to share plugin interface to be (un)loaded dynamically
1. ����� ������, ����������� IModule (��������� ��������� � Prism.Modularity, ������ Prism.Wpf), � Initialize ����� ��������������
   ���� ���������� ���� � �������������� - ��� ��� � �������. ShellBootstrapper ������ ���� �������� ������ ������ � CreateModuleCatalog();
2. ����������� ��������� ���� ������ UI, ���� ������ �� ������� ���� ����� ��������� ���.
   ������ ������ - �������� ������ ����� IUiModule ��� �������� ��������, ������� � ���� �������������� UI.