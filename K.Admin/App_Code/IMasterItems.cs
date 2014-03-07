using System;

public interface IMasterItems
{
    ServerObject ServerObject { get; }
    Credits.Module ModuleCredits { get;  }
    Accounting.Module ModuleAccounting { get;  }
    Security.Module ModuleSecurity { get; }
    Security.MainModule ModuleMain { get; }
    Security.User UserObject { get; }
    void PerformPreloadActions(string currentModuleId, string pageName);
    bool PermissionAllowed(string moduleName, string domainName, Constants.Constants.Classifiers permission);

    bool AutentificatedUser { get; }   
}



